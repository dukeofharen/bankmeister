using System;
using System.Collections.Generic;

namespace Bankmeister.Models
{
    public class ReportModel
    {
        public DateTime BeginDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public double TotalUp { get; set; }

        public double TotalDown { get; set; }

        public double StartAmount { get; set; }

        public double EndAmount { get; set; }

        public IEnumerable<NameAmountModel> NameAmountGroups { get; set; }

        public IEnumerable<MutationModel> Mutations { get; set; }

        public IEnumerable<AmountFrequencyModel> AmountFrequencies { get; set; }
    }
}
