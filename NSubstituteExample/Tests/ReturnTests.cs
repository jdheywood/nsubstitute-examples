using System;
using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see:
    /// http://nsubstitute.github.io/help/set-return-value/
    /// http://nsubstitute.github.io/help/return-for-args/
    /// http://nsubstitute.github.io/help/return-for-any-args/
    /// http://nsubstitute.github.io/help/return-from-function/
    /// http://nsubstitute.github.io/help/multiple-returns/
    /// http://nsubstitute.github.io/help/replacing-return-values/
    /// </summary>
    public class ReturnTests
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
        public void SpecificArgsTest()
        {
            // when first arg is anything and second arg is 5:
            calculator.Add(Arg.Any<int>(), 5).Returns(10);
            
            Assert.AreEqual(10, calculator.Add(123, 5));
            Assert.AreEqual(10, calculator.Add(-9, 5));
            Assert.AreNotEqual(10, calculator.Add(-9, -9));

            // when first arg is 1 and second arg less than 0:
            calculator.Add(1, Arg.Is<int>(x => x < 0)).Returns(345);
            
            Assert.AreEqual(345, calculator.Add(1, -2));
            Assert.AreNotEqual(345, calculator.Add(1, 2));

            // when both args equal to 0:
            calculator.Add(Arg.Is(0), Arg.Is(0)).Returns(99);
            
            Assert.AreEqual(99, calculator.Add(0, 0));
        }

        [Test]
        public void AnyArgsTest()
        {
            // ignore input and return same value regardless!
            calculator.Add(1, 2).ReturnsForAnyArgs(100);

            Assert.AreEqual(calculator.Add(1, 2), 100);
            Assert.AreEqual(calculator.Add(-7, 15), 100);
        }

        [Test]
        public void FunctionTest()
        {
            calculator
                .Add(Arg.Any<int>(), Arg.Any<int>())
                .Returns(x => (int) x[0] + (int) x[1]); // Access arguments of func using indexer

            // Numerous ways to assert equality
            Assert.AreEqual(calculator.Add(1, 1), 2);
            Assert.That(calculator.Add(20, 30), Is.EqualTo(50));
            Assert.True(calculator.Add(-2, 12) == 10);

            // Access arguments of func using convenience method
            foo.Bar(0, "").ReturnsForAnyArgs(x => "Hello " + x.Arg<string>()); // x.Arg<string>() replaces x[1] - only works with one arg of a given type though as it can't work out if you have 2 string args which you want!
            Assert.That(foo.Bar(1, "World"), Is.EqualTo("Hello World"));

            // Callback whenever a call is made, trivial example incrementing counter
            var counter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x =>
                {
                    counter++;
                    return 0;
                });

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            calculator.Add(11, -3);
            Assert.AreEqual(counter, 3); // Show we called thricely!

            // Alternative syntax using .AndDoes - nicer
            var altCounter = 0;
            calculator
                .Add(0, 0)
                .ReturnsForAnyArgs(x => 0)
                .AndDoes(x => altCounter++); // our callback

            calculator.Add(7, 3);
            calculator.Add(2, 2);
            Assert.AreEqual(altCounter, 2);
        }

        [Test]
        public void MultipleValuesTest()
        {
            // assert in correct order!
            calculator.Mode.Returns("DEC", "HEX", "BIN");
            Assert.AreEqual("DEC", calculator.Mode);
            Assert.AreEqual("HEX", calculator.Mode);
            Assert.AreEqual("BIN", calculator.Mode);

            // as above, but with func in return sequence so we can do something with this, or use a callback 
            var result = String.Empty;
            calculator.Mode.Returns(x => "DEC", x => "HEX", x => { throw new Exception(); });
            Assert.AreEqual("DEC", calculator.Mode);
            Assert.AreEqual("HEX", calculator.Mode);
            Assert.Throws<Exception>(() => { result = calculator.Mode; });
            Assert.IsInstanceOf(typeof(String), result);
        }

        [Test]
        public void ReplacingValuesTest()
        {
            // return value can be set to whatever, only the most recent will be used so be careful when and what you assert
            calculator.Mode.Returns("DEC,HEX,OCT");
            calculator.Mode.Returns(x => "???");
            calculator.Mode.Returns("HEX");
            calculator.Mode.Returns("BIN");

            Assert.AreEqual(calculator.Mode, "BIN"); // last set return value
        }

        [TearDown]
        public void TearDown()
        {
            // break it, break it down
            calculator = null;
            foo = null;
        }
    }
}
