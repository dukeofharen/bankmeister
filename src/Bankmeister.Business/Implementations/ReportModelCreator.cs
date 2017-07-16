using System.Collections.Generic;
using System.Linq;
using Bankmeister.Models;
using Bankmeister.Models.Enums;

namespace Bankmeister.Business.Implementations
{
    public class ReportModelCreator : IReportModelCreator
    {
        private readonly IDateTimeLogic _reportLogic;

        public ReportModelCreator(IDateTimeLogic reportLogic)
        {
            _reportLogic = reportLogic;
        }

        public IEnumerable<ReportModel> GetReportModels(IEnumerable<MutationModel> mutations, PeriodType periodType, double beginAmount = 0)
        {
            var mutationsArray = mutations.ToArray();
            var result = new List<ReportModel>();

            var minDate = mutationsArray.Min(m => m.DateTime);
            var maxDate = mutationsArray.Max(m => m.DateTime);
            var dateTimeRanges = _reportLogic.CalculateDateTimeRanges(periodType, minDate, maxDate);
            double startAmountPointer = beginAmount;

            foreach (var range in dateTimeRanges)
            {
                var filteredMutations = mutationsArray
                    .Where(m => m.DateTime >= range.From && m.DateTime <= range.To)
                    .ToArray();
                double totalUp = filteredMutations
                    .Where(m => m.Amount >= 0)
                    .Sum(m => m.Amount);
                double totalDown = filteredMutations
                    .Where(m => m.Amount < 0)
                    .Sum(m => m.Amount) * -1;
                double endAmount = startAmountPointer + filteredMutations
                    .Sum(m => m.Amount);

                result.Add(new ReportModel
                {
                    BeginDateTime = range.From,
                    EndDateTime = range.To,
                    Mutations = filteredMutations,
                    TotalUp = totalUp,
                    TotalDown = totalDown,
                    StartAmount = startAmountPointer,
                    EndAmount = endAmount,
                    AmountFrequencies = GetAmountFrequencies(filteredMutations),
                    NameAmountGroups = GetNameAmountModels(filteredMutations)
                });

                startAmountPointer = endAmount;
            }

            return result;
        }

        private static IEnumerable<AmountFrequencyModel> GetAmountFrequencies(MutationModel[] mutations)
        {
            var result = new List<AmountFrequencyModel>
            {
                GetAmountFrequency(mutations, 0, 1),
                GetAmountFrequency(mutations, 1, 5),
                GetAmountFrequency(mutations, 5, 20),
                GetAmountFrequency(mutations, 20, 50),
                GetAmountFrequency(mutations, 50, 100),
                GetAmountFrequency(mutations, 100, 200),
                GetAmountFrequency(mutations, 200, 1000),
                GetAmountFrequency(mutations, 1000, 2000),
                GetAmountFrequency(mutations, 2000)
            };

            return result;
        }

        private static AmountFrequencyModel GetAmountFrequency(MutationModel[] mutations, int from, int? to = null)
        {
            return new AmountFrequencyModel
            {
                FromAmount = from,
                ToAmount = to,
                FrequencyDown = mutations.Count(m => -m.Amount >= from && (to == null || -m.Amount < to)),
                FrequencyUp = mutations.Count(m => m.Amount >= from && (to == null || m.Amount < to))
            };
            
        }

        private static IEnumerable<NameAmountModel> GetNameAmountModels(MutationModel[] mutations)
        {
            var groups = mutations
                .GroupBy(m => m.Name);
            return groups
                .Select(g => new NameAmountModel
                {
                    Amount = g.Sum(m => m.Amount),
                    Name = g.Key
                })
                .OrderByDescending(m => m.Amount)
                .ToArray();
        }
    }
}
