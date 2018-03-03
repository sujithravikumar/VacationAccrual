using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VacationAccrual
{
    public class Global
    {
        public static List<SelectListItem> GetStartDateItems()
        {
            List<SelectListItem> startDateList = new List<SelectListItem>();
            DateTime startDate = DateTime.Now;

            int diff = DayOfWeek.Sunday - startDate.DayOfWeek;
            DateTime weekBegin = startDate.AddDays(diff);

            var calendar = new GregorianCalendar();
            var weekNumber = calendar.GetWeekOfYear(weekBegin, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            int biweeklyKey = weekNumber % 2;

            if (biweeklyKey == 0)
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.ToString("MM-dd-yy") });
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-7).ToString("MM-dd-yy") });
            }
            else
            {
                startDateList.Add(new SelectListItem { Text = weekBegin.AddDays(-7).ToString("MM-dd-yy") });
                startDateList.Add(new SelectListItem { Text = weekBegin.ToString("MM-dd-yy") });
            }

            return startDateList;
        }
    }
}
