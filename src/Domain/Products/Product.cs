using SampleStore.Domain.SharedKernel.Abstractions;
using SampleStore.Domain.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.Products
{
    public class Product : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public MoneyValue Price { get; set; }

        private Product() { }
    }
}
