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

        public User Login(string username, string password)
        {
            User user = userDB.Login(username);
            if (user == null)
            {
                return null;
            }
            if (VeryfiUser(user, password))
            {
                return user;
            }
            return null;
        }

        private bool VeryfiUser(User user, string password)
        {
            if (CryptoService.VerifyPassword(password, user.Password, user.Salt))
            {
                return true;
            }
            return false;
        }
    }
}
