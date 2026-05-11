using System.Text;
using MediCore.Api.Repositories;
using MediCore.Api.Repositories.TokenRepo;
using MediCore.Api.Repositories.UserRepo;
using MediCore.Api.Services;
using MediCore.Api.Services.AuthServices;
using System.Text.Json.Serialization;
using MediCore.Api.Services.UserServices;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MediCore.Api.Repositories.AuditRepo;
using Microsoft.AspNetCore.Diagnostics;
using MediCore.Api.Services.EMR;
using MediCore.Api.Services.PrescriptionServices;
using MediCore.Api.Repositories.PrescriptionRepo;
using Microsoft.OpenApi;
using MediCore.Api.Repositories.AppointmentRepository;
using MediCore.Api.Services.AppointmentServices;
using MediCore.Api.Repositories.LabTestRepository; 
using MediCore.Api.Services.LabTestServices;
using MediCore.Api.Services.PatientServices;
using MediCore.Api.Repositories.PatientRepo;
using Microsoft.AspNetCore.Mvc;
using MediCore.Api.Mapper;
using MediCore.Api.Repositories.DispenseRepo;
using MediCore.Api.Services.DispenseServices;
using MediCore.Api.Repositories.ComplianceRepo;
using MediCore.Api.Services.ComplianceServices;
using MediCore.Api.Repositories.LabReportRepo;
using MediCore.Api.Services.LabReportServices;
using MediCore.Api.Repositories.BillingRepo;
using MediCore.Api.Services.PatientDocumentServices;
using MediCore.Api.Repositories.PatientDocumentRepo;
using MediCore.Api.Repositories.InsuranceClaimRepo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IEmrService, EmrService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<ILabTestRepository, LabTestRepository>();
builder.Services.AddScoped<ILabTestService, LabTestService>();
builder.Services.AddScoped<IPatientRepository,PatientRepository>();
builder.Services.AddScoped<IPatientService,PatientService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IScheduleRepository,ScheduleRepository>();
builder.Services.AddControllers();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IDispenseRepository, DispenseRepository>();
builder.Services.AddScoped<IDispenseService, DispenseService>();
builder.Services.AddScoped<IComplianceRepository, ComplianceRepository>();
builder.Services.AddScoped<IComplianceService, ComplianceService>();
builder.Services.AddScoped<ILabReportRepository, LabReportRepository>();
builder.Services.AddScoped<ILabReportService, LabReportService>();
builder.Services.AddScoped<IPatientDocumentService,PatientDocumentService>();
builder.Services.AddScoped<IPatientDocumentRepo,PatientDocumentRepo>();

// builder.Services.AddAutoMapper(typeof(MappingProfile));
// builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IBillRepository,BillRepository>();
builder.Services.AddScoped<IInsuranceClaimService,InsuranceClaimService>();
builder.Services.AddScoped<IInsuranceClaimRepository,InsuranceClaimRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MediCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MediCore.Api")));     
        
//Read JWT Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters=new TokenValidationParameters
    {
        ValidateIssuer=true,
        ValidateAudience=true,
        ValidateLifetime=true,
        ValidateIssuerSigningKey=true,

        ValidIssuer=jwtSettings["Issuer"],
        ValidAudience=jwtSettings["Audience"],
        IssuerSigningKey=new SymmetricSecurityKey(key),
        ClockSkew=TimeSpan.Zero
    };
    
 options.Events = new JwtBearerEvents
        {
            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Access denied. You are not authorized to access this resource."
                });
            }
        };

});

// Added Authorization
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authentication using Bearer scheme"
    });
    options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", doc), new List<string>() }
    });
}

);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
       options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
        
    });

builder.Services.AddControllers()
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var firstError = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .FirstOrDefault() ?? "Invalid request.";

        return new BadRequestObjectResult(new { error = firstError });
    };
});
var app = builder.Build();

// Must be FIRST — before all other middleware
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        // Log real error to terminal
        Console.WriteLine($"EXCEPTION TYPE: {ex?.GetType().Name}");
        Console.WriteLine($"EXCEPTION MESSAGE: {ex?.Message}");
        Console.WriteLine($"STACK TRACE: {ex?.StackTrace}");

        (int status, object body) = ex switch
        {
            ArgumentException ae      => (400, (object)new { error = ae.Message }),
            InvalidOperationException => (409, (object)new { error = ex.Message }),
            _                         => (500, (object)new { error = ex?.Message ?? "An unexpected error occurred." })
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(body);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
//Middleware Pipeline
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.Run();