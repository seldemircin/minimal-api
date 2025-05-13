namespace ch_15_identity.Entities.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {

    }
}