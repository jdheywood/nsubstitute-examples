using NSubstituteExample.Interfaces;

namespace NSubstituteExample.Classes
{
    public class CommandRepeater
    {
        readonly ICommand command;
        
        readonly int numberOfTimesToCall;

        public CommandRepeater(ICommand command, int numberOfTimesToCall)
        {
            this.command = command;
            this.numberOfTimesToCall = numberOfTimesToCall;
        }

        public void Execute()
        {
            for (var i = 0; i < numberOfTimesToCall; i++) command.Execute();
        }
    }
}
