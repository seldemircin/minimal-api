namespace ch_14_relations.Entities.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {

    }
}