using System;
using NSubstituteExample.Classes;

namespace NSubstituteExample.Interfaces
{
    public interface IEngine
    {
        event EventHandler Idling;

        event EventHandler<LowFuelWarningEventArgs> LowFuelWarning;

        event Action<int> RevvedAt;
    }
}
