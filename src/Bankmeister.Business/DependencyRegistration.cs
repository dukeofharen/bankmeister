using Bankmeister.Business.Implementations;
using Bankmeister.Business.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Business
{
    public class DependencyRegistration
    {
        public static void RegisterDependencies(IServiceCollection collection)
        {
            collection.AddTransient<IngParser>();
            collection.AddTransient<IDateTimeLogic, DateTimeLogic>();
            collection.AddTransient<IReportModelCreator, ReportModelCreator>();

            Services.DependencyRegistration.RegisterDependencies(collection);
        }
    }
}
