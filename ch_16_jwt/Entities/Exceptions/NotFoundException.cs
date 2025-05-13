namespace ch_16_jwt.Entities.Exceptions;

public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message) : base(message)
    {

    }
}