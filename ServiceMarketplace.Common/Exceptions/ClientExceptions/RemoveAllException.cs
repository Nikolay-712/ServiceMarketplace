namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class RemoveAllException : ClientException
{
    public RemoveAllException(string? message) 
        : base(message)
    {
    }
}
