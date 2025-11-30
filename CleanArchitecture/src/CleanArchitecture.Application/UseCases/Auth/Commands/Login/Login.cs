
using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Secutiry;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Login;

public sealed record Login(string Username, string Password) : ICommand<AuthenticationTokenPair>;