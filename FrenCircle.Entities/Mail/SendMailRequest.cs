using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Entities.Mail
{
    public class SendMailRequest
    {
        public string From { get; set; }
        public List<string> To { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}
