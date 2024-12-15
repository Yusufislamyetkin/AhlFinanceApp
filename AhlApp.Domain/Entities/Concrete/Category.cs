using AhlApp.Domain.Entities.Abstract;

namespace AhlApp.Domain.Entities.Concrete
{
    public class Category : IEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }

        // Parametresiz constructor (EF Core için)
        private Category() { }

        public Category(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}

