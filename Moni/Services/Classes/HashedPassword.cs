namespace Moni.Services.Classes
{
    public class PasswordHash
    {
        public PasswordHash(string hashed, string salt)
        {
            Hashed = hashed;
            Salt = salt;
        }

        public string Hashed { get; }
        public string Salt { get; }
    }
}
