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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddControllers();
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
});

// Added Authorization
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
       options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
        
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

