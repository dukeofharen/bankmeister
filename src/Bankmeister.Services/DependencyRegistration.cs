using Bankmeister.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Bankmeister.Services
{
    public static class DependencyRegistration
    {
        public static void RegisterDependencies(IServiceCollection collection)
        {
            collection.AddTransient<ICsvService, CsvService>();
            collection.AddTransient<IFileService, FileService>();
        }
    }
}
