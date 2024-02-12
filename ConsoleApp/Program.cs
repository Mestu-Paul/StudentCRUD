using MassTransit;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(new Uri("rabbitmq://localhost"), h =>
    {
        h.Username("guest");
        h.Password("guest");
    });
});

await busControl.StartAsync();

await busControl.Publish(new MyMessage { Content = "Hello from console!" });