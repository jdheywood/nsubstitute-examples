using System;
using System.Windows.Input;
using NSubstitute;
using NSubstituteExample.Classes;
using NSubstituteExample.Interfaces;
using NUnit.Framework;
using ICommand = NSubstituteExample.Interfaces.ICommand;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/creating-a-substitute/
    /// </summary>
    public class CreatingSubstituteTests
    {
        private Class1 class1;

        [SetUp]
        public void Setup()
        {
            class1 = Substitute.For<Class1>(-1, "Hello World");
        }

        [Test]
        public void DoSomethingTest()
        {
            // Class1.DoSomething(int input) must be virtual in order to configure return values on our substitute
            class1.DoSomething(77).Returns("Case 77");
            Assert.That(class1.DoSomething(77), Is.EqualTo("Case 77"));
            class1.Received().DoSomething(77);
            class1.DidNotReceive().DoSomething(99);
        }

        [Test]
        public void MultipleInterfaceSubstituteTest()
        {
            // We can implement multiple interfaces, BUT, only one class per substitute, as below
            var substitute = Substitute.For(
                new[] {typeof (ICommand), typeof (ICalculator), typeof (Class1)},
                new object[] {-1, "Hello World"}
                );

            Assert.IsInstanceOf<ICommand>(substitute);
            Assert.IsInstanceOf<ICalculator>(substitute);
            Assert.IsInstanceOf<Class1>(substitute);
        }

        [Test]
        public void DelegateSubstituteTest()
        {
            var func = Substitute.For<Func<string>>();

            func().Returns("hello");
            Assert.AreEqual("hello", func());
        }

        [TearDown]
        public void Teardown()
        {
            // break it, break it down, then you are free to go
            class1.Dispose();
        }
    }
}
