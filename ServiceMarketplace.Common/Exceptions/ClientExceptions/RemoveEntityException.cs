namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class RemoveEntityException : ClientException
{
    public RemoveEntityException(string? message) 
        : base(message)
    {
    }
}
