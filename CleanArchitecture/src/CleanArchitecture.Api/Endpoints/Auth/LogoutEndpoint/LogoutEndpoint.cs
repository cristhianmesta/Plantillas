using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Application.UseCases.Auth.Commands.Logout;
using CleanArchitecture.Application.UseCases.Auth.Commands.Refresh;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Endpoints.Auth.LogoutEndpoint;

public static class LogoutEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/auth/logout", async (
                    [FromServices] ICommandHandler<Logout> handler,
                    HttpContext httpContext) =>
        {
            var refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Results.Unauthorized();

            Logout command = new(refreshToken);

            var result = await handler.Handle(command);

            httpContext.Response.Cookies.Append(
              "refreshToken",
              "",
              new CookieOptions
              {
                  HttpOnly = true,
                  Secure = true,
                  SameSite = SameSiteMode.Lax,
                  Expires = DateTime.Now.AddDays(-1),
              }
            );

            if (!result.IsSuccess)
            {
                return Results.NotFound(new { status = "NotFound", message = "Se finalizó la sesión." });
            }
            return Results.Ok(new { status = "ok", message = "Se finalizó la sesión." });
        })
        .WithName("Logout")
        .WithTags("Auth")
        .Produces<Guid>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}