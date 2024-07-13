namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class ExistsCategoryNameException : ClientException
{
    public ExistsCategoryNameException(string? message) 
        : base(message)
    {
    }
}
