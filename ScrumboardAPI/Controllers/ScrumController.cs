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
        public IActionResult GetTasks()
        {
            return Ok(scrum.GetTasks());
        }

        [HttpGet]
        [Route("/GetTaskFromID")]
        public IActionResult GetTaskFromID(int id)
        {
            return Ok(scrum.GetTaskFromID(id));
        }

        [HttpGet]
        [Route("/GetScrumboard")]
        public IActionResult GetBoard()
        {
            return Ok(scrum.GetBoard());
        }
    }
}
