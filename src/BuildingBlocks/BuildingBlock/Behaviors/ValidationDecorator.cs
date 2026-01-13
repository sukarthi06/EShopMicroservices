using BuildingBlock.CQRS;
using FluentValidation;

namespace BuildingBlock.Behaviors;

//builder.Services.Decorate(
//    typeof(ICommandHandler<,>),
//    typeof(ValidationDecorator<,>)
//);


public sealed class ValidationDecorator<TCommand, TResponse>
    : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    private readonly ICommandHandler<TCommand, TResponse> _inner;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationDecorator(
        ICommandHandler<TCommand, TResponse> inner,
        IEnumerable<IValidator<TCommand>> validators)
    {
        _inner = inner;
        _validators = validators;
    }

    public async Task<TResponse> HandleAsync(TCommand command)
    {
        var context = new ValidationContext<TCommand>(command);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context))
        );

        var failures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
            throw new ValidationException(failures);

        return await _inner.HandleAsync(command);
    }
}

