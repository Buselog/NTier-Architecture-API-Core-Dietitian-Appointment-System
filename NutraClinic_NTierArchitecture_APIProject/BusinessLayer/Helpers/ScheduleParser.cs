using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Helpers
{
    public static class ScheduleParser
    {
        public static HashSet<DayOfWeek> ParseWorkingDays(string? input)
        {
            var result = new HashSet<DayOfWeek>();
            if (string.IsNullOrWhiteSpace(input))
                return result;

            var parts = input.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var raw in parts)
            {
                var token = raw.Trim();

                var rangeParts = token.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (rangeParts.Length == 2)
                {
                    var start = ParseDayAbbrev(rangeParts[0].Trim());
                    var end = ParseDayAbbrev(rangeParts[1].Trim());
                    if (start != null && end != null)
                        AddDayRange(result, start.Value, end.Value);
                    continue;
                }

                var day = ParseDayAbbrev(token);
                if (day != null)
                    result.Add(day.Value);
            }

            return result;
        }

        public static (TimeSpan? start, TimeSpan? end) ParseWorkingHours(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return (null, null);

            var parts = input.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return (null, null);

            if (TimeSpan.TryParse(parts[0].Trim(), CultureInfo.InvariantCulture, out var start) &&
                TimeSpan.TryParse(parts[1].Trim(), CultureInfo.InvariantCulture, out var end))
            {
                return (start, end);
            }

            return (null, null);
        }
        private static DayOfWeek? ParseDayAbbrev(string token)
        {
            token = token.Trim().ToLowerInvariant();

            return token switch
            {
                "mon" or "monday" => DayOfWeek.Monday,
                "tue" or "tues" or "tuesday" => DayOfWeek.Tuesday,
                "wed" or "wednesday" => DayOfWeek.Wednesday,
                "thu" or "thur" or "thurs" or "thursday" => DayOfWeek.Thursday,
                "fri" or "friday" => DayOfWeek.Friday,
                "sat" or "saturday" => DayOfWeek.Saturday,
                "sun" or "sunday" => DayOfWeek.Sunday,
                _ => null
            };
        }
        private static void AddDayRange(HashSet<DayOfWeek> set, DayOfWeek start, DayOfWeek end)
        {
            int s = (int)start;
            int e = (int)end;

            int i = s;
            while (true)
            {
                set.Add((DayOfWeek)i);
                if (i == e) break;
                i = (i + 1) % 7;
            }
        }
    }
}
