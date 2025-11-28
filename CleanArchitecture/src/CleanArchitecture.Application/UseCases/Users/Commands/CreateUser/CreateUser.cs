using CleanArchitecture.Application.Abstractions.Cqrs;

namespace CleanArchitecture.Application.UseCases.Users.Commands.CreateUser;

public sealed record CreateUser(string Username, string Email, string Password) : ICommand<Guid>;
