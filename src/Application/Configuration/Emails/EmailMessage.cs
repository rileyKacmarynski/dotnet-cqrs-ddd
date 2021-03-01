using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration.Emails
{
    public struct EmailMessage
    {
        public string From { get; }
        public string To { get; }
        public string Content { get; }

        public EmailMessage(string from, string to, string content)
        {
            From = from;
            To = to;
            Content = content;
        }
    }
}
