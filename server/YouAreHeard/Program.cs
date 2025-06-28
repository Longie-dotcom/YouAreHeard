using Microsoft.Extensions.FileProviders;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Implementation;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services;
using YouAreHeard.Services.Implementation;
using YouAreHeard.Services.Interfaces;
using YouAreHeard.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Application Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IZoomService, ZoomService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ITreatmentPlanService, TreatmentPlanService>();
builder.Services.AddScoped<ILabTestService, LabTestService>();
builder.Services.AddScoped<IPatientProfileService, PatientProfileService>();
builder.Services.AddScoped<IPayOSService, PayOSService>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IARVRegimenRepository, ARVRegimenRepository>();
builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
builder.Services.AddScoped<IPillRemindTimesRepository, PillRemindTimesRepository>();
builder.Services.AddScoped<ITreatmentPlanRepository, TreatmentPlanRepository>();
builder.Services.AddScoped<ILabResultRepository, LabResultRepository>();
builder.Services.AddScoped<ITestStageRepository, TestStageRepository>();
builder.Services.AddScoped<ITestTypeRepository, TestTypeRepository>();
builder.Services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
builder.Services.AddScoped<ITestMetricRepository, TestMetricRepository>();
builder.Services.AddScoped<ITestMetricValueRepository, TestMetricValueRepository>();

var app = builder.Build();

// Enable Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "YouAreHeard API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Serve static files
app.UseStaticFiles();
var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

// Email config
try
{
    var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
    EmailSettingsContext.Initialize(emailSettings);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to initialize email!");
}

// Zoom config
try
{
    var zoomSettings = builder.Configuration.GetSection("Zoom").Get<ZoomSettings>();
    ZoomSettingContext.Initialize(zoomSettings);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to initialize Zoom!");
}

// PayOS config
try
{
    var payosConfig = builder.Configuration.GetSection("PayOS").Get<PayOSSettings>();
    PayOSSettingContext.Initialize(payosConfig);
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Failed to initialize PayOS!");
}

// DB config
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