namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class ExistsCityException : ClientException
{
    public ExistsCityException(string? message) 
        : base(message)
    {
    }
}
