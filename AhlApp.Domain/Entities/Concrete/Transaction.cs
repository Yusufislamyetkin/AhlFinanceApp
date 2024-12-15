using AhlApp.Domain.Entities.Abstract;
using AhlApp.Domain.ValueObjects;
using AhlApp.Domain.Constants;

namespace AhlApp.Domain.Entities.Concrete
{
    public class Transaction : IEntity
    {
        public Guid Id { get; private set; }
        public Guid? AccountId { get; private set; }
        public Account? Account { get; private set; }
        public Guid? SenderAccountId { get; private set; }
        public Account? SenderAccount { get; private set; }

        public Guid? ReceiverAccountId { get; private set; }
        public Account? ReceiverAccount { get; private set; }

        public Money Amount { get; private set; }
        public string Description { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } 

        public TransactionType TransactionType { get; private set; }
        public DateTime TransactionDate { get; private set; }

        private Transaction() { } 

        public Transaction(
            TransactionType transactionType,
            Money amount,
            string description,
            Guid categoryId,
            Guid? accountId = null,
            Guid? senderAccountId = null,
            Guid? receiverAccountId = null)
        {
            Id = Guid.NewGuid();
            Amount = amount;
            Description = description;
            CategoryId = categoryId;
            TransactionType = transactionType;
            TransactionDate = DateTime.UtcNow;

            switch (transactionType)
            {
                case TransactionType.Expense:
                case TransactionType.Deposit:
                    if (accountId == null)
                        throw new ArgumentException(ErrorMessages.AccountIdRequiredForExpenseOrDeposit, nameof(accountId));
                    AccountId = accountId;
                    break;

                case TransactionType.Transfer:
                    if (senderAccountId == null || receiverAccountId == null)
                        throw new ArgumentException(ErrorMessages.AccountIdsRequiredForTransfer);
                    SenderAccountId = senderAccountId;
                    ReceiverAccountId = receiverAccountId;
                    break;

                default:
                    throw new ArgumentException(ErrorMessages.InvalidTransactionType, nameof(transactionType));
            }
        }
    }

    public enum TransactionType
    {
        Expense = 1, // Gider
        Transfer = 2, // Transfer
        Deposit = 3   // Para Yükleme
    }
}
