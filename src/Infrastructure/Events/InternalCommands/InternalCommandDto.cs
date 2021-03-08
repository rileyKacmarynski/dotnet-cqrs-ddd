using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleStore.Infrastructure.Events.InternalCommands
{
    public class InternalCommandDto
    {
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
