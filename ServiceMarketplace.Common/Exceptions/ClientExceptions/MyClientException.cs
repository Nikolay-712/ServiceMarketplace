namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class MyClientException : ServerException
{
    public MyClientException(string? message) 
        : base(message)
    {
    }
}
