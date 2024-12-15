using AhlApp.Shared.Models;
using AhlApp.Domain.ValueObjects;
using AhlApp.Domain.Constants;
using AhlApp.Domain.Entities.Abstract;

namespace AhlApp.Domain.Entities.Concrete
{
    public class Account : IEntity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public Money Balance { get; private set; }
        public string AccountName { get; private set; }

        private readonly List<Transaction> _transactions = new();
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        private Account() { }

        public Account(Guid userId, Money initialBalance, string accountName)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Balance = initialBalance;
            AccountName = accountName;
        }

        // Bakiyeyi güncelleme metodu
        public void UpdateBalance(Money newBalance)
        {
            Balance = newBalance;
        }




    }
}
