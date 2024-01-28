using A.Contracts.DBSettings;
using System.Runtime;
using System.Text;
using B.DatabaseAccess.DataAccess;
using B.DatabaseAccess.IDataAccess;
using B1.RedisCache;
using C.BusinessLogic.ILoigcs;
using C.BusinessLogic.Logics;
using D.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddSingleton<ICache, Cache>();
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

app.UseAuthentication();
app.UseAuthorization();

// permit the host http://localhost:4200
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.MapControllers();

app.Run();
