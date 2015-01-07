using System;
using NSubstituteExample.Interfaces;

namespace NSubstituteExample.Classes
{
    public class CommandWatcher
    {
        public CommandWatcher(ICommand command)
        {
            command.Executed += OnExecuted;
        }
        
        public bool DidStuff { get; private set; }

        public void OnExecuted(object o, EventArgs e)
        {
            DidStuff = true;
        }
    } 
}
