using SampleStore.Domain.SharedKernel.ValueObjects;

namespace SampleStore.Domain.Products
{
    public class ProductPrice
    {
        public MoneyValue Value { get; private set; }

        private ProductPrice()
        {

        }
    }
}