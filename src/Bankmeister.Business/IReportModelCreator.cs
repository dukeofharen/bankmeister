using System.Collections.Generic;
using Bankmeister.Models;
using Bankmeister.Models.Enums;

namespace Bankmeister.Business
{
    public interface IReportModelCreator
    {
        IEnumerable<ReportModel> GetReportModels(IEnumerable<MutationModel> mutations, PeriodType periodType, double beginAmount = 0);
    }
}
