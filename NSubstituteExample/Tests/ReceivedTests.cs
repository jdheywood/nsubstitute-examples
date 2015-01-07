using NSubstitute;
using NSubstituteExample.Classes;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    public class ReceivedTests
    {
        private ICommand command;
        private SomethingThatNeedsACommand something;

        [SetUp]
        public void Setup()
        {
            // Arrange, arguably cleaner/tidier to do this in the test that uses these, but I like to use my setup!
            command = Substitute.For<ICommand>();
            something = new SomethingThatNeedsACommand(command);
        }

        [Test]
        public void ShouldExecuteCommandTest()
        {
            //Act
            something.DoSomething();

            //Assert
            command.Received().Execute();
        }

        [TearDown]
        public void TearDown()
        {
            // break it, break it down
            command = null;
            something = null;
        }
    }
}
