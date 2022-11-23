﻿using ScrumboardAPI.Database;
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

        public List<Models.Task> GetTasks()
        {
            return scrumDB.GetTasks();
        }

        public Models.Task GetTaskFromID(int id)
        {
           return scrumDB.GetTaskFromID(id);
        }

        public Board GetBoard()
        {
            return scrumDB.GetBoard();
        }
    }
}