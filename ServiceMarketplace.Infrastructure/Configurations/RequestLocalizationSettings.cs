namespace ServiceMarketplace.Infrastructure.Configurations;

public class RequestLocalizationSettings
{
    public string DefaultRequestCulture { get; set; }

    public HashSet<string> SupportedCultures { get; set; }
}
