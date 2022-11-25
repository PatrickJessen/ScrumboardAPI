using System.Data.SqlClient;

namespace ScrumboardAPI.Database
{
    public class Database
    {
        static public SqlConnection connect { get; private set; }
        static public SqlCommand command { get; set; }
        static public SqlDataReader dataReader { get; set; }
        static public SqlDataAdapter adapter { get; set; }

        private const string connectionString = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=ScrumDB;Trusted_Connection=True;"; // school
        //private const string connectionString = "Server=DESKTOP-R394HDQ;Database=ScrumDB;Trusted_Connection=True;"; // home

        public static void Connect()
        {
            connect = new SqlConnection(connectionString);
            connect.Open();
        }

        public static void Close()
        {
            connect.Close();
        }

        public static void ExecuteQuery(string query)
        {
            adapter = new SqlDataAdapter(query, connect);
            command = new SqlCommand(query, connect);
        }
    }
}
