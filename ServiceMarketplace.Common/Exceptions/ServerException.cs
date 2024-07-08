namespace ServiceMarketplace.Common.Exceptions;

public class ServerException : Exception
{
    public ServerException(string? message) 
        : base(message)
    {
    }
}
