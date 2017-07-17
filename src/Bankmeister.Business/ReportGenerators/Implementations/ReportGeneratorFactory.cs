using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Business.ReportGenerators.Implementations
{
    internal class ReportGeneratorFactory : IReportGeneratorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportGeneratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReportGenerator GetReportGenerator(string name)
        {
            var service = _serviceProvider
                .GetServices<IReportGenerator>()
                .FirstOrDefault(p => p.Key == name);
            return service;
        }
    }
}
