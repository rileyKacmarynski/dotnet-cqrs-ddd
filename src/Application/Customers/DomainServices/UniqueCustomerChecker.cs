using Dapper;
using SampleStore.Application.Configuration.Data;
using SampleStore.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Application.Customers.DomainServices
{
    public class UniqueCustomerChecker : IUniqueCustomerChecker
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UniqueCustomerChecker(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public bool IsUnique(string email)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sql = "SELECT TOP 1 1" +
                   "FROM [orders].[Customers] AS [Customer] " +
                   "WHERE [Customer].[Email] = @Email";

            var customerId = connection.QuerySingleOrDefault<int?>(sql, new
            {
                Email = email
            });

            return !customerId.HasValue;
        }
    }
}
