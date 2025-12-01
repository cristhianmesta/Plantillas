using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.UseCases.Users.Commands.ChangePassword;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Endpoints.Users.ChangePasswordEndpoint;

public static class ChangePasswordEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPatch("/users/change-password", async (
                    [FromBody] ChangePasswordRequest request,
                    [FromServices] ICommandHandler<ChangePassword> handler) =>
        {
            ChangePassword command = new(request.Username, request.Password, request.NewPassword);

            var result = await handler.Handle(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error ?? "No se pudo crear el usuario.");
            }
            return Results.NoContent();
        })
        .WithName("ChangePassword")
        .WithTags("Users")
        .Produces<Guid>(StatusCodes.Status204NoContent)
        .Produces<string>(StatusCodes.Status400BadRequest); 

        return endpoints;
    }
}