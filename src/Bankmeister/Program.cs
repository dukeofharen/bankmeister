﻿using System.Collections.Generic;
using Bankmeister.Business.Parsers;
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

            // Test code, can be removed lateron
            var arguments = new Dictionary<string, string> { { "path", @"C:\Users\Duco\Desktop\ing" } };
            var parser = provider.GetService<IngParser>();
            var mutations = parser.ParseMutations(arguments);
        }
    }
}