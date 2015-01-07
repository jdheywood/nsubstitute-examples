namespace NSubstituteExample.Interfaces
{
    public interface ILookup
    {
        bool TryLookup(string key, out string value);
    }
}
