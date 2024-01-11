using A.Contracts.DBSettings;
using System.Runtime;
using B.DatabaseAccess.DataAccess;
using B.DatabaseAccess.IDataAccess;
using C.BusinessLogic.ILoigcs;
using C.BusinessLogic.Logics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// configure mongoDB
builder.Services.Configure<MongoDBSetting>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<IStudentLogic, StudentLogic>();
builder.Services.AddSingleton<IStudentDataAccess, StudentDataAccess>();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline for swagger.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// permit the host http://localhost:4200
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.MapControllers();

app.Run();
