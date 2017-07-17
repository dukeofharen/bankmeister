using Bankmeister.Business;
using Bankmeister.Helpers;
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

            var argumentsDictionary = args.Parse();

            // TODO Test code, can be removed lateron
            // TODO Args parser
            // TODO Unit tests
            // TODO in-memory integration test
            // TODO StyleCop check
            // TODO ReSharper check
            // TODO CodeMaid cleanup
            // TODO Exception handling and logging
            // TODO Help page for command line
            var generator = provider.GetService<IReportManager>();
            generator.GenerateReport(argumentsDictionary);
        }
    }
}