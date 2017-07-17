using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Business.Implementations
{
    internal class ParserFactory : IParserFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ParserFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IParser GetParser(string parser)
        {
            var service = _serviceProvider
                .GetServices<IParser>()
                .FirstOrDefault(p => p.Key == parser);
            return service;
        }
    }
}
