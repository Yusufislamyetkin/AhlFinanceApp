using AhlApp.Shared.Models;
using AhlApp.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace AhlApp.Domain.ValueObjects
{
    [Owned]
    public record Money
    {
        public const string DefaultCurrency = "TRY";

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency) 
        {
            Amount = amount;
            Currency = currency.ToUpper();
        }

        public static Response<Money> Create(decimal amount, string currency = DefaultCurrency)
        {
            if (amount < 0)
                return Response<Money>.ErrorResponse(ErrorMessages.NegativeAmountNotAllowed);

            if (string.IsNullOrWhiteSpace(currency))
                return Response<Money>.ErrorResponse(ErrorMessages.CurrencyCannotBeNull);

            return Response<Money>.SuccessResponse(new Money(amount, currency));
        }

        public static Response<Money> Add(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                return Response<Money>.ErrorResponse(ErrorMessages.DifferentCurrenciesNotAllowed);

            return Response<Money>.SuccessResponse(new Money(a.Amount + b.Amount, a.Currency));
        }

        public static Response<Money> Subtract(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                return Response<Money>.ErrorResponse(ErrorMessages.DifferentCurrenciesNotAllowed);

            return Response<Money>.SuccessResponse(new Money(a.Amount - b.Amount, a.Currency));
        }

        public override string ToString() => $"{Amount} {Currency}";
    }
}
