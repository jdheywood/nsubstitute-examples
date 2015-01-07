using System;
using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    public class ExceptionTests
    {
        private ICalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = Substitute.For<ICalculator>();
        }

        [Test]
        public void ThrowException()
        {
            // non-voids
            calculator.Add(-1, -1).Returns(x => { throw new Exception(); });

            // voids and non-voids
            calculator
                .When(x => x.Add(-2, -2))
                .Do(x => { throw new Exception(); });

            // both calls will throw an exception
            Assert.Throws<Exception>(() => calculator.Add(-1, -1));
            Assert.Throws<Exception>(() => calculator.Add(-2, -2));
        }

        [TearDown]
        public void TearDown()
        {
            calculator = null;
        }
    }
}
