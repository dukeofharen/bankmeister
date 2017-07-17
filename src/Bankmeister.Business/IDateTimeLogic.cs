using System;
using System.Collections.Generic;
using Bankmeister.Models;
using Bankmeister.Models.Enums;

namespace Bankmeister.Business
{
    public interface IDateTimeLogic
    {
        IEnumerable<DateTimeFromToModel> CalculateDateTimeRanges(PeriodType periodType, DateTime beginDateTime, DateTime endDateTime);
    }
}
