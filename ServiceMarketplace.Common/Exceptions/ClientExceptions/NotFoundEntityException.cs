namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class NotFoundEntityException : ClientException
{
    public NotFoundEntityException(string? message) 
        : base(message)
    {
    }
}
