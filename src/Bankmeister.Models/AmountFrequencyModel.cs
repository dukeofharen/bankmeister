namespace Bankmeister.Models
{
    public class AmountFrequencyModel
    {
        public double FromAmount { get; set; }

        public double? ToAmount { get; set; }

        public int FrequencyDown { get; set; }

        public int FrequencyUp { get; set; }

    }
}
