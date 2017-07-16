using System;

namespace Bankmeister.Models
{
    public class MutationModel
    {
        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public string FromAccount { get; set; }

        public string ToAccount { get; set; }

        public double Amount { get; set; }

        public string MutationType { get; set; }

        public string Description { get; set; }
    }
}
