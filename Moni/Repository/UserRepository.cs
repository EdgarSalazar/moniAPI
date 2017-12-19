using System.Linq;
using Moni.Enums;
using Moni.Models;
using Moni.Services;
using Moni.ViewModels;

namespace Moni.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(MoniContext context) : base(context)
        {

        }

        public AuthStatus Authorize(AuthViewModel auth, bool displayFailReason = false)
        {
            var user = GetAll().Where(x => x.Username == auth.Username).Select(x => new {x.Password, x.Salt})
                .FirstOrDefault();

            if (user == null)
                return displayFailReason ? AuthStatus.UserNotFound : AuthStatus.Failed;

            var hashPassword = new PasswordHasher().HashPassword(auth.Password, user.Salt).Hashed;

            if (user.Password != hashPassword)
                return displayFailReason ? AuthStatus.WrongPassword : AuthStatus.Failed;

            return AuthStatus.Success;
        }
    }
}
