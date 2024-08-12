namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class EmailAlreadyExistsException : ClientException
{
    public EmailAlreadyExistsException(string? message) 
        : base(message)
    {
    }
}
