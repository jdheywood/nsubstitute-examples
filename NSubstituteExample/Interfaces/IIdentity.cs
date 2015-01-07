namespace NSubstituteExample.Interfaces
{
    public interface IIdentity
    {
        string Name { get; }

        string[] Roles();
    }
}
