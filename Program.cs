

using System.Text.Json.Serialization;
using Employee_Leave_Management_System.Data;

using Employee_Leave_Management_System.Repositories.Implementations;
using Employee_Leave_Management_System.Repositories.Interface;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers with JSON cycle handling (Combined into a single block)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Validation & Repositories
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();

// 3. Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection2")));

// 4. Adjusted CORS Policy to support Deployed Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", policy => 
    {
        policy
        .WithOrigins(
                "http://localhost:5173", // Your local Vite dev environment
                "https://your-frontend-app.vercel.app" // <-- REPLACE THIS with your actual deployed frontend URL later
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    
});

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}


var app = builder.Build();

// 5. Enable Swagger for both local testing and production tracking on Render
app.UseSwagger();
app.UseSwaggerUI();

// 6. Middleware Pipeline Execution Order
app.UseCors("Dev");

// Note: You can comment out HttpsRedirection if Render handles the SSL/HTTPS termination for you
app.UseHttpsRedirection(); 

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}


app.Run();