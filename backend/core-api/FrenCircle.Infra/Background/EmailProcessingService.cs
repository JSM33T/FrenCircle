using FrenCircle.Infra.Repositories.Interfaces;
using FrenCircle.Infra.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrenCircle.Infra.Background
{
    public class EmailProcessingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailQueue _emailQueue;
        private readonly ILogger<EmailProcessingService> _logger;
        private readonly int _maxRetryAttempts = 3;
        private readonly int _retryDelaySeconds = 5;

        public EmailProcessingService(
            IServiceProvider serviceProvider,
            EmailQueue emailQueue,
            ILogger<EmailProcessingService> logger)
        {
            _serviceProvider = serviceProvider;
            _emailQueue = emailQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email processing service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await foreach (var emailItem in _emailQueue.Reader.ReadAllAsync(stoppingToken))
                    {
                        await ProcessEmailItemAsync(emailItem, stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when cancellation is requested
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in email processing service");
                    await Task.Delay(5000, stoppingToken); // Wait before retrying
                }
            }

            _logger.LogInformation("Email processing service stopped");
        }

        private async Task ProcessEmailItemAsync(QueuedEmailItem emailItem, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var mailRepository = scope.ServiceProvider.GetRequiredService<IMailRepository>();

            try
            {
                _logger.LogDebug("Processing email item: {EmailId}, Type: {Type}", emailItem.Id, emailItem.Type);

                var success = emailItem.Type switch
                {
                    EmailQueueItemType.SingleEmail => await ProcessSingleEmailAsync(mailRepository, emailItem, cancellationToken),
                    EmailQueueItemType.BulkEmail => await ProcessBulkEmailAsync(mailRepository, emailItem, cancellationToken),
                    EmailQueueItemType.TemplateEmail => await ProcessTemplateEmailAsync(mailRepository, emailItem, cancellationToken),
                    _ => false
                };

                if (success)
                {
                    emailItem.ProcessedAt = DateTime.UtcNow;
                    _logger.LogDebug("Email item processed successfully: {EmailId}", emailItem.Id);
                }
                else
                {
                    await HandleEmailFailureAsync(emailItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing email item: {EmailId}", emailItem.Id);
                emailItem.ErrorMessage = ex.Message;
                await HandleEmailFailureAsync(emailItem);
            }
        }

        private async Task<bool> ProcessSingleEmailAsync(IMailRepository mailRepository, QueuedEmailItem emailItem, CancellationToken cancellationToken)
        {
            if (emailItem.EmailMessage == null)
            {
                _logger.LogError("EmailMessage is null for item: {EmailId}", emailItem.Id);
                return false;
            }

            var result = await mailRepository.SendEmailAsync(emailItem.EmailMessage, cancellationToken);
            
            if (!result.Success)
            {
                emailItem.ErrorMessage = result.ErrorMessage;
                _logger.LogWarning("Failed to send email: {EmailId}, Error: {Error}", emailItem.Id, result.ErrorMessage);
            }

            return result.Success;
        }

        private async Task<bool> ProcessBulkEmailAsync(IMailRepository mailRepository, QueuedEmailItem emailItem, CancellationToken cancellationToken)
        {
            if (emailItem.BulkEmailMessage == null)
            {
                _logger.LogError("BulkEmailMessage is null for item: {EmailId}", emailItem.Id);
                return false;
            }

            var result = await mailRepository.SendBulkEmailAsync(emailItem.BulkEmailMessage, cancellationToken);
            
            if (!result.Success)
            {
                emailItem.ErrorMessage = $"Bulk email failed. Success: {result.SuccessfulEmails}, Failed: {result.FailedEmails}";
                _logger.LogWarning("Bulk email partially failed: {EmailId}, Success: {Success}, Failed: {Failed}", 
                    emailItem.Id, result.SuccessfulEmails, result.FailedEmails);
            }

            return result.Success;
        }

        private async Task<bool> ProcessTemplateEmailAsync(IMailRepository mailRepository, QueuedEmailItem emailItem, CancellationToken cancellationToken)
        {
            if (emailItem.TemplateType == null || emailItem.Recipient == null || emailItem.Variables == null)
            {
                _logger.LogError("Template email properties are incomplete for item: {EmailId}", emailItem.Id);
                return false;
            }

            var result = await mailRepository.SendTemplateEmailAsync(
                emailItem.TemplateType.Value, 
                emailItem.Recipient, 
                emailItem.Variables, 
                cancellationToken);
            
            if (!result.Success)
            {
                emailItem.ErrorMessage = result.ErrorMessage;
                _logger.LogWarning("Failed to send template email: {EmailId}, Template: {Template}, Error: {Error}", 
                    emailItem.Id, emailItem.TemplateType, result.ErrorMessage);
            }

            return result.Success;
        }

        private async Task HandleEmailFailureAsync(QueuedEmailItem emailItem)
        {
            emailItem.RetryCount++;

            if (emailItem.RetryCount < _maxRetryAttempts)
            {
                _logger.LogInformation("Retrying email item: {EmailId}, Attempt: {Attempt}/{MaxAttempts}", 
                    emailItem.Id, emailItem.RetryCount, _maxRetryAttempts);
                
                // Wait before retrying
                await Task.Delay(_retryDelaySeconds * 1000 * emailItem.RetryCount);
                
                // Re-queue the item for retry
                switch (emailItem.Type)
                {
                    case EmailQueueItemType.SingleEmail when emailItem.EmailMessage != null:
                        await _emailQueue.EnqueueEmailAsync(emailItem.EmailMessage);
                        break;
                    case EmailQueueItemType.BulkEmail when emailItem.BulkEmailMessage != null:
                        await _emailQueue.EnqueueBulkEmailAsync(emailItem.BulkEmailMessage);
                        break;
                    case EmailQueueItemType.TemplateEmail when emailItem.TemplateType != null && emailItem.Recipient != null && emailItem.Variables != null:
                        await _emailQueue.EnqueueTemplateEmailAsync(emailItem.TemplateType.Value, emailItem.Recipient, emailItem.Variables);
                        break;
                }
            }
            else
            {
                _logger.LogError("Email item failed permanently after {MaxAttempts} attempts: {EmailId}, Error: {Error}", 
                    _maxRetryAttempts, emailItem.Id, emailItem.ErrorMessage);
            }
        }
    }
}
