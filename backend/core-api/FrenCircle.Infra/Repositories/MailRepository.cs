using FrenCircle.Contracts.Mail;
using FrenCircle.Infra.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace FrenCircle.Infra.Repositories
{
    public class MailRepository : IMailRepository
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<MailRepository> _logger;
        private readonly Dictionary<EmailTemplateType, EmailTemplate> _templates;

        public MailRepository(IOptions<EmailSettings> emailSettings, ILogger<MailRepository> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
            _templates = new Dictionary<EmailTemplateType, EmailTemplate>();
            InitializeDefaultTemplates();
        }

        public async Task<EmailResult> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            try
            {
                var mimeMessage = CreateMimeMessage(emailMessage);
                var result = await SendMimeMessageAsync(mimeMessage, cancellationToken);
                
                if (_emailSettings.EnableLogging && result.Success)
                {
                    _logger.LogInformation("Email sent successfully to {To} with subject: {Subject}", 
                        emailMessage.To, emailMessage.Subject);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To} with subject: {Subject}", 
                    emailMessage.To, emailMessage.Subject);
                
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    SentAt = DateTime.UtcNow,
                    Provider = "SMTP"
                };
            }
        }

        public async Task<BulkEmailResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage, CancellationToken cancellationToken = default)
        {
            var result = new BulkEmailResult
            {
                TotalEmails = bulkEmailMessage.Recipients.Count,
                Results = new List<EmailResult>()
            };

            var batches = bulkEmailMessage.Recipients
                .Select((recipient, index) => new { recipient, index })
                .GroupBy(x => x.index / bulkEmailMessage.BatchSize)
                .Select(g => g.Select(x => x.recipient).ToList())
                .ToList();

            foreach (var batch in batches)
            {
                var batchTasks = batch.Select(async recipient =>
                {
                    var personalizedMessage = CreatePersonalizedMessage(bulkEmailMessage, recipient);
                    return await SendEmailAsync(personalizedMessage, cancellationToken);
                });

                var batchResults = await Task.WhenAll(batchTasks);
                result.Results.AddRange(batchResults);

                if (bulkEmailMessage.DelayBetweenBatchesMs > 0 && batches.IndexOf(batch) < batches.Count - 1)
                {
                    await Task.Delay(bulkEmailMessage.DelayBetweenBatchesMs, cancellationToken);
                }
            }

            result.SuccessfulEmails = result.Results.Count(r => r.Success);
            result.FailedEmails = result.Results.Count(r => !r.Success);
            result.Success = result.FailedEmails == 0;

            if (result.FailedEmails > 0)
            {
                result.Errors = result.Results
                    .Where(r => !r.Success && !string.IsNullOrEmpty(r.ErrorMessage))
                    .GroupBy(r => r.ErrorMessage!)
                    .ToDictionary(g => g.Key, g => g.Count().ToString());
            }

            _logger.LogInformation("Bulk email completed. Total: {Total}, Success: {Success}, Failed: {Failed}",
                result.TotalEmails, result.SuccessfulEmails, result.FailedEmails);

            return result;
        }

        public async Task<EmailResult> SendTemplateEmailAsync(
            EmailTemplateType templateType, 
            EmailRecipient recipient, 
            Dictionary<string, string> variables, 
            CancellationToken cancellationToken = default)
        {
            if (!_templates.TryGetValue(templateType, out var template))
            {
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = $"Email template '{templateType}' not found",
                    SentAt = DateTime.UtcNow,
                    Provider = "SMTP"
                };
            }

            var emailMessage = new EmailMessage
            {
                To = recipient.Email,
                ToName = recipient.Name,
                Subject = ReplaceVariables(template.Subject, variables),
                Body = ReplaceVariables(template.HtmlBody, variables),
                IsHtml = true
            };

            return await SendEmailAsync(emailMessage, cancellationToken);
        }

        public async Task<BulkEmailResult> SendBulkTemplateEmailAsync(
            EmailTemplateType templateType, 
            List<EmailRecipient> recipients, 
            Dictionary<string, string>? globalVariables = null,
            CancellationToken cancellationToken = default)
        {
            if (!_templates.TryGetValue(templateType, out var template))
            {
                return new BulkEmailResult
                {
                    Success = false,
                    TotalEmails = recipients.Count,
                    FailedEmails = recipients.Count,
                    Errors = new Dictionary<string, string> { { $"Template '{templateType}' not found", recipients.Count.ToString() } }
                };
            }

            var bulkEmailMessage = new BulkEmailMessage
            {
                Recipients = recipients.Select(r =>
                {
                    var mergedVariables = new Dictionary<string, string>(globalVariables ?? new Dictionary<string, string>());
                    if (r.PersonalizationData != null)
                    {
                        foreach (var kv in r.PersonalizationData)
                        {
                            mergedVariables[kv.Key] = kv.Value;
                        }
                    }
                    r.PersonalizationData = mergedVariables;
                    return r;
                }).ToList(),
                Subject = template.Subject,
                Body = template.HtmlBody,
                IsHtml = true
            };

            var results = new List<EmailResult>();
            foreach (var recipient in recipients)
            {
                var personalizedVariables = recipient.PersonalizationData ?? new Dictionary<string, string>();
                var result = await SendTemplateEmailAsync(templateType, recipient, personalizedVariables, cancellationToken);
                results.Add(result);
            }

            return new BulkEmailResult
            {
                TotalEmails = recipients.Count,
                SuccessfulEmails = results.Count(r => r.Success),
                FailedEmails = results.Count(r => !r.Success),
                Success = results.All(r => r.Success),
                Results = results
            };
        }

        public async Task<bool> ValidateEmailServiceAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, 
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None, cancellationToken);
                
                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
                }
                
                await client.DisconnectAsync(true, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email service validation failed");
                return false;
            }
        }

        public Dictionary<EmailTemplateType, EmailTemplate> GetAvailableTemplates()
        {
            return new Dictionary<EmailTemplateType, EmailTemplate>(_templates);
        }

        public void RegisterTemplate(EmailTemplateType templateType, EmailTemplate template)
        {
            _templates[templateType] = template;
            _logger.LogInformation("Email template '{TemplateType}' registered successfully", templateType);
        }

        private MimeMessage CreateMimeMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            
            message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromEmail));
            message.To.Add(new MailboxAddress(emailMessage.ToName ?? emailMessage.To, emailMessage.To));
            message.Subject = emailMessage.Subject;

            var bodyBuilder = new BodyBuilder();
            
            if (emailMessage.IsHtml)
            {
                bodyBuilder.HtmlBody = emailMessage.Body;
            }
            else
            {
                bodyBuilder.TextBody = emailMessage.Body;
            }

            // Add attachments
            if (emailMessage.Attachments != null)
            {
                foreach (var attachment in emailMessage.Attachments)
                {
                    using var stream = new MemoryStream(attachment.Content);
                    if (attachment.IsInline && !string.IsNullOrEmpty(attachment.ContentId))
                    {
                        var inlineAttachment = bodyBuilder.LinkedResources.Add(attachment.FileName, stream);
                        inlineAttachment.ContentId = attachment.ContentId;
                    }
                    else
                    {
                        bodyBuilder.Attachments.Add(attachment.FileName, stream, ContentType.Parse(attachment.ContentType));
                    }
                }
            }

            message.Body = bodyBuilder.ToMessageBody();

            // Set priority
            message.Priority = emailMessage.Priority switch
            {
                EmailPriority.High => MessagePriority.Urgent,
                EmailPriority.Low => MessagePriority.NonUrgent,
                _ => MessagePriority.Normal
            };

            // Add custom headers
            if (emailMessage.Headers != null)
            {
                foreach (var header in emailMessage.Headers)
                {
                    message.Headers.Add(header.Key, header.Value);
                }
            }

            return message;
        }

        private async Task<EmailResult> SendMimeMessageAsync(MimeMessage message, CancellationToken cancellationToken)
        {
            using var client = new SmtpClient();
            
            try
            {
                client.Timeout = _emailSettings.TimeoutSeconds * 1000;
                
                await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, 
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None, cancellationToken);
                
                if (!string.IsNullOrEmpty(_emailSettings.Username))
                {
                    await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password, cancellationToken);
                }
                
                var messageId = await client.SendAsync(message, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
                
                return new EmailResult
                {
                    Success = true,
                    MessageId = messageId,
                    SentAt = DateTime.UtcNow,
                    Provider = "SMTP"
                };
            }
            catch (Exception ex)
            {
                return new EmailResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    SentAt = DateTime.UtcNow,
                    Provider = "SMTP"
                };
            }
        }

        private EmailMessage CreatePersonalizedMessage(BulkEmailMessage bulkMessage, EmailRecipient recipient)
        {
            var personalizedSubject = bulkMessage.Subject;
            var personalizedBody = bulkMessage.Body;

            if (recipient.PersonalizationData != null)
            {
                personalizedSubject = ReplaceVariables(personalizedSubject, recipient.PersonalizationData);
                personalizedBody = ReplaceVariables(personalizedBody, recipient.PersonalizationData);
            }

            return new EmailMessage
            {
                To = recipient.Email,
                ToName = recipient.Name,
                Subject = personalizedSubject,
                Body = personalizedBody,
                IsHtml = bulkMessage.IsHtml,
                Attachments = bulkMessage.Attachments,
                Headers = bulkMessage.Headers,
                Priority = bulkMessage.Priority
            };
        }

        private string ReplaceVariables(string content, Dictionary<string, string> variables)
        {
            if (string.IsNullOrEmpty(content) || variables == null || !variables.Any())
                return content;

            var result = content;
            foreach (var variable in variables)
            {
                result = result.Replace($"{{{{{variable.Key}}}}}", variable.Value);
            }
            return result;
        }

        private void InitializeDefaultTemplates()
        {
            // Email Verification Template
            RegisterTemplate(EmailTemplateType.EmailVerification, new EmailTemplate
            {
                Name = "Email Verification",
                Subject = "Verify Your Email Address - {{AppName}}",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>Email Verification</h2>
                        <p>Hi {{UserName}},</p>
                        <p>Thank you for joining {{AppName}}! Please verify your email address by clicking the button below:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{VerificationLink}}' style='background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Verify Email Address
                            </a>
                        </div>
                        <p>Or copy and paste this link into your browser:</p>
                        <p style='word-break: break-all; color: #666;'>{{VerificationLink}}</p>
                        <p>This verification link will expire in {{ExpirationHours}} hours.</p>
                        <p>If you didn't create an account with us, please ignore this email.</p>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            // Password Reset Template
            RegisterTemplate(EmailTemplateType.PasswordReset, new EmailTemplate
            {
                Name = "Password Reset",
                Subject = "Reset Your Password - {{AppName}}",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>Password Reset Request</h2>
                        <p>Hi {{UserName}},</p>
                        <p>We received a request to reset your password for your {{AppName}} account.</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{ResetLink}}' style='background-color: #dc3545; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Reset Password
                            </a>
                        </div>
                        <p>Or copy and paste this link into your browser:</p>
                        <p style='word-break: break-all; color: #666;'>{{ResetLink}}</p>
                        <p>This reset link will expire in {{ExpirationHours}} hours.</p>
                        <p><strong>If you didn't request this password reset, please ignore this email.</strong> Your password will remain unchanged.</p>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            // Welcome Template
            RegisterTemplate(EmailTemplateType.Welcome, new EmailTemplate
            {
                Name = "Welcome",
                Subject = "Welcome to {{AppName}}!",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #333; text-align: center;'>Welcome to {{AppName}}!</h1>
                        <p>Hi {{UserName}},</p>
                        <p>Welcome to {{AppName}}! We're excited to have you on board.</p>
                        <p>Your account has been successfully created and verified. You can now start exploring all the features we have to offer.</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{LoginLink}}' style='background-color: #28a745; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Get Started
                            </a>
                        </div>
                        <p>If you have any questions or need help getting started, feel free to reach out to our support team.</p>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            // Two Factor Code Template
            RegisterTemplate(EmailTemplateType.TwoFactorCode, new EmailTemplate
            {
                Name = "Two Factor Authentication Code",
                Subject = "Your {{AppName}} Verification Code",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>Two-Factor Authentication</h2>
                        <p>Hi {{UserName}},</p>
                        <p>Your two-factor authentication code for {{AppName}} is:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <div style='background-color: #f8f9fa; border: 2px dashed #007bff; padding: 20px; font-size: 24px; font-weight: bold; letter-spacing: 5px; color: #007bff;'>
                                {{Code}}
                            </div>
                        </div>
                        <p>This code will expire in {{ExpirationMinutes}} minutes.</p>
                        <p><strong>For security reasons, do not share this code with anyone.</strong></p>
                        <p>If you didn't try to sign in, please secure your account immediately.</p>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            // Login Alert Template
            RegisterTemplate(EmailTemplateType.LoginAlert, new EmailTemplate
            {
                Name = "Login Alert",
                Subject = "New Login to Your {{AppName}} Account",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #333;'>New Login Alert</h2>
                        <p>Hi {{UserName}},</p>
                        <p>We detected a new login to your {{AppName}} account:</p>
                        <div style='background-color: #f8f9fa; padding: 15px; border-left: 4px solid #007bff; margin: 20px 0;'>
                            <p><strong>Time:</strong> {{LoginTime}}</p>
                            <p><strong>Location:</strong> {{Location}}</p>
                            <p><strong>Device:</strong> {{Device}}</p>
                            <p><strong>IP Address:</strong> {{IpAddress}}</p>
                        </div>
                        <p>If this was you, you can safely ignore this email.</p>
                        <p><strong>If this wasn't you, please secure your account immediately:</strong></p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{SecureAccountLink}}' style='background-color: #dc3545; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Secure My Account
                            </a>
                        </div>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            // Account Locked Template
            RegisterTemplate(EmailTemplateType.AccountLocked, new EmailTemplate
            {
                Name = "Account Locked",
                Subject = "Your {{AppName}} Account Has Been Locked",
                HtmlBody = @"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h2 style='color: #dc3545;'>Account Security Alert</h2>
                        <p>Hi {{UserName}},</p>
                        <p>Your {{AppName}} account has been temporarily locked due to {{Reason}}.</p>
                        <div style='background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 4px; margin: 20px 0;'>
                            <p><strong>Locked at:</strong> {{LockTime}}</p>
                            <p><strong>Reason:</strong> {{Reason}}</p>
                            <p><strong>Unlock time:</strong> {{UnlockTime}}</p>
                        </div>
                        <p>To unlock your account immediately, you can reset your password:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{{UnlockLink}}' style='background-color: #007bff; color: white; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block;'>
                                Unlock Account
                            </a>
                        </div>
                        <p>If you need further assistance, please contact our support team.</p>
                        <hr style='margin: 30px 0; border: none; border-top: 1px solid #eee;'>
                        <p style='color: #666; font-size: 12px;'>Best regards,<br>The {{AppName}} Team</p>
                    </div>"
            });

            _logger.LogInformation("Default email templates initialized successfully");
        }
    }
}
