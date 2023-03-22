namespace Application.Services.GetTimeZone;

public class GetTimeZone : IGetTimeZone
{
    public DateTime GetApplicationTimeZone()
       => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Bahia Standard Time");
}
