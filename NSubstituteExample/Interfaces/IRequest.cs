namespace NSubstituteExample.Interfaces
{
    public interface IRequest
    {
        IIdentity Identity { get; }

        IIdentity NewIdentity(string name);
    }
}
