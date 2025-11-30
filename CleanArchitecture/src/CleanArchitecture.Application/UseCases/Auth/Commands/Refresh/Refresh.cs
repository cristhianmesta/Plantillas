using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Secutiry;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Refresh;

public sealed record Refresh(string RefreshToken) : ICommand<AuthenticationTokenPair>;


