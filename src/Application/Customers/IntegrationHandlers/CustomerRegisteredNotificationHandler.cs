using Dapper;
using MediatR;
using SampleStore.Application.Configuration.Data;
using SampleStore.Application.Configuration.Emails;
using SampleStore.Application.Configuration.Processing;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleStore.Application.Customers.IntegrationHandlers
{
    public class CustomerRegisteredNotificationHandler : INotificationHandler<CustomerRegisteredNotification>
    {
        private readonly ICommandScheduler _commandsScheduler;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CustomerRegisteredNotificationHandler(ICommandScheduler commandsScheduler, IEmailSender emailSender, EmailSettings emailSettings, ISqlConnectionFactory sqlConnectionFactory)
        {
            _commandsScheduler = commandsScheduler;
            _emailSender = emailSender;
            _emailSettings = emailSettings;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task Handle(CustomerRegisteredNotification notification, CancellationToken cancellationToken)
        {
            // Send Email
            var connection = _sqlConnectionFactory.GetOpenConnection();
            const string sql = "SELECT [Customer].[Email] " +
                   "FROM orders.v_Customers AS [Customer] " +
                   "WHERE [Customer].[Id] = @Id";

            var customerEmail = await connection.QueryFirstAsync<string>(sql, new
            {
                Id = notification.CustomerId.Value
            });

            var emailMessage = new EmailMessage(
                _emailSettings.FromEmailAddress,
                customerEmail,
                $"Welcome to SampleStore.");

            await _emailSender.SendEmailAsync(emailMessage);

            // We need to mark the customer as welcomed. Could probably
            // just flip the flag here, let's say we have more logic that needs to 
            // be done after sending the welcome email. For that situation an internal
            // command will be created and saved to the internal command database. 
            // The domain notification table is kind of like an outbox, and the internal
            // command table is kind of like an inbox.

            await _commandsScheduler.EnqueueAsync(new MarkCustomerAsWelcomedCommand(Guid.NewGuid(), notification.CustomerId));
        }
    }
}
