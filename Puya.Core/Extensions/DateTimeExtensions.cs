using System;
using System.Globalization;

namespace Puya.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToPersian(this DateTime d, bool includeTime = false)
        {
            var pc = new PersianCalendar();
            var year = pc.GetYear(d);
            var month = pc.GetMonth(d);
            var day = pc.GetDayOfMonth(d);
            var time = includeTime ? $" {pc.GetHour(d)}:{pc.GetMinute(d)}:{pc.GetSecond(d)}" : "";

            return $"{year}/{(month < 10 ? "0" + month : month.ToString())}/{(day < 10 ? "0" + day : day.ToString())}{time}";
        }
    }
}
