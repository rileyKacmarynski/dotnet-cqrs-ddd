using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration.Queries
{
    // Could just use IRequest<TResult> and IRequestHandler<TRequest, TResult> instead
    // of creating these interfaces, but this makes it a little more apparent what
    // the code intends to do. 
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }

    public interface IQueryHandler<in TQuery, TResult> : 
        IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

    }
}
