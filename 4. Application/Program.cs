using _1.BOL.DBSettings;
using _2._DAL.DataServices;
using _2._DAL.IDataServices;
using _3._BLL.ILogics;
using _3._BLL.Logics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("MongoDB"));
// builder.Services.AddSingleton<StudentDAL>();
// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSingleton<IStudentLogic, StudentLogic>();
builder.Services.AddSingleton<IStudentDAL, StudentDAL>();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.MapControllers();

app.Run();
