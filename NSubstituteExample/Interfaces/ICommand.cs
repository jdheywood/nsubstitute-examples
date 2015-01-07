using System;

namespace NSubstituteExample.Interfaces
{
    public interface ICommand
    {
        void Execute();
        event EventHandler Executed;
    }
}
