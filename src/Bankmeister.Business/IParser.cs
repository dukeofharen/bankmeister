using System.Collections.Generic;
using Bankmeister.Models;

namespace Bankmeister.Business
{
    public interface IParser
    {
        IEnumerable<MutationModel> ParseMutations(IDictionary<string, string> arguments);
    }
}
