using SampleStore.Domain.SharedKernel.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleStore.Domain.SharedKernel.ValueObjects
{

    public class MoneyValue : ValueObject
    {

        public decimal Value { get; }
        public string Currency { get; }

        private MoneyValue(decimal value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        public MoneyValue(MoneyValue moneyValue)
        {
            Value = moneyValue.Value;
            Currency = moneyValue.Currency;
        }

        public static MoneyValue From(decimal value, string currency)
        {
            CheckRule(new MoneyValueMustHaveCurrencyRule(currency));

            return new MoneyValue(value, currency);
        }

        public static MoneyValue operator +(MoneyValue moneyValueLeft, MoneyValue moneyValueRight)
        {
            CheckRule(new MoneyValueOperationMustBePerformedOnTheSameCurrencyRule(moneyValueLeft, moneyValueRight));

            if (moneyValueLeft.Currency != moneyValueRight.Currency)
            {
                throw new ArgumentException();
            }

            return new MoneyValue(moneyValueLeft.Value + moneyValueRight.Value, moneyValueLeft.Currency);
        }

        public static MoneyValue operator *(int number, MoneyValue moneyValueRight)
        {
            return new MoneyValue(number * moneyValueRight.Value, moneyValueRight.Currency);
        }

        public static MoneyValue operator *(decimal number, MoneyValue moneyValueRight)
        {
            return new MoneyValue(number * moneyValueRight.Value, moneyValueRight.Currency);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }

    public static class MoneyValueExtensions
    {
        public static MoneyValue Sum<T>(this IEnumerable<T> source, Func<T, MoneyValue> selector) =>
            new MoneyValue(source.Select(selector).Aggregate((sum, value) => sum + value));

        public static MoneyValue Sum(this IEnumerable<MoneyValue> source) => source.Aggregate((sum, value) => sum + value);
    }
}
