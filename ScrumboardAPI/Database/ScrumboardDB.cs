using Microsoft.Extensions.Primitives;
using ScrumboardAPI.Models;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace ScrumboardAPI.Database
{
    public class ScrumboardDB
    {
        private const string connectionString = "Server=PJJ-P15S-2022\\SQLEXPRESS;Database=ScrumDB;Trusted_Connection=True;"; // school
        //private const string connectionString = "Server=DESKTOP-R394HDQ;Database=ScrumDB;Trusted_Connection=True;"; // home
        public string CreateNewBoard(string title)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"INSERT INTO Board (Title) VALUES (@title)";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteScalar();
                connect.Close();
                return "Successfully created board";
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
        }

        public void DeleteBoard(string title)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"DELETE FROM Board WHERE Title = @title";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteScalar();
                connect.Close();
            }
            catch
            {

            }
        }

        public string CreateNewTask(Models.Task task)
        {
            int boardId = GetBoardIdFromTitle(task.Sprint);
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"INSERT INTO Task (Title, Description, Points, AssignedTo, State, Priority, BoardId) VALUES" +
                    $" (@title, @description, @points, @assignedTo, @state, @priority, (SELECT id FROM Board WHERE id = @boardId))";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@title", task.Title);
                command.Parameters.AddWithValue("@description", task.Description);
                command.Parameters.AddWithValue("@points", task.Points);
                command.Parameters.AddWithValue("@assignedTo", task.AssignedTo);
                command.Parameters.AddWithValue("@state", task.State);
                command.Parameters.AddWithValue("@priority", task.Priority);
                command.Parameters.AddWithValue("@boardId", boardId);
                command.ExecuteScalar();
                connect.Close();
                return "Successfully created board";
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
        }

        public void UpdateTask(Models.Task task)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"UPDATE Task SET Title = @title, Description = @description, Points = @points, AssignedTo = @assignedTo, State = @state, Priority = @priority " +
                    $"WHERE Id = {task.Id}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@title", task.Title);
                command.Parameters.AddWithValue("@description", task.Description);
                command.Parameters.AddWithValue("@points", task.Points);
                command.Parameters.AddWithValue("@assignedTo", task.AssignedTo);
                command.Parameters.AddWithValue("@state", task.State);
                command.Parameters.AddWithValue("@priority", task.Priority);
                command.ExecuteScalar();
                connect.Close();
            }
            catch
            {

            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"DELETE Task WHERE Id = {id}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.ExecuteScalar();
                connect.Close();
            }
            catch
            {

            }
        }

        private int GetBoardIdFromTitle(string title)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT id FROM Board WHERE Title = '{title}'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader dataReader = command.ExecuteReader();
                int id = 0;
                while (dataReader.Read())
                {
                    id = (int)dataReader["id"];
                }
                return id;
            }
            catch
            {
                return -1;
            }
        }

        public List<Models.Task> GetTasks(string boardTitle)
        {
            int boardId = GetBoardIdFromTitle(boardTitle);
            List<Models.Task> tasks = new List<Models.Task>();
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM Task WHERE BoardId = {boardId}";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.ExecuteScalar();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string title = reader["Title"].ToString();
                    string description = reader["Description"].ToString();
                    int points = (int)reader["Points"];
                    string assignedTo = reader["AssignedTo"].ToString();
                    TaskState state = (TaskState)reader["State"];
                    TaskPriority priority = (TaskPriority)reader["Priority"];
                    int id = (int)reader["Id"];
                    Models.Task t = new Models.Task(title, state, description, points, assignedTo, boardTitle, priority);
                    t.Id = id;
                    tasks.Add(t);
                }
                connect.Close();
                return tasks;
            }
            catch
            {
                return null;
            }
        }

        public Models.Task GetTaskFromID(int id)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM Task WHERE TaskId = @id";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@id", id);
                Models.Task task = (Models.Task)command.ExecuteScalar();
                connect.Close();
                return task;
            }
            catch
            {
                return null;
            }
        }

        public Board GetBoard(string boardTitle)
        {
            Board board = null;
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM Board";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    board = new Board((int)dataReader["id"], dataReader["Title"].ToString());
                }
                connect.Close();
                board.Tasks = GetTasks(boardTitle);
                return board;
            }
            catch
            {
                Database.Close();
                return null;
            }
        }

        struct Sprint
        {
            public int Id;
            public string Title;
        }

        public List<string> GetSprintNames()
        {
            List<string> list = new List<string>();
            List<Sprint> sprints = new List<Sprint>();
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT * FROM Board";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Sprint s = new Sprint();
                    s.Id = (int)dataReader["id"];
                    s.Title = (string)dataReader["Title"];
                    sprints.Add(s);
                }
                connect.Close();
                for (int i = 0; i < sprints.Count; i++)
                {
                    for (int j = 0; j < sprints.Count; j++)
                    {
                        if (sprints[i].Id < sprints[j].Id)
                        {
                            Sprint s = sprints[i];
                            sprints[i] = sprints[j];
                            sprints[j] = s;
                        }
                    }
                }
                for (int i = 0; i < sprints.Count; i++)
                {
                    list.Add(sprints[i].Title);
                }
                return list;
            }
            catch
            {
                return new List<string>();
            }
        }

        private void AddTasksToBoard(in List<BoardState> states, List<Models.Task> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < states.Count; j++)
                {
                    if (states[j].Title == TaskStateToString(tasks[i].State))
                    {
                        states[j].Tasks.Add(tasks[i]);
                    }
                }
            }
        }

        private int GetBoardId(string title)
        {
            try
            {
                SqlConnection connect = new SqlConnection(connectionString);
                connect.Open();
                string query = $"SELECT Id FROM Board WHERE Title = @title";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteScalar();
                int id = Convert.ToInt32(command.ExecuteScalar());
                connect.Close();
                return id;
            }
            catch
            {
                Database.Close();
                return 0;
            }
        }

        private string TaskStateToString(TaskState state)
        {
            switch (state)
            {
                case TaskState.TO_DO:
                    return "TODO";
                    break;
                case TaskState.IN_PROGRESS:
                    return "IN PROGRESS";
                    break;
                case TaskState.REVIEW:
                    return "REVIEW";
                    break;
                case TaskState.DONE:
                    return "DONE";
                    break;
                default:
                    return null;
                    break;
            }
        }
    }
}
