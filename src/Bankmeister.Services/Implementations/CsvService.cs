using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace Bankmeister.Services.Implementations
{
    internal class CsvService : ICsvService
    {
        public IEnumerable<string[]> ParseCsvRows(string csv)
        {
            var result = new List<string[]>();
            using (var stringReader = new StringReader(csv))
            using (var parser = new CsvParser(stringReader))
            {
                while (true)
                {
                    var row = parser.Read();
                    if (row == null)
                    {
                        break;
                    }

                    result.Add(row);
                }
            }

            return result;
        }
    }
}
