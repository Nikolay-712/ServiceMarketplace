using System.Text.RegularExpressions;

namespace ServiceMarketplace.Models.Validators;

public static class ValidatorHelper
{
    private static readonly string phoneNumberRegexPattern = "^(?:\\+359(886|887|888|889|896|897|898|899)\\d{3}\\d{3}|08(86|87|88|89|96|97|98|99)\\d{3}\\d{3})$";
    private static readonly string urlRegexPattern = "^(https?:\\/\\/)?((([a-zA-Z0-9\\-]+\\.)+[a-zA-Z]{2,}))(:\\d{1,5})?(\\/[^\\s]*)?$";
    public static bool BulgarianMobileNumberValidation(string phoneNumber)
    {
        Regex regex = new(phoneNumberRegexPattern);
        return regex.IsMatch(phoneNumber);
    }

    public static bool UrlValidation(string url)
    {
        Regex urlRegex = new(urlRegexPattern);
        return urlRegex.IsMatch(url);
    }
}
