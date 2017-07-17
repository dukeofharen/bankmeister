using Bankmeister.Business.Implementations;
using Bankmeister.Business.Parsers;
using Bankmeister.Business.ReportGenerators;
using Bankmeister.Business.ReportGenerators.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Business
{
    public class DependencyRegistration
    {
        public static void RegisterDependencies(IServiceCollection collection)
        {
            collection.AddTransient<IDateTimeLogic, DateTimeLogic>();
            collection.AddTransient<IParserFactory, ParserFactory>();
            collection.AddTransient<IReportManager, ReportManager>();
            collection.AddTransient<IReportGeneratorFactory, ReportGeneratorFactory>();
            collection.AddTransient<IReportModelCreator, ReportModelCreator>();

            // Parsers
            collection.AddTransient<IParser, IngParser>();

            // Generators
            collection.AddTransient<IReportGenerator, ExcelReportGenerator>();

            Services.DependencyRegistration.RegisterDependencies(collection);
        }
    }
}
