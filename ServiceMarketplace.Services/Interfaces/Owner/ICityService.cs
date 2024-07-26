namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface ICityService
{
    Task ValidateSelectedCityAsync(Guid cityId);
}
