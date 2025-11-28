using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.UseCases.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Endpoints.Users.CreateUserEndpoint;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/users/", async (
                    [FromBody] CreateUserRequest request,
                    [FromServices] ICommandHandler<CreateUser, Guid> command) =>
        {
            CreateUser createUser = new(request.Username, request.Email, request.Password);

            var result = await command.Handle(createUser);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error ?? "No se pudo crear el usuario.");
            }
            return Results.Ok(result.Value);
        })
        .WithName("CreateUser")
        .WithTags("Users")
        .Produces<Guid>(StatusCodes.Status200OK)
        .Produces<string>(StatusCodes.Status400BadRequest); 

        return endpoints;
    }
}