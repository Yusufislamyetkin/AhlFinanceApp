namespace AhlApp.Shared.Security
{
    public interface ISecurityHasher
    {
        (string Hash, string Salt) Hash(string input);
        bool Validate(string input, string hash, string salt);
    }
}
