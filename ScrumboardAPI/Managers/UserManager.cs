using ScrumboardAPI.Database;
using ScrumboardAPI.Models;

namespace ScrumboardAPI.Managers
{
    public class UserManager
    {
        ScrumUserDB userDB = new ScrumUserDB();

        public string CreateUser(User user)
        {
            HashSalt hs = CryptoService.SaltPassword(user.Password);
            user.Password = hs.Hash;
            user.Salt = hs.Salt;
            return userDB.CreateUser(user);
        }

        public User GetUser(string username)
        {
            return userDB.GetUser(username);
        }
    }
}
