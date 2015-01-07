namespace NSubstituteExample.Interfaces
{
    public interface IContext
    {
        IRequest CurrentRequest { get; }
    }
}
