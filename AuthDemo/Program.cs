using AuthDemo.API.ServiceCollectionExtensions;
using AuthDemo.API.Swagger;
using AuthDemo.Infrastructure.BusinessLogic.Auth;
using AuthDemo.Infrastructure.Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddAuthDemoDbContext(builder.Configuration);
builder.Services.AddIdentityService();
builder.Services.AddJwtService(builder.Configuration);

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
builder.Services.AddTransient<IAuthBusinessLogic, AuthBusinessLogic>();
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
