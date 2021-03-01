using SampleStore.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Payments
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(Guid paymentId);
        Task AddAsync(Payment payment);
    }
}
