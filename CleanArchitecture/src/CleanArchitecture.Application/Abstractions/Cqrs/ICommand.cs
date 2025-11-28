namespace CleanArchitecture.Application.Abstractions.Cqrs;

public interface ICommand
{ 
}

public interface ICommand<out TResponse>
{ 
}