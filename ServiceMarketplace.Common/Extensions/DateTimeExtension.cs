using System.Globalization;

namespace ServiceMarketplace.Common.Extensions;

public static class DateTimeExtension
{
    public static string DateFormat(this DateTime dateTime)
    {
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        return dateTime.ToString(cultureInfo);
    }
}
