using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text.Json.Serialization;
using Todos.DataContext.Persistence;
using Todos.Domain.Model;
using Todos.Repository.IRepository;
using Todos.Repository.Repository;
using TodosService.IServices;
using TodosService.Services;
using Utilitities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodosConnectionString"));
}
);
builder.Services.Configure<ApiModel>(options => builder.Configuration.GetSection("Api").Bind(options));
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<TodoContext>();
builder.Services.AddScoped<Category>();
builder.Services.AddScoped<TodoItems>();
builder.Services.AddScoped<DataObject>();
builder.Services.AddScoped<TodosResponseMapper>();
builder.Services.AddScoped<WeatherResposeMapper>();
builder.Services.AddScoped<Condition>();
builder.Services.AddScoped<StandardResponse>();
builder.Services.AddScoped<TodosModel>();
builder.Services.AddScoped<TodosUpdateRapper>();
builder.Services.AddScoped<ApiResponse>();
builder.Services.AddScoped<ResponseHelper>();
builder.Services.AddHttpClient();



builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition
                        = JsonIgnoreCondition.WhenWritingNull;
});

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
