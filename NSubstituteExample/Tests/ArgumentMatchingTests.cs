using System.Collections;
using System.Linq;
using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;
using IFormatter = NSubstituteExample.Interfaces.IFormatter;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/argument-matchers/
    /// </summary>
    public class ArgumentMatchingTests
    {
        private ICalculator calculator;
        private IFormatter formatter;

        [SetUp]
        public void Setup()
        {
            calculator = Substitute.For<ICalculator>();
            formatter = Substitute.For<IFormatter>();
        }

        [Test]
        public void IgnoreArgument()
        {
            calculator.Add(Arg.Any<int>(), 5).Returns(7); // Add with any value for 1st arg and 5 for 2nd arg always returns 7

            Assert.AreEqual(7, calculator.Add(42, 5));
            Assert.AreEqual(7, calculator.Add(123, 5));
            Assert.AreNotEqual(7, calculator.Add(1, 7)); // Not Equal as the 2nd argument here is not 5
        }

        [Test]
        public void MatchAnyArgumentValueByType()
        {
            formatter.Format(new object());
            formatter.Format("some string");

            formatter.Received().Format(Arg.Any<object>()); // Arg.Any FTW
            formatter.Received().Format(Arg.Any<string>());
            formatter.DidNotReceive().Format(Arg.Any<int>()); // Have not called this overload (int) in this test, so didn't receive, nice
        }

        [Test]
        public void ConditionallyMatch()
        {
            calculator.Add(1, -10);

            // call with first arg 1 and second arg less than 0:
            calculator.Received().Add(1, Arg.Is<int>(x => x < 0));

            // call with first arg 1 and second arg of -2, -5, or -10:
            calculator
                .Received()
                .Add(1, Arg.Is<int>(x => new[] { -2, -5, -10 }.Contains(x)));

            // no call with first arg greater than 10:
            calculator.DidNotReceive().Add(Arg.Is<int>(x => x > 10), -10);

            // another example
            formatter.Format(Arg.Is<string>(x => x.Length <= 10)).Returns("matched");
            Assert.AreEqual("matched", formatter.Format("short"));
            Assert.AreNotEqual("matched", formatter.Format("not matched, too long"));
            
            // trying to access .Length on null will throw an exception NSubstitute will assume it does not match and swallow the exception
            Assert.AreNotEqual("matched", formatter.Format(null));
        }

        [Test]
        public void MatchSpecificArgument()
        {
            calculator.Add(0, 42);

            // This won't work; NSubstitute isn't sure which arg the matcher applies to:
            // calculator.Received().Add(0, Arg.Any<int>());

            // so use Arg.Is for the first and then we can use any for the second, if args were of different types we'd be ok
            calculator.Received().Add(Arg.Is(0), Arg.Any<int>());
        }

        [TearDown]
        public void TearDown()
        {
            // break it, break it down
            calculator = null;
            formatter = null;
        }
    }
}
