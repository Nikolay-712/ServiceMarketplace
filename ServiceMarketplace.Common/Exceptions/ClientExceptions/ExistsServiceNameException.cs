namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class ExistsServiceNameException : ClientException
{
    public ExistsServiceNameException(string? message)
        : base(message)
    {
    }
}
