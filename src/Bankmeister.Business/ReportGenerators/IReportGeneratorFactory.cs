namespace Bankmeister.Business.ReportGenerators
{
    public interface IReportGeneratorFactory
    {
        IReportGenerator GetReportGenerator(string name);
    }
}
