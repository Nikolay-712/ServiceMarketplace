namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class UserRoleExistsException : ClientException
{
    public UserRoleExistsException(string? message) 
        : base(message)
    {
    }
}
