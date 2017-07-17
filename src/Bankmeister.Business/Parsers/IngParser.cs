using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bankmeister.Models;
using Bankmeister.Services;

namespace Bankmeister.Business.Parsers
{
    public class IngParser : IParser
    {
        private const string FileExtension = "csv";
        private readonly ICsvService _csvService;
        private readonly IFileService _fileService;

        public IngParser(
            ICsvService csvService,
            IFileService fileService)
        {
            _csvService = csvService;
            _fileService = fileService;
        }

        public string Key => "ing";

        public IEnumerable<MutationModel> ParseMutations(IDictionary<string, string> arguments)
        {
            var result = new List<MutationModel>();

            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (!arguments.TryGetValue(Constants.PathArgumentName, out string path))
            {
                throw new ArgumentException($"Argument not set: {Constants.PathArgumentName}");
            }

            var files = _fileService.GetFiles(path, $"*.{FileExtension}");
            foreach (string file in files)
            {
                string contents = _fileService.ReadAllText(file);
                var rows = _csvService.ParseCsvRows(contents)
                    .Skip(1)
                    .ToArray();

                foreach (var row in rows)
                {
                    int modifier = row[5] == "Bij" ? 1 : -1;
                    double amount = double.Parse(row[6].Replace(",", "."), CultureInfo.InvariantCulture) * modifier;
                    var dateTime = DateTime.ParseExact(row[0], "yyyyMMdd", CultureInfo.InvariantCulture);

                    result.Add(new MutationModel
                    {
                        Amount = amount,
                        DateTime = dateTime,
                        Description = row[8],
                        FromAccount = row[2],
                        MutationType = row[7],
                        Name = row[1],
                        ToAccount = row[3]
                    });
                }
            }

            return result
                .OrderBy(m => m.DateTime);
        }
    }
}
