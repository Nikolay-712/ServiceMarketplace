namespace ServiceMarketplace.Common.Exceptions.ServerExceptions;

public class NotSuccessfulIdentityOperationException : ServerException
{
    public NotSuccessfulIdentityOperationException(string? message) 
        : base(message)
    {
    }
}
