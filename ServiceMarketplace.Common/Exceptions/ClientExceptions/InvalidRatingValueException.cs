namespace ServiceMarketplace.Common.Exceptions.ClientExceptions;

public class InvalidRatingValueException : ClientException
{
    public InvalidRatingValueException(string? message) 
        : base(message)
    {
    }
}
