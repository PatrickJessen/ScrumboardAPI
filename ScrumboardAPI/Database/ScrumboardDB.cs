using Microsoft.Extensions.Primitives;
using ScrumboardAPI.Models;
using System.Threading.Tasks;

namespace ScrumboardAPI.Database
{
    public class ScrumboardDB
    {
        public string CreateNewBoard(string title)
        {
            try
            {
                Database.Connect();
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
            string query = $"INSERT INTO Board (Title) VALUES (@title)";
            Database.ExecuteQuery(query);
            Database.command.Parameters.AddWithValue("@title", title);
            Database.command.ExecuteScalar();
            Database.Close();
            return "Successfully created board";
        }

        public string CreateNewTask(Models.Task task)
        {
            int boardId = GetBoardId(TaskStateToString(task.State));
            try
            {
                Database.Connect();
                string query = $"INSERT INTO Task (Title, Description, Points, AssignedTo, State, Priority, TaskId) VALUES" +
                    $" (@title, @description, @points, @assignedTo, @state, @priority, @taskId)";
                Database.ExecuteQuery(query);
                Database.command.Parameters.AddWithValue("@title", task.Title);
                Database.command.Parameters.AddWithValue("@description", task.Description);
                Database.command.Parameters.AddWithValue("@points", task.Points);
                Database.command.Parameters.AddWithValue("@assignedTo", task.AssignedTo);
                Database.command.Parameters.AddWithValue("@state", task.State);
                Database.command.Parameters.AddWithValue("@priority", task.Priority);
                Database.command.Parameters.AddWithValue("@taskId", boardId);
                Database.command.ExecuteScalar();
                Database.Close();
                return "Successfully created board";
            }
            catch
            {
                Database.Close();
                return "Error 400! Failed to connect to database";
            }
        }

        public void UpdateTask(Models.Task task)
        {
            try
            {
                Database.Connect();
                string query = $"UPDATE Task SET Title = @title, Description = @description, Points = @points, AssignedTo = @assignedTo, State = @state, Priority = @priority " +
                    $"WHERE Id = {task.Id}";
                Database.ExecuteQuery(query);
                Database.command.Parameters.AddWithValue("@title", task.Title);
                Database.command.Parameters.AddWithValue("@description", task.Description);
                Database.command.Parameters.AddWithValue("@points", task.Points);
                Database.command.Parameters.AddWithValue("@assignedTo", task.AssignedTo);
                Database.command.Parameters.AddWithValue("@state", task.State);
                Database.command.Parameters.AddWithValue("@priority", task.Priority);
                Database.command.ExecuteScalar();
                Database.Close();
            }
            catch
            {
                Database.Close();
            }
        }

        public void DeleteTask(int id)
        {
            try
            {
                Database.Connect();
                string query = $"DELETE Task WHERE Id = {id}";
                Database.ExecuteQuery(query);
                Database.command.ExecuteScalar();
                Database.Close();
            }
            catch
            {
                Database.Close();
            }
        }

        private int GetBoardIdFromTitle(string title)
        {
            try
            {
                Database.Connect();
                string query = $"SELECT id FROM Board WHERE Title = {title}";
                Database.ExecuteQuery(query);
                int id = 0;
                while (Database.dataReader.Read())
                {
                    id = (int)Database.dataReader["id"];
                }
                Database.Close();
                return id;
            }
            catch
            {
                Database.Close();
                return -1;
            }
        }

        public List<Models.Task> GetTasks(string boardTitle)
        {
            int boardId = GetBoardIdFromTitle(boardTitle);
            List<Models.Task> tasks = new List<Models.Task>();
            try
            {
                Database.Connect();
            }
            catch
            {
                return null;
            }
            string query = $"SELECT * FROM Task WHERE BoardId = {boardId}";
            Database.ExecuteQuery(query);
            Database.command.ExecuteScalar();
            Database.dataReader = Database.command.ExecuteReader();
            while (Database.dataReader.Read())
            {
                string title = Database.dataReader["Title"].ToString();
                string description = Database.dataReader["Description"].ToString();
                int points = (int)Database.dataReader["Points"];
                string assignedTo = Database.dataReader["AssignedTo"].ToString();
                TaskState state = (TaskState)Database.dataReader["State"];
                TaskPriority priority = (TaskPriority)Database.dataReader["Priority"];
                int id = (int)Database.dataReader["Id"];
                Models.Task t = new Models.Task(title, state, description, points, assignedTo, priority);
                t.Id = id;
                tasks.Add(t);
            }
            Database.Close();
            return tasks;
        }

        public Models.Task GetTaskFromID(int id)
        {
            try
            {
                Database.Connect();
                string query = $"SELECT * FROM Task WHERE TaskId = @id";
                Database.ExecuteQuery(query);
                Database.command.Parameters.AddWithValue("@id", id);
                Models.Task task = (Models.Task)Database.command.ExecuteScalar();
                Database.Close();
                return task;
            }
            catch
            {
                Database.Close();
                return null;
            }
        }

        public Board GetBoard(string boardTitle)
        {
            Board board = null;
            try
            {
                Database.Connect();
                string query = $"SELECT * FROM Board";
                Database.ExecuteQuery(query);
                //Database.command.ExecuteScalar();
                Database.dataReader = Database.command.ExecuteReader();
                while (Database.dataReader.Read())
                {
                    board = new Board(Database.dataReader["Title"].ToString());
                }
                Database.Close();
                board.Tasks = GetTasks(boardTitle);
                return board;
            }
            catch
            {
                Database.Close();
                return null;
            }
        }

        public List<string> GetSprintNames()
        {
            List<string> list = new List<string>();
            try
            {
                Database.Connect();
                string query = $"SELECT * FROM Board";
                Database.ExecuteQuery(query);
                //Database.command.ExecuteScalar();
                Database.dataReader = Database.command.ExecuteReader();
                while (Database.dataReader.Read())
                {
                    list.Add(Database.dataReader["Title"].ToString());
                }
                Database.Close();
                return list;
            }
            catch
            {
                Database.Close();
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
                Database.Connect();
                string query = $"SELECT Id FROM Board WHERE Title = @title";
                Database.ExecuteQuery(query);
                Database.command.Parameters.AddWithValue("@title", title);
                Database.command.ExecuteScalar();
                int id = Convert.ToInt32(Database.command.ExecuteScalar());
                Database.Close();
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
