using A.Contracts.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace MessageProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IBus bus, ILogger<StudentsController>logger)
        {
            _bus = bus;
            _logger = logger;
        }

        [HttpPut("/update")]
        public async Task<IActionResult> UpdateStudent(UpdateStudent student)
        {
            _logger.LogInformation($"Sending request... {student.Username}");

            await _bus.Publish(student);

            _logger.LogInformation("Request sent");
            return Ok("Received your request, please wait ...");
        }
    }
}
