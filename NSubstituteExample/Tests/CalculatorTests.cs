using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// See: http://nsubstitute.github.io/help/getting-started/
    /// </summary>
    public class CalculatorTests
    {
        private ICalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = Substitute.For<ICalculator>();
        }

        [Test]
        public void FirstUnitTest()
        {
            // just gimme a substitute calculator, with an add method please, ta   
            calculator.Add(1, 2).Returns(3);
            Assert.That(calculator.Add(1, 2), Is.EqualTo(3));
        }

        [Test]
        public void SecondUnitTest()
        {
            // check our sub received a call
            calculator.Add(1, 2);
            calculator.Received().Add(1, 2);
            calculator.DidNotReceive().Add(5, 7);

            // calculator.Received().Add(1, 3);
            // Above gives some useful exception output about the call and param in error, nice
        }

        [Test]
        public void ThirdUnitTest()
        {
            calculator.Mode.Returns("DEC");
            Assert.AreEqual(calculator.Mode, "DEC");

            calculator.Mode.Returns("HEX");
            Assert.AreEqual(calculator.Mode, "HEX");
        }

        [Test]
        public void FourthUnitTest()
        {
            calculator.Add(10, -5);
            calculator.Received().Add(10, Arg.Any<int>());
            calculator.Received().Add(10, Arg.Is<int>(x => x < 0));

            // this is a bit nasty if you ask me, anonymous function passed to sub
            calculator
                .Add(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => (int) x[0] + (int) x[1]);
            
            Assert.That(calculator.Add(5, 10), Is.EqualTo(15));
            Assert.AreEqual(calculator.Add(2, 4), 6);
            Assert.AreNotEqual(calculator.Add(1, 1), 3);
        }

        [Test]
        public void FifthUnitTest()
        {
            // chain/sequence return values and test (in correct order, obvs!)
            calculator.Mode.Returns("HEX", "DEC", "BIN");
            Assert.That(calculator.Mode, Is.EqualTo("HEX"));
            Assert.That(calculator.Mode, Is.EqualTo("DEC"));
            Assert.That(calculator.Mode, Is.EqualTo("BIN"));
        }

        [Test]
        public void SixthUnitTest()
        {
            bool eventWasRaised = false;
            calculator.PoweringUp += (sender, args) => eventWasRaised = true;

            calculator.PoweringUp += Raise.Event();
            Assert.That(eventWasRaised);
        }

        [TearDown]
        public void TearDown()
        {
            // break it, break it down
            calculator = null;
        }
    }
}
