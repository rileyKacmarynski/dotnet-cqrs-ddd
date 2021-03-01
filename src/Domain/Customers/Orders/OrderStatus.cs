namespace SampleStore.Domain.Customers.Orders
{
    public enum OrderStatus
    {
        Placed = 0,
        Processed = 1,
        PaymentProcessed = 2,
        Shipped = 3, 
        Delivered = 4,
        Canceled = 5
    }
}