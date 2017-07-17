using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Bankmeister.Business.ReportGenerators;
using Bankmeister.Models.Enums;
using Bankmeister.Services;

namespace Bankmeister.Business.Implementations
{
    public class ReportManager : IReportManager
    {
        private readonly IReportModelCreator _reportModelCreator;
        private readonly IParserFactory _parserFactory;
        private readonly IFileService _fileService;
        private readonly IReportGeneratorFactory _reportGeneratorFactory;

        public ReportManager(
            IReportModelCreator reportModelCreator,
            IParserFactory parserFactory,
            IFileService fileService,
            IReportGeneratorFactory reportGeneratorFactory)
        {
            _reportModelCreator = reportModelCreator;
            _parserFactory = parserFactory;
            _fileService = fileService;
            _reportGeneratorFactory = reportGeneratorFactory;
        }

        public void GenerateReport(IDictionary<string, string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (!arguments.TryGetValue(Constants.ParserArgumentName, out string parserName))
            {
                throw new ArgumentException($"Argument not set: {Constants.ParserArgumentName}");
            }

            PeriodType periodType = PeriodType.Monthly;
            arguments.TryGetValue(Constants.PeriodTypeArgumentName, out string periodTypeName);
            if (!string.IsNullOrWhiteSpace(periodTypeName))
            {
                periodType = GetPeriodType(periodTypeName);
            }

            double beginAmount = 0;
            arguments.TryGetValue(Constants.BeginAmountArgumentName, out string beginAmountText);
            if (!string.IsNullOrWhiteSpace(beginAmountText))
            {
                beginAmount = double.Parse(beginAmountText.Replace(",", "."), CultureInfo.InvariantCulture);
            }

            var parser = _parserFactory.GetParser(parserName);
            if (parser == null)
            {
                throw new InvalidOperationException($"Bank parser with name '{parserName}' not found.");
            }

            var mutations = parser.ParseMutations(arguments);
            var reportModels = _reportModelCreator.GetReportModels(mutations, periodType, beginAmount);

            arguments.TryGetValue(Constants.ReportGeneratorArgumentName, out string generatorName);
            if (string.IsNullOrWhiteSpace(generatorName))
            {
                generatorName = "excel";
            }

            var reportGenerator = _reportGeneratorFactory.GetReportGenerator(generatorName);
            if (reportGenerator == null)
            {
                throw new InvalidOperationException($"Report generator with name '{generatorName}' not found.");
            }

            arguments.TryGetValue(Constants.OutputDirectoryArgumentName, out string outputPath);
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                arguments.TryGetValue(Constants.PathArgumentName, out outputPath);
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new InvalidOperationException($"Please set the '{Constants.PathArgumentName}' or '{Constants.OutputDirectoryArgumentName}'.");
            }

            foreach (var reportModel in reportModels)
            {
                var generatedReport = reportGenerator.GenerateReport(reportModel);
                string filename = $"report_{reportModel.BeginDateTime:yyyyMMdd}_{reportModel.EndDateTime:yyyyMMdd}.{reportGenerator.Extension}";
                string fullPath = Path.Combine(outputPath, filename);
                _fileService.WriteAllBytes(fullPath, generatedReport);
            }
        }

        private static PeriodType GetPeriodType(string input)
        {
            switch (input)
            {
                case "daily":
                    return PeriodType.Daily;
                case "weekly":
                    return PeriodType.Weekly;
                case "monthly":
                    return PeriodType.Monthly;
                case "yearly":
                    return PeriodType.Yearly;
                case "everything":
                    return PeriodType.Everything;
                default:
                    throw new InvalidOperationException($"No period type found with name '{input}'.");
            }
        }
    }
}
