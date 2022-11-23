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
            }
            catch
            {
                return "Error 400! Failed to connect to database";
            }
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

        public List<Models.Task> GetTasks()
        {
            List<Models.Task> tasks = new List<Models.Task>();
            try
            {
                Database.Connect();
            }
            catch
            {
                return null;
            }
            string query = $"SELECT * FROM Task";
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
                int id = (int)Database.dataReader["TaskId"];
                tasks.Add(new Models.Task(title, state, description, points, assignedTo, priority));
            }
            Database.Close();
            return tasks;
        }

        public Models.Task GetTaskFromID(int id)
        {
            try
            {
                Database.Connect();
            }
            catch
            {
                return null;
            }
            string query = $"SELECT * FROM Task WHERE TaskId = @id";
            Database.ExecuteQuery(query);
            Database.command.Parameters.AddWithValue("@id", id);
            Models.Task task = (Models.Task)Database.command.ExecuteScalar();
            Database.Close();
            return task;
        }

        public Board GetBoard()
        {
            Board board = new Board();
            try
            {
                Database.Connect();
            }
            catch
            {
                return null;
            }
            string query = $"SELECT * FROM Board";
            Database.ExecuteQuery(query);
            Database.command.ExecuteScalar();
            Database.dataReader = Database.command.ExecuteReader();
            while (Database.dataReader.Read())
            {
                board.States.Add(new BoardState(Database.dataReader["Title"].ToString()));
            }
            Database.Close();
            AddTasksToBoard(board.States, GetTasks());
            return board;
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
            }
            catch
            {
                return 0;
            }
            string query = $"SELECT Id FROM Board WHERE Title = @title";
            Database.ExecuteQuery(query);
            Database.command.Parameters.AddWithValue("@title", title);
            Database.command.ExecuteScalar();
            int id = Convert.ToInt32(Database.command.ExecuteScalar());
            Database.Close();
            return id;
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
