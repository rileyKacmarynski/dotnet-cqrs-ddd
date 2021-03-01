﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration.Validation
{
    public class InvalidCommandException : Exception
    {
        public string Details { get; }
        public InvalidCommandException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}
