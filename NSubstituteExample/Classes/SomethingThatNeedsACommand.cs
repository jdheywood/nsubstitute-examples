using NSubstituteExample.Interfaces;

namespace NSubstituteExample.Classes
{
    public class SomethingThatNeedsACommand
    {
        private readonly ICommand command;

        public SomethingThatNeedsACommand(ICommand command)
        {
            this.command = command;
        }

        public void DoSomething() { command.Execute(); }
        
        public void DontDoAnything() { }
    }
}
