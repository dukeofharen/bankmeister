using System.Collections.Generic;
using Bankmeister.Models;

namespace Bankmeister.Business
{
    public interface IParser
    {
        string Key { get; }

        IEnumerable<MutationModel> ParseMutations(IDictionary<string, string> arguments);
    }
}
