using System.Globalization;

namespace ServiceMarketplace.Common.Extensions;

public static class DateTimeExtension
{
    private static readonly CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

    public static string DateFormat(this DateTime dateTime)
    {
        
        return dateTime.ToString(cultureInfo);
    }

    public static string TimeFormat(this TimeOnly timeOnly)
    {
        return timeOnly.ToString(cultureInfo);
    }
}
