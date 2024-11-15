using FrenCircle.Entities.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Services
{
    public interface IMessageService
    {
        public Task SendMail(SendMailRequest mailRequest);
        public Task SendTelegramMessage(SendMailRequest mailRequest);
    }
}
