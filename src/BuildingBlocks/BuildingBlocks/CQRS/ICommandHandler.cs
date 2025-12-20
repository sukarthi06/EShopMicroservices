using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Void command handler
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand>
    : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand<Unit>
{
}
/// <summary>
/// Command handler with return type
/// Defines a contract for handling commands of type <typeparamref name="TCommand"/> and producing a response of type
/// <typeparamref name="TResponse"/>.
/// </summary>
/// <remarks>This interface extends <see cref="IRequestHandler{TRequest, TResponse}"/> to provide a specialized
/// handler for commands. Implementations of this interface are responsible for processing the given command and
/// returning an appropriate response.</remarks>
/// <typeparam name="TCommand">The type of the command to be handled. Must implement <see cref="ICommand{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of the response produced by handling the command. Must be a non-nullable type.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>    
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}
