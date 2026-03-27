using MediCore.Api.Repositories.UserRepo;
using MediCore.Api.Services.UserServices;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MediCoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("MediCore.Api")));
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Service
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();


//  Must be FIRST — before all other middleware
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        //  Log real error to terminal
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
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

