using Bankmeister.Business.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Business
{
    public class DependencyRegistration
    {
        public static void RegisterDependencies(IServiceCollection collection)
        {
            collection.AddTransient<IngParser>();

            Services.DependencyRegistration.RegisterDependencies(collection);
        }
    }
}
