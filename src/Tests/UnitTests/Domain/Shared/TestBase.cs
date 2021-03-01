using FluentAssertions;
using SampleStore.Domain.SharedKernel;
using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Domain.Shared
{
    public abstract class TestBase : IDisposable
    {
        public static T AssertPublishedDomainEvent<T>(Entity aggregate) where T : IDomainEvent
        {
            var domainEvent = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().SingleOrDefault();

            if(domainEvent is null)
            {
                throw new Exception($"{typeof(T).Name} not published");
            }

            return domainEvent;
        }

        public static List<T> AssertPublishedDomainEvents<T>(Entity aggregate) where T : IDomainEvent
        {
            var domainEvents = DomainEventsTestHelper.GetAllDomainEvents(aggregate).OfType<T>().ToList();

            if (!domainEvents.Any())
            {
                throw new Exception($"{typeof(T).Name} not published");
            }

            return domainEvents;
        }

        // This won't work if I need to return something from the delegate.
        public void AssertBrokenRule<TRule>(Action action) where TRule : class, IBusinessRule
        {
            var message = $"Expected {typeof(TRule).Name} broken rule";
            var ex = Assert.Throws<BusinessRuleValidationException>(action);
            ex.BrokenRule.Should().BeOfType<TRule>();
        }

        public void Dispose()
        {
            SystemClock.Reset();
        }
    }
}
