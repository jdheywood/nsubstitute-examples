namespace NSubstituteExample.Interfaces
{
    public interface IFormatter
    {
        string Format(object toFormat);

        string Format(string toFormat);

        string Format(int toFormat);
    }
}
