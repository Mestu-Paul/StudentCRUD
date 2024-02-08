using A.Contracts.Models;
using B1.RedisCache;
using B.DatabaseAccess.IDataAccess;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;

namespace C.BusinessLogic.Consumers
{
    public class StudentUpdateConsumer : IConsumer<UpdateStudent>
    {
        private readonly IStudentDataAccess _studentDataAccess;
        private readonly IMediator _mediator;
        private readonly ILogger<StudentUpdateConsumer> _logger;
        private readonly ICache _redisCache;


        public StudentUpdateConsumer(IStudentDataAccess studentDataAccess, IMediator mediator, ILogger<StudentUpdateConsumer> logger, ICache redisCache)
        {
            _studentDataAccess = studentDataAccess;
            _mediator = mediator;
            _logger = logger;
            _redisCache = redisCache;
        }
        public async Task Consume(ConsumeContext<UpdateStudent> context)
        {
            UpdateStudent student = context.Message;
            _logger.LogInformation($"-------------------- waiting to update {student.Username}");

            await Task.Delay(TimeSpan.FromSeconds(30));

            _logger.LogInformation("-------------------- update started");

            
            await _studentDataAccess.UpdateStudentAsync(student);
            await _redisCache.ClearCache();

            _logger.LogInformation($"-------------------- update complete {student.Username}");
        }
    }
}
