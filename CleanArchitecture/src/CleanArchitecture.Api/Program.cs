using CleanArchitecture.Api;
using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File(
                                path: "Logs/log-.txt",
                                rollingInterval: RollingInterval.Day,
                                retainedFileCountLimit: 7,
                                shared: true,                 // OK
                                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                        .CreateLogger();


try
{
    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

    builder.Host.UseSerilog();


    builder.Services.AddApplication();
    builder.Services.AddPersistence(builder.Configuration);
    builder.Services.AddInfraestructure(builder.Configuration);
    builder.Services.AddPresentation();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
        app.MapScalarApiReference();
    }
    app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseAuthentication();
    app.UseAuthorization();

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

