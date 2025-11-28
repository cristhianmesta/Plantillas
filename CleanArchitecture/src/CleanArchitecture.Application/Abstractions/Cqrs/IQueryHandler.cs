using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.Abstractions.Cqrs;

public interface IQueryHandler<in TQuery, TResponse> 
           where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}