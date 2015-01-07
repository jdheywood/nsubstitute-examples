using System;

namespace NSubstituteExample.Classes
{
    public class LowFuelWarningEventArgs : EventArgs
    {
        public int PercentLeft { get; private set; }

        public LowFuelWarningEventArgs(int percentLeft)
        {
            PercentLeft = percentLeft;
        }
    }
}
