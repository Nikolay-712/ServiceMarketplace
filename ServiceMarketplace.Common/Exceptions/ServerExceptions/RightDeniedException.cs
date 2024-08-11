namespace ServiceMarketplace.Common.Exceptions.ServerExceptions;

public class RightDeniedException : ServerException
{
    public RightDeniedException(string? message) 
        : base(message)
    {
    }
}
