using Microsoft.AspNetCore.Mvc;
using ScrumboardAPI.Managers;
using ScrumboardAPI.Models;

namespace ScrumboardAPI.Controllers
{
    public class UserController : Controller
    {
        UserManager um = new UserManager();
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/PostUser")]
        public IActionResult PostUser(User user)
        {
            return Ok(um.CreateUser(user));
        }

        [HttpPost]
        [Route("/GetUser")]
        public IActionResult GetUser(string username)
        {
            return Ok(um.GetUser(username));
        }
    }
}
