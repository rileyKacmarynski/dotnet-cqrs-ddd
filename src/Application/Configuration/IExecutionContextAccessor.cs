﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }
        bool IsAvailable { get; }
    }
}
