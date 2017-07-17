using System.Collections.Generic;

namespace Bankmeister.Business
{
    public interface IReportManager
    {
        void GenerateReport(IDictionary<string, string> arguments);
    }
}
