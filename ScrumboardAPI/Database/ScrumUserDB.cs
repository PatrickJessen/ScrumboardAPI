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
            catch
            {
                Database.Close();
                return "Error 400! Failed to connect to database";
            }
        }

        public User GetUser(string username)
        {
            try
            {
                Database.Connect();
                string query = $"SELECT * FROM ScrumUser";
                Database.ExecuteQuery(query);
                Database.dataReader = Database.command.ExecuteReader();
                User user = new User();
                Database.dataReader.Read();
                user.Username = Database.dataReader["Username"].ToString();
                user.IsScrumMaster = (bool)Database.dataReader["IsScrumMaster"];
                Database.Close();
                return user;
            }
            catch
            {
                Database.Close();
                return null;
            }
        }

        public User Login(string username)
        {
            try
            {
                Database.Connect();
                string query = $"SELECT * FROM ScrumUser WHERE Username = '{username}'";
                Database.ExecuteQuery(query);
                Database.dataReader = Database.command.ExecuteReader();
                Database.dataReader.Read();
                User user = new User();
                user.Username = Database.dataReader["Username"].ToString();
                user.Password = Database.dataReader["HashPassword"].ToString();
                user.Salt = Database.dataReader["Salt"].ToString();
                user.IsScrumMaster = (bool)Database.dataReader["IsScrumMaster"];
                Database.Close();
                return user;
            }
            catch
            {
                Database.Close();
                return null;
            }
        }
    }
}
