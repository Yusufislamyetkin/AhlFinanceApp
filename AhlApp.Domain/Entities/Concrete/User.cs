using AhlApp.Domain.Constants;
using AhlApp.Domain.Entities.Abstract;
using AhlApp.Domain.ValueObjects;
using AhlApp.Shared.Models;

namespace AhlApp.Domain.Entities.Concrete
{
    public class User : IEntity, IAggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }

        private readonly List<Account> _accounts = new();
        public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();

        private User() { } 

        public User(string name, string email, string passwordHash, string passwordSalt)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;

            AddAccount(new Money(0, "TRY"), "Varsayılan");
        }

        public Response<bool> AddAccount(Money initialBalance, string accountName)
        {
            if (_accounts.Any(a => a.AccountName == accountName))
                return Response<bool>.ErrorResponse(ErrorMessages.AccountNumberAlreadyExists);

            var account = new Account(Id, initialBalance, accountName);
            _accounts.Add(account);

            return Response<bool>.SuccessResponse(true);
        }
    }
}
