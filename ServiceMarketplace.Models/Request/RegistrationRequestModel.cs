namespace ServiceMarketplace.Models.Request;

public record RegistrationRequestModel(
    string Email,
    string Password,
    string ConfirmPassword,
    string FullName);

