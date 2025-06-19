using AuthDemo.API.ServiceCollectionExtensions;
using AuthDemo.API.Swagger;
using AuthDemo.Infrastructure.BusinessLogic;
using AuthDemo.Infrastructure.BusinessLogic.Auth;
using AuthDemo.Infrastructure.Dapper;
using AuthDemo.Infrastructure.Dapper.BooksRepository;
using AuthDemos.Core.DTOs.TypeForms;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();
builder.Services.AddAuthDemoDbContext(builder.Configuration);
builder.Services.AddIdentityService();
builder.Services.AddJwtService(builder.Configuration);

builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IDapperRepository<>), typeof(DapperRepository<>));
builder.Services.AddScoped<IBookRepository, BooksRepository>();

builder.Services.AddTransient<IAuthBusinessLogic, AuthBusinessLogic>();
builder.Services.AddTransient<IAuthorBusinessLogic, AuthorBusinessLogic>();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<TypeFormCredentials>(options => builder.Configuration.GetSection("TypeForm").Bind(options));
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
