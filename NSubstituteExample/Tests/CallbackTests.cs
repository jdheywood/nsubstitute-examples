using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/callbacks/
    /// </summary>
    public class CallbackTests
    {
        private ICalculator calculator;
        private IFoo foo;

        [SetUp]
        public void Setup()
        {
            calculator = Substitute.For<ICalculator>();
            foo = Substitute.For<IFoo>();
        }

        [Test]
        public void Callback()
        {
            var counter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x => 0)
                .AndDoes(x => counter++);

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            calculator.Add(11, -3);
            Assert.AreEqual(counter, 3);
        }

        [Test]
        public void CallbackForVoid()
        {
            var counter = 0;
            foo.When(x => x.SayHello("World"))
                .Do(x => counter++); // we can't call Returns on a void method, but we can do this instead, nice

            foo.SayHello("World");
            foo.SayHello("World");
            Assert.AreEqual(2, counter);
        }

        [Test]
        public void WhenDoForNonVoid()
        {
            // we can also use Wnen...Do for non-void methods if we want, although Returns...AndDoes is arguably clearer/more explicit...
            var counter = 0;
            calculator.Add(1, 2).Returns(3);
            calculator
                .When(x => x.Add(Arg.Any<int>(), Arg.Any<int>()))
                .Do(x => counter++);

            var result = calculator.Add(1, 2);
            Assert.AreEqual(3, result);
            Assert.AreEqual(1, counter);
        }
        
        [TearDown]
        public void TearDown()
        {
            calculator = null;
        }
    }
}
