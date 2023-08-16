namespace Infrastructure.Utilities;

public static class TimeZone
{
    public static DateTime ConvertToEGYZone(this DateTime dateTime)
    {
        TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

        DateTime timeInEgypt = TimeZoneInfo.ConvertTimeFromUtc(dateTime, egyptTimeZone);

        return timeInEgypt;
    }
}
