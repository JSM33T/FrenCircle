using FrenCircle.Contracts.Mail;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace FrenCircle.Infra.Services
{
    public interface IEmailQueue
    {
        Task EnqueueEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
        Task EnqueueBulkEmailAsync(BulkEmailMessage bulkEmailMessage, CancellationToken cancellationToken = default);
        Task EnqueueTemplateEmailAsync(EmailTemplateType templateType, EmailRecipient recipient, Dictionary<string, string> variables, CancellationToken cancellationToken = default);
        Task<int> GetQueueCountAsync();
    }

    public class EmailQueue : IEmailQueue
    {
        private readonly Channel<QueuedEmailItem> _channel;
        private readonly ILogger<EmailQueue> _logger;

        public EmailQueue(ILogger<EmailQueue> logger)
        {
            _logger = logger;
            
            // Create a channel with a bounded capacity
            var options = new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = false,
                SingleWriter = false
            };
            
            _channel = Channel.CreateBounded<QueuedEmailItem>(options);
        }

        public async Task EnqueueEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            var queuedItem = new QueuedEmailItem
            {
                Id = Guid.NewGuid(),
                Type = EmailQueueItemType.SingleEmail,
                EmailMessage = emailMessage,
                QueuedAt = DateTime.UtcNow
            };

            await _channel.Writer.WriteAsync(queuedItem, cancellationToken);
            _logger.LogDebug("Email queued: {EmailId} to {Recipient}", queuedItem.Id, emailMessage.To);
        }

        public async Task EnqueueBulkEmailAsync(BulkEmailMessage bulkEmailMessage, CancellationToken cancellationToken = default)
        {
            var queuedItem = new QueuedEmailItem
            {
                Id = Guid.NewGuid(),
                Type = EmailQueueItemType.BulkEmail,
                BulkEmailMessage = bulkEmailMessage,
                QueuedAt = DateTime.UtcNow
            };

            await _channel.Writer.WriteAsync(queuedItem, cancellationToken);
            _logger.LogDebug("Bulk email queued: {EmailId} with {RecipientCount} recipients", 
                queuedItem.Id, bulkEmailMessage.Recipients.Count);
        }

        public async Task EnqueueTemplateEmailAsync(EmailTemplateType templateType, EmailRecipient recipient, Dictionary<string, string> variables, CancellationToken cancellationToken = default)
        {
            var queuedItem = new QueuedEmailItem
            {
                Id = Guid.NewGuid(),
                Type = EmailQueueItemType.TemplateEmail,
                TemplateType = templateType,
                Recipient = recipient,
                Variables = variables,
                QueuedAt = DateTime.UtcNow
            };

            await _channel.Writer.WriteAsync(queuedItem, cancellationToken);
            _logger.LogDebug("Template email queued: {EmailId} template {TemplateType} to {Recipient}", 
                queuedItem.Id, templateType, recipient.Email);
        }

        public Task<int> GetQueueCountAsync()
        {
            // Channel doesn't provide exact count, so we return -1 to indicate unknown count
            // In a real implementation, you might want to track this separately
            return Task.FromResult(-1);
        }

        internal ChannelReader<QueuedEmailItem> Reader => _channel.Reader;
    }

    public class QueuedEmailItem
    {
        public Guid Id { get; set; }
        public EmailQueueItemType Type { get; set; }
        public DateTime QueuedAt { get; set; }
        public int RetryCount { get; set; } = 0;
        public DateTime? ProcessedAt { get; set; }
        public string? ErrorMessage { get; set; }

        // Single email properties
        public EmailMessage? EmailMessage { get; set; }

        // Bulk email properties
        public BulkEmailMessage? BulkEmailMessage { get; set; }

        // Template email properties
        public EmailTemplateType? TemplateType { get; set; }
        public EmailRecipient? Recipient { get; set; }
        public Dictionary<string, string>? Variables { get; set; }
    }

    public enum EmailQueueItemType
    {
        SingleEmail,
        BulkEmail,
        TemplateEmail
    }
}
