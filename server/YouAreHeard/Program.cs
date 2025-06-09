using Microsoft.Data.SqlClient;
using Microsoft.Extensions.FileProviders;
using YouAreHeard.Models;

// Add services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Needed if using cookies
    });
});
builder.Services.AddControllers();
var app = builder.Build();

app.UseStaticFiles(); // for wwwroot
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Email setting
try
{
    var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
    EmailSettingsContext.Initialize(emailSettings);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to initialize email!");
}
// Connect database
try
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    DBContext.Initialize(connectionString);
    app.Logger.LogInformation("Database connected successfully!");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to connect to database.");
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
