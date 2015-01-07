using System.Collections.Generic;

namespace NSubstituteExample.Interfaces
{
    public interface INumberParser
    {
        IEnumerable<int> Parse(string expression);
    }
}
