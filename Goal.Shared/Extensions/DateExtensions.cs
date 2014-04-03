using System;

namespace Goals.Shared.Extensions
{
    public static class DateExtensions
    {
        public static DateTime ConvertToLocal(this DateTime mydate)
        {
            DateTime iKnowThisIsUtc = mydate;
            DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind(
                iKnowThisIsUtc,
                DateTimeKind.Utc);
            return runtimeKnowsThisIsUtc.ToLocalTime();
        }
    }
}
