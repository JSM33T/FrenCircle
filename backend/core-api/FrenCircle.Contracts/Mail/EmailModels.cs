using System.ComponentModel.DataAnnotations;

namespace FrenCircle.Contracts.Mail
{
    public class EmailMessage
    {
        [Required]
        [EmailAddress]
        public string To { get; set; } = string.Empty;
        
        public string? ToName { get; set; }
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        public bool IsHtml { get; set; } = true;
        
        public List<EmailAttachment>? Attachments { get; set; }
        
        public Dictionary<string, string>? Headers { get; set; }
        
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;
    }

    public class BulkEmailMessage
    {
        [Required]
        public List<EmailRecipient> Recipients { get; set; } = new();
        
        [Required]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Body { get; set; } = string.Empty;
        
        public bool IsHtml { get; set; } = true;
        
        public List<EmailAttachment>? Attachments { get; set; }
        
        public Dictionary<string, string>? Headers { get; set; }
        
        public EmailPriority Priority { get; set; } = EmailPriority.Normal;
        
        public int BatchSize { get; set; } = 50;
        
        public int DelayBetweenBatchesMs { get; set; } = 1000;
    }

    public class EmailRecipient
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        public string? Name { get; set; }
        
        public Dictionary<string, string>? PersonalizationData { get; set; }
    }

    public class EmailAttachment
    {
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        [Required]
        public byte[] Content { get; set; } = Array.Empty<byte>();
        
        public string ContentType { get; set; } = "application/octet-stream";
        
        public bool IsInline { get; set; } = false;
        
        public string? ContentId { get; set; }
    }

    public class EmailTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string? TextBody { get; set; }
        public Dictionary<string, string>? DefaultVariables { get; set; }
    }

    public class EmailResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? MessageId { get; set; }
        public DateTime SentAt { get; set; }
        public string? Provider { get; set; }
    }

    public class BulkEmailResult
    {
        public bool Success { get; set; }
        public int TotalEmails { get; set; }
        public int SuccessfulEmails { get; set; }
        public int FailedEmails { get; set; }
        public List<EmailResult> Results { get; set; } = new();
        public Dictionary<string, string>? Errors { get; set; }
    }

    public enum EmailPriority
    {
        Low = 0,
        Normal = 1,
        High = 2
    }

    public enum EmailTemplateType
    {
        EmailVerification,
        PasswordReset,
        Welcome,
        TwoFactorCode,
        LoginAlert,
        AccountLocked,
        CustomNotification
    }
}
