namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class ExistsTagException : ClientException
{
    public ExistsTagException(string? message) 
        : base(message)
    {
    }
}
