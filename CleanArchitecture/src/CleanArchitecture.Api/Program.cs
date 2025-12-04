using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Api.Infraestructure;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

//app.UseAuthentication();

//app.UseAuthorization();

app.MapGet("/", async () =>
{
    return new 
    {
        Status = "OK",
        Message = "CleanArchitecture is running..."
    };
})
.WithName("Inicio")
.WithTags("Inicio");

app.AutoRegisterEndpoints();

app.Run();