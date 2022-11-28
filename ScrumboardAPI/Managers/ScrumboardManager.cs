using ScrumboardAPI.Database;
using ScrumboardAPI.Models;

namespace ScrumboardAPI.Managers
{
    public class ScrumboardManager
    {
        private ScrumboardDB scrumDB;

        public ScrumboardManager()
        {
            scrumDB = new ScrumboardDB();
        }

        public string CreateNewBoard(string title)
        {
            return scrumDB.CreateNewBoard(title);
        }

        public string CreateNewTask(Models.Task task)
        {
            return scrumDB.CreateNewTask(task);
        }

        public List<Models.Task> GetTasks(string title)
        {
            return scrumDB.GetTasks(title);
        }

        public Models.Task GetTaskFromID(int id)
        {
           return scrumDB.GetTaskFromID(id);
        }

        public void UpdateTask(Models.Task task)
        {
            scrumDB.UpdateTask(task);
        }

        public void DeleteTask(int id)
        {
            scrumDB.DeleteTask(id);
        }

        public Board GetBoard(string boardTitle)
        {
            return scrumDB.GetBoard(boardTitle);
        }

        public List<string> GetSprintNames()
        {
            return scrumDB.GetSprintNames();
        }
    }
}
