using FluentAssertions;
using Moq;
using SampleStore.Domain.Customers;
using SampleStore.Domain.Customers.Events;
using SampleStore.Domain.Customers.Rules;
using SampleStore.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Domain.Shared;
using Xunit;

namespace UnitTests.Domain.Customers
{
    public class CustomerRegistrationTests : TestBase
    {
        [Fact]
        public void RegisterCustomer_EmailIsUnique_IsSuccessful()
        {
            // Arrange
            var uniqueCustomerMock = new Mock<IUniqueCustomerChecker>();
            uniqueCustomerMock.Setup(mock => mock.IsUnique(It.IsAny<string>())).Returns(true);

            // Act
            var customer = Customer.Register("bob@email.com", "Bob", uniqueCustomerMock.Object);

            // Assert
            AssertPublishedDomainEvent<CustomerRegisteredEvent>(customer);
        }

        [Fact]
        public void RegisterCustomer_EmailIsUnique_BreaksCustomerEmailMustBeUniqueRule()
        {
            // Arrange
            var uniqueCustomerMock = new Mock<IUniqueCustomerChecker>();
            uniqueCustomerMock.Setup(mock => mock.IsUnique(It.IsAny<string>())).Returns(false);

            // Act
            Action registerAction = () => Customer.Register("bob@email.com", "Bob", uniqueCustomerMock.Object);

            // Assert

            //var ex = Assert.Throws<BusinessRuleValidationException>(registerAction);
            //ex.BrokenRule.Should().BeOfType<CustomerEmailMustBeUniqueRule>();
            AssertBrokenRule<CustomerEmailMustBeUniqueRule>(registerAction);
        }
    }
}
