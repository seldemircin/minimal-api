namespace ch_12_auto_mapper.Entities.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {

    }
}