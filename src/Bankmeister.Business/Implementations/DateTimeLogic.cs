using System;
using System.Collections.Generic;
using Bankmeister.Models;
using Bankmeister.Models.Enums;

namespace Bankmeister.Business.Implementations
{
    internal class DateTimeLogic : IDateTimeLogic
    {
        public IEnumerable<DateTimeFromToModel> CalculateDateTimeRanges(PeriodType periodType, DateTime beginDateTime, DateTime endDateTime)
        {
            var result = new List<DateTimeFromToModel>();
            if (periodType == PeriodType.Everything)
            {
                result.Add(new DateTimeFromToModel
                {
                    From = beginDateTime.Date,
                    To = endDateTime.Date.AddDays(1).AddSeconds(-1)
                });
            }
            else
            {
                DateTime firstDayInPeriod = GetFirstDayInPeriod(periodType, beginDateTime);
                DateTime pointer = firstDayInPeriod;

                do
                {
                    var model = new DateTimeFromToModel
                    {
                        From = pointer
                    };

                    switch (periodType)
                    {
                        case PeriodType.Daily:
                            pointer = pointer.AddDays(1);
                            break;
                        case PeriodType.Weekly:
                            pointer = pointer.AddDays(7);
                            break;
                        case PeriodType.Monthly:
                            pointer = pointer.AddMonths(1);
                            break;
                        case PeriodType.Yearly:
                            pointer = pointer.AddYears(1);
                            break;
                    }

                    model.To = pointer.AddSeconds(-1);
                    result.Add(model);
                } while (pointer <= endDateTime);
            }

            return result;
        }

        private static DateTime GetFirstDayInPeriod(PeriodType periodType, DateTime beginDateTime)
        {
            DateTime firstDayInPeriod;
            switch (periodType)
            {
                case PeriodType.Daily:
                    firstDayInPeriod = beginDateTime.Date;
                    break;
                case PeriodType.Weekly:
                    firstDayInPeriod = StartOfWeek(beginDateTime.Date, DayOfWeek.Monday);
                    break;
                case PeriodType.Monthly:
                    firstDayInPeriod = new DateTime(beginDateTime.Year, beginDateTime.Month, 1, 0, 0, 0);
                    break;
                case PeriodType.Yearly:
                    firstDayInPeriod = new DateTime(beginDateTime.Year, 1, 1, 0, 0, 0);
                    break;
                default:
                    throw new InvalidOperationException("Invalid period type.");
            }

            return firstDayInPeriod;
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}
