using A.Contracts.DBSettings;
using System.Text;
using B.DatabaseAccess.DataAccess;
using B.DatabaseAccess.IDataAccess;
using B1.RedisCache;
using C.BusinessLogic.Consumers;
using C.BusinessLogic.ILoigcs;
using C.BusinessLogic.Logics;
using C.BusinessLogic.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using D.Application.Middleware;
using MassTransit;
using D.Application.WebSocket;

var builder = WebApplication.CreateBuilder(args);

// configure mongoDB
builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IStudentLogic, StudentLogic>();
builder.Services.AddSingleton<IStudentDataAccess, StudentDataAccess>();
builder.Services.AddSingleton<ITeacherDataAccess, TeacherDataAccess>();
builder.Services.AddSingleton<ITeacherLogic, TeacherLogic>();
builder.Services.AddSingleton<IAccountDataAccess,AccountDataAccess>();
builder.Services.AddSingleton<IAccountLogic,AccountLogic>();
builder.Services.AddSingleton<ISharedDataAccess, SharedDataAccess>();
builder.Services.AddSingleton<IMessageDataAccess,MessageDataAccess>();
builder.Services.AddSingleton<IMessageLogic,MessageLogic>();
builder.Services.AddSingleton<ICache, Cache>();

builder.Services.AddSingleton<WebSocketHandler>();

builder.Services.AddSingleton<StudentUpdateConsumer>();

builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer =  false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("student", policy => policy.RequireRole("student"));
    options.AddPolicy("teacher", policy => policy.RequireRole("teacher"));
});

// RabbitMQ configuration 
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<StudentUpdateConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:RabbitMQ"]);
        cfg.ReceiveEndpoint("UpdateStudent-queue", c =>
        {
            c.ConfigureConsumer< StudentUpdateConsumer>(ctx);
        });
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediator((cfg) => cfg.AddConsumersFromNamespaceContaining<StudentUpdateConsumer>());



var app = builder.Build();

// Configure the HTTP request pipeline for swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();


// permit the host http://localhost:4200
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));


app.MapControllers();

app.UseWebSockets();
app.Map("/ws", builder =>
{
    builder.UseMiddleware<WebSocketMiddleware>();
});

app.Run();

