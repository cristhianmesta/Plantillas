using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Application.UseCases.Auth.Commands.Refresh;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Endpoints.Auth.RefreshTokenEndpoint;

public static class RefreshTokenEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/auth/refresh", async (
                    [FromServices] ICommandHandler<Refresh, AuthenticationTokenPair> handler,
                    HttpContext httpContext) =>
        {
            var refreshToken = httpContext.Request.Cookies["refreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
                return Results.Unauthorized();

            Refresh command = new(refreshToken);

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
        .WithName("RefreshToken")
        .WithTags("Auth")
        .Produces<Guid>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized);

        return endpoints;
    }
}