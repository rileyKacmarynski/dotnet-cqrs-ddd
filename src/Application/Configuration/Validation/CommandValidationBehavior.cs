using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Application.Configuration.Validation
{
    public class CommandValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public CommandValidationBehavior(IEnumerable<IValidator<TRequest>> _validators)
        {
            this._validators = _validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (!errors.Any())
            {
                return next();
            }

            var errorString = errors.Aggregate(new StringBuilder().AppendLine("Invalid command: "), 
                (errorBuilder, error) => errorBuilder.AppendLine(error.ErrorMessage))
                .ToString();

            throw new InvalidCommandException(errorString, null);
        }
    }
}
