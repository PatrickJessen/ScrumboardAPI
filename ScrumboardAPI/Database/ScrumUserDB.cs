using ScrumboardAPI.Models;

namespace ScrumboardAPI.Database
{
    public class ScrumUserDB
    {
        public string CreateUser(User user)
        {
            try
            {
                Database.Connect();
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
            string query = $"INSERT INTO ScrumUser (Username, Salt, HashPassword, IsScrumMaster) VALUES (@username, @salt, @hashPassword, @isScrumMaster)";
            Database.ExecuteQuery(query);
            Database.command.Parameters.AddWithValue("@username", user.Username);
            Database.command.Parameters.AddWithValue("@salt", user.Salt);
            Database.command.Parameters.AddWithValue("@hashPassword", user.Password);
            Database.command.Parameters.AddWithValue("@isScrumMaster", user.IsScrumMaster);
            Database.command.ExecuteScalar();
            Database.Close();
            return "Successfully created user";
        }

        public User GetUser(string username)
        {
            try
            {
                Database.Connect();
            }
            catch
            {
                return null;
            }
            string query = $"SELECT * FROM ScrumUser";
            Database.ExecuteQuery(query);
            User user = (User)Database.command.ExecuteScalar();
            Database.Close();
            return user;
        }
    }
}
