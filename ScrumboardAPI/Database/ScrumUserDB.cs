using ScrumboardAPI.Models;
using System.Data.SqlClient;

namespace ScrumboardAPI.Database
{
    public class ScrumUserDB
    {
        //private const string connectionString = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=ScrumDB;Trusted_Connection=True;"; // school
        private const string connectionString = "Server=DESKTOP-R394HDQ;Database=ScrumDB;Trusted_Connection=True;"; // home
        public string CreateUser(User user)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"INSERT INTO ScrumUser (Username, Salt, HashPassword, IsScrumMaster) VALUES (@username, @salt, @hashPassword, @isScrumMaster)";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@salt", user.Salt);
                command.Parameters.AddWithValue("@hashPassword", user.Password);
                command.Parameters.AddWithValue("@isScrumMaster", user.IsScrumMaster);
                command.ExecuteScalar();
                connect.Close();
                return "Successfully created user";
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
        }

        public User GetUser(string username)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM ScrumUser";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader dataReader = command.ExecuteReader();
                User user = new User();
                dataReader.Read();
                user.Username = dataReader["Username"].ToString();
                user.IsScrumMaster = (bool)dataReader["IsScrumMaster"];
                connect.Close();
                return user;
            }
            catch
            {
                return null;
            }
        }

        public User Login(string username)
        {
            User user = new User();
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM ScrumUser WHERE Username = '{username}'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {

                    user.Username = dataReader["Username"].ToString();
                    user.Password = dataReader["HashPassword"].ToString();
                    user.Salt = dataReader["Salt"].ToString();
                    user.IsScrumMaster = (bool)dataReader["IsScrumMaster"];
                }
                connect.Close();
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
