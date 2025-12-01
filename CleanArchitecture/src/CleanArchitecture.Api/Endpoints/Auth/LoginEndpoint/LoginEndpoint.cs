using Azure.Core;
using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Application.UseCases.Auth.Commands.Login;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Endpoints.Auth.LoginEndpoint;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/auth/login", async (
                    [FromBody] LoginRequest request,
                    [FromServices] ICommandHandler<Login, AuthenticationTokenPair> handler,
                    HttpContext httpContext) =>
        {
            Login command = new(request.Username, request.Password);

            var result = await handler.Handle(command);

            if (!result.IsSuccess)
            {
                return Results.Unauthorized();
            }

            httpContext.Response.Cookies.Append(
              "refreshToken",
              result.Value.RefreshToken,
              new CookieOptions
              {
                  HttpOnly = true,
                  Secure = true,
                  SameSite = SameSiteMode.Lax,
                  Expires = result.Value.RefreshTokenExpiresOn
              }
            );

            return Results.Ok(new { accessToken = result.Value.AccessToken, accessTokenExpiresOn = result.Value.AccessTokenExpiresOn });
        })
        .WithName("Login")
        .WithTags("Auth")
        .Produces<Guid>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}

public sealed record LoginRequest(string Username, string Password);