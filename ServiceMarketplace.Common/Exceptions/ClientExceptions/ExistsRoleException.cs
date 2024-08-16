namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class ExistsRoleException : ClientException
{
    public ExistsRoleException(string? message) 
        : base(message)
    {
    }
}
