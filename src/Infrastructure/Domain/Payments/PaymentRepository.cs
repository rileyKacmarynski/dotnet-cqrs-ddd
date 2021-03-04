using Microsoft.EntityFrameworkCore;
using SampleStore.Application.Payments;
using SampleStore.Domain.Payments;
using SampleStore.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Infrastructure.Domain.Payments
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly OrdersContext _context;

        public PaymentRepository(OrdersContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }

        public async Task<Payment> GetByIdAsync(Guid paymentId)
        {
            return await _context.Payments
                .SingleAsync(p => p.Id == paymentId);
        }
    }
}
