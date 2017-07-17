namespace Bankmeister.Business
{
    public interface IParserFactory
    {
        IParser GetParser(string parser);
    }
}
