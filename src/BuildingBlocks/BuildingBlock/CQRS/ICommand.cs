namespace BuildingBlock.CQRS;

public interface ICommand
{
}

public interface ICommand<out TResponse>
{
}

