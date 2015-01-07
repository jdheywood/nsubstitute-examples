using NSubstituteExample.Interfaces;

namespace NSubstituteExample.Classes
{
    public class OnceOffCommandRunner
    {
        ICommand command;

        public OnceOffCommandRunner(ICommand command)
        {
            this.command = command;
        }

        public void Run()
        {
            if (command == null)
            {
                return;
            }

            command.Execute();
            
            command = null; // once run, get rid of the command to prevent re-running
        }
    }
}
