using SampleStore.Domain.SharedKernel.Abstractions;

namespace SampleStore.Domain.SharedKernel.ValueObjects
{
    internal class MoneyValueOperationMustBePerformedOnTheSameCurrencyRule : IBusinessRule
    {
        private MoneyValue _moneyValueLeft;
        private MoneyValue _moneyValueRight;

        public MoneyValueOperationMustBePerformedOnTheSameCurrencyRule(MoneyValue moneyValueLeft, MoneyValue moneyValueRight)
        {
            _moneyValueLeft = moneyValueLeft;
            _moneyValueRight = moneyValueRight;
        }

        public string Message => "Money value currencies must be the same";

        public bool IsBroken() => _moneyValueLeft != _moneyValueRight;
    }
}