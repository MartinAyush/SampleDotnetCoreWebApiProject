using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents(int id)
        {
            string[] students = { "Martin", "Abhinay", "Mayank", "Gaurav", "Mahendra" };
            return Ok(students[id]);
        }

        [HttpPost]
        public IActionResult CreateStudent()
        {
            return Ok("Student got created");
        }
    }
}
