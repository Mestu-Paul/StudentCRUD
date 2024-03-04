 //using A.Contracts.Models;

 using A.Contracts.Models;
 using MassTransit;
//using MessageProducer.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly IPublishEndpoint _endpoint;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IBus bus, ILogger<StudentsController>logger, IPublishEndpoint endpoint)
        {
            _bus = bus;
            _logger = logger;
            _endpoint = endpoint;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateStudent([FromBody]UpdateStudent student)
        {
            _logger.LogInformation($"Sending request... {student.Username}");

            await _endpoint.Publish(student);

            _logger.LogInformation("Request sent");
            return Ok("Received your request, please wait ...");
        }
    }
}
