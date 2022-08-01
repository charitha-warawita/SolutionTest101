using MicroService101.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroService101.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStudentService _studentService;

        public StudentAPIController(ILogger<WeatherForecastController> logger, IStudentService stdSvc)
        {
            _logger = logger;
            _studentService = stdSvc;
        }

        [HttpGet]
        public ActionResult GetAllStudents()
        {
            var students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet]
        public ActionResult GetStudentInfoById(int id)
        {
            var info = _studentService.GetStudentInfoById(id);
            return Ok(info);
        }
    }
}
