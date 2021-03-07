using SampleStore.Domain.Customers.Events;
using SampleStore.Domain.Customers.Orders;
using SampleStore.Domain.Customers.Rules;
using SampleStore.Domain.Products;
using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.Abstractions.TypedIds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Customers
{
    public record CustomerId(Guid Value) : StronglyTypedId<Guid>(Value);

    public class Customer : Entity, IAggregateRoot
    {
        // Seems like strongly typed IDs are the recommended approach here.
        public CustomerId Id { get; private set; }

        // This should probably be a value object, but I want to focus on the event stuff. 
        private string _email;

        private string _name;

        private readonly List<Order> _orders;

        private bool _welcomeEmailWasSent;

        private Customer(string email, string name)
        {
            Id = new CustomerId(Guid.NewGuid());
            _email = email;
            _name = name;

            AddDomainEvent(new CustomerRegisteredEvent(Id));
        }

        public static Customer Register(string email, string name, IUniqueCustomerChecker uniqueCustomerChecker)
        {
            CheckRule(new CustomerEmailMustBeUniqueRule(email, uniqueCustomerChecker));
            return new Customer(email, name);
        }

        public void MarkAsWelcomed()
        {
            _welcomeEmailWasSent = true;
        }

        public Guid PlaceOrder(
            List<OrderProductData> orderProducts,
            List<ProductPriceData> productPrices,
            string currency)
        {
            CheckRule(new OrderShouldHaveAtLeastOneProductRule(orderProducts));

            var order = Order.CreateNew(orderProducts, productPrices, currency);
            _orders.Add(order);

            AddDomainEvent(new OrderPlacedEvent(order.Id, Id, order.GetTotal()));

            return order.Id;
        }
    }
}
