using Microsoft.AspNetCore.Mvc;
using ScrumboardAPI.Managers;

namespace ScrumboardAPI.Controllers
{
    public class ScrumController : Controller
    {
        ScrumboardManager scrum = new ScrumboardManager();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/PostScrumboard")]
        public IActionResult PostBoard(string title)
        {
            return Ok(scrum.CreateNewBoard(title));
        }

        [HttpPost]
        [Route("/PostTask")]
        public IActionResult PostNewTask(Models.Task task)
        {
            return Ok(scrum.CreateNewTask(task));
        }

        [HttpGet]
        [Route("/GetTasks")]
        public IActionResult GetTasks(string title)
        {
            return Ok(scrum.GetTasks(title));
        }

        [HttpGet]
        [Route("/GetTaskFromID")]
        public IActionResult GetTaskFromID(int id)
        {
            return Ok(scrum.GetTaskFromID(id));
        }

        [HttpGet]
        [Route("/GetScrumboard")]
        public IActionResult GetBoard(string boardTitle)
        {
            return Ok(scrum.GetBoard(boardTitle));
        }

        [HttpPut]
        [Route("/UpdateTask")]
        public IActionResult UpdateTask(Models.Task task)
        {
            scrum.UpdateTask(task);
            return Ok();
        }

        [HttpDelete]
        [Route("/DeleteTask")]
        public IActionResult DeleteTask(int id)
        {
            scrum.DeleteTask(id);
            return Ok();
        }

        [HttpGet]
        [Route("/GetSprintNames")]
        public IActionResult GetSprintNames()
        {
            return Ok(scrum.GetSprintNames());
        }

        [HttpDelete]
        [Route("/DeleteSprint")]
        public IActionResult DeleteBoard(string title)
        {
            scrum.DeleteBoard(title);
            return Ok();
        }
    }
}
