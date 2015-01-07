using System;

namespace NSubstituteExample.Classes
{
    public class Class1 : IDisposable
    {
        public int Counter { get; set; }

        public string Message { get; set; }

        public Class1(int counter, string message)
        {
            Counter = counter;
            Message = message;
        }

        public virtual string DoSomething(int input)
        {
            switch (input)
            {
                case 1:
                    return "Case 1";
                case 2:
                    return "Case 2";
                default:
                    return String.Format("Case {0}", input.ToString());
            }
        }

        public void Dispose()
        {
            Counter = 0;
            Message = string.Empty;
        }
    }
}
