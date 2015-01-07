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
        private CommandRepeater repeater;

        [SetUp]
        public void Setup()
        {
            // Arrange, arguably cleaner/tidier to do this in the test that uses these, but I like to use my setup!
            command = Substitute.For<ICommand>();
            something = new SomethingThatNeedsACommand(command);
            repeater = new CommandRepeater(command, 3);
        }

        [Test]
        public void ShouldExecuteCommandTest()
        {
            //Act
            something.DoSomething();
            //Assert
            command.Received().Execute();
        }

        [Test]
        public void ShouldNotExecuteCommandTest()
        {
            //Act
            something.DontDoAnything();
            //Assert
            command.DidNotReceive().Execute(); // This must be in seperate test to the Received() above, as they will interfere with each other if not and fail!
        }

        [Test]
        public void ShouldExecuteSpecifiedNumberOfTimes()
        {
            repeater.Execute(); // Instance of our command repeater
            command.Received(3).Execute(); // Ensure our substitute command was executed thrice!
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
