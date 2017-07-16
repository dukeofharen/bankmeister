using System.Collections.Generic;

namespace Bankmeister.Services
{
    public interface ICsvService
    {
        IEnumerable<string[]> ParseCsvRows(string csv);
    }
}
