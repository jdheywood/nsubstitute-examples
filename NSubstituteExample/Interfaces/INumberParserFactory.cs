namespace NSubstituteExample.Interfaces
{
    public interface INumberParserFactory
    {
        INumberParser Create(char delimiter);
    }
}
