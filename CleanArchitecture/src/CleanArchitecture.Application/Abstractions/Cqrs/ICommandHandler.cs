using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.Abstractions.Cqrs;

public interface ICommandHandler<in TCommand>
           where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);

}

public interface ICommandHandler<in TCommand, TResponse> 
           where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}