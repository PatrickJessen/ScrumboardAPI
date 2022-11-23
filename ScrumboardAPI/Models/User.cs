namespace ScrumboardAPI.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsScrumMaster { get; set; }

        public User(string username, string password, string salt, bool isScrumMaster)
        {
            Username = username;
            Password = password;
            Salt = salt;
            IsScrumMaster = isScrumMaster;
        }

        public User()
        {

        }
    }
}
