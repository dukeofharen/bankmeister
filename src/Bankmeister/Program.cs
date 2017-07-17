using System.Collections.Generic;
using Bankmeister.Business;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            Business.DependencyRegistration.RegisterDependencies(serviceCollection);
            var provider = serviceCollection.BuildServiceProvider();

            // TODO Test code, can be removed lateron
            // TODO Args parser
            // TODO Unit tests
            // TODO in-memory integration test
            // TODO HTML report generator
            // TODO PDF report generator
            // TODO StyleCop check
            // TODO ReSharper check
            // TODO CodeMaid cleanup
            var arguments = new Dictionary<string, string>
            {
                { "path", @"C:\Users\Duco\Desktop\ing" },
                { "bank", "ing" },
                { "periodType", "yearly" },
                { "beginAmount", "0" },
                { "generator", "excel" },
                { "outputDirectory", @"C:\Users\Duco\Desktop\ing\output" }
            };
            var generator = provider.GetService<IReportManager>();
            generator.GenerateReport(arguments);
        }
    }
}