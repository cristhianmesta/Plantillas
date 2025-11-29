using CleanArchitecture.Application.Abstractions.Cqrs;

namespace CleanArchitecture.Application.UseCases.Users.Commands.ChangePassword;

public sealed record ChangePassword(string Username, string OldPassword, string NewPassword) : ICommand;
