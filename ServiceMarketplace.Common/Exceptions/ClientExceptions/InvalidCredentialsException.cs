namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class InvalidCredentialsException : ClientException
{
    public InvalidCredentialsException(string? message) 
        : base(message)
    {
    }
}
