using Bankmeister.Models;

namespace Bankmeister.Business.ReportGenerators
{
    public interface IReportGenerator
    {
        string Key { get; }

        string Extension { get; }

        byte[] GenerateReport(ReportModel reportModel);
    }
}
