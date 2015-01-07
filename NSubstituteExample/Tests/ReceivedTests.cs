using System;
using System.Collections.Generic;
using NSubstitute;
using NSubstituteExample.Classes;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: 
    /// http://nsubstitute.github.io/help/received-calls/
    /// http://nsubstitute.github.io/help/clear-received-calls/
    /// </summary>
    public class ReceivedTests
    {
        private ICommand command;
        private SomethingThatNeedsACommand something;
        private CommandRepeater repeater;
        private ICalculator calculator;
        private IDictionary<string, int> dictionary;
        private CommandWatcher watcher;
        private OnceOffCommandRunner runner;

        [SetUp]
        public void Setup()
        {
            // Arrange, arguably cleaner/tidier to do this in the test(s) that uses these, but I like to use my setup, plus it saves repetition which is good
            command = Substitute.For<ICommand>();
            something = new SomethingThatNeedsACommand(command);
            repeater = new CommandRepeater(command, 3);
            calculator = Substitute.For<ICalculator>();
            dictionary = Substitute.For<IDictionary<string, int>>();
            watcher = new CommandWatcher(command);
            runner = new OnceOffCommandRunner(command);
        }

        [Test]
        public void ShouldExecuteCommand()
        {
            //Act
            something.DoSomething();
            //Assert
            command.Received().Execute(); // received AT LEAST ONCE - and possibly N times!
            command.Received(1).Execute(); // received ONCE ONLY - important distinction!
        }

        [Test]
        public void ShouldNotExecuteCommand()
        {
            //Act
            something.DontDoAnything();
            //Assert
            command.DidNotReceive().Execute(); // This must be in seperate test to the Received() above, as they will interfere with each other if not and fail!
            command.Received(0).Execute(); // Received(0) same as DidNotReceive() - DidNotReceive() is clearer to understand for me
        }

        [Test]
        public void ShouldExecuteSpecifiedNumberOfTimes()
        {
            repeater.Execute(); // Instance of our command repeater
            command.Received(3).Execute(); // Ensure our substitute command was executed thrice!
        }

        [Test]
        public void CheckWithArgumentMatchers()
        {
            calculator.Add(1, 2);
            calculator.Add(-100, 100);

            // received with second arg of 2 and any first arg:
            calculator.Received().Add(Arg.Any<int>(), 2);
            
            // received with first arg less than 0, and second arg of 100:
            calculator.Received().Add(Arg.Is<int>(x => x < 0), 100);
            
            // did not receive a call where second arg is >= 500 and any first arg:
            calculator
                .DidNotReceive()
                .Add(Arg.Any<int>(), Arg.Is<int>(x => x >= 500));
        }

        [Test]
        public void IgnoringArguments()
        {
            calculator.Add(1, 3);

            calculator.ReceivedWithAnyArgs().Add(10, 30);
            calculator.DidNotReceiveWithAnyArgs().Subtract(0, 0);
        }

        [Test]
        public void CheckCallToProperty()
        {
            // this is checking that the property getter/setter are called, limited usefulness but could come in handy
            var mode = calculator.Mode;
            Assert.IsInstanceOf(typeof(String), mode); // Keep stylecop happy
            calculator.Mode = "TEST";

            // compiler forces us to assign this to a variable 
            var temp = calculator.Received().Mode;
            Assert.IsNull(temp); // Keep stylecop happy

            calculator.Received().Mode = "TEST"; // Our actual check on the property
        }

        [Test]
        public void CheckCallToIndexer()
        {
            // Indexers are just another property, so the same checks can be made as in previous test
            dictionary["test"] = 1;

            dictionary.Received()["test"] = 1;
            dictionary.Received()["test"] = Arg.Is<int>(x => x < 5);
        }

        [Test]
        public void CheckEventSubscriberActs()
        {
            // Raise event on substitute and assert subscribing class performs expected action
            command.Executed += Raise.Event();

            Assert.That(watcher.DidStuff);
        }

        [Test]
        public void CheckEventSubscriberSubscribes()
        {
            // Not recommended. Favour testing behaviour over implementation specifics.
            
            // check subscription
            command.Received().Executed += watcher.OnExecuted;
            
            // Or, if the handler is not accessible, use Any argument match, cheeky. This could potentially be useful even if not recommended
            command.Received().Executed += Arg.Any<EventHandler>();
        }

        [Test]
        public void ClearingReceivedCalls()
        {
            runner.Run(); // Run sets the command to null, so this will only ever run once, unless we re-instantiate the object
            command.Received().Execute();

            // clear previous calls to command
            command.ClearReceivedCalls();

            runner.Run(); // command is null at this point so runner will not execute, hance the DidNotReceive passes below
            command.DidNotReceive().Execute();
        }

        [TearDown]
        public void TearDown()
        {
            // break it, break it down
            command = null;
            something = null;
            repeater = null;
            calculator = null;
            dictionary = null;
            watcher = null;
            runner = null;
        }
    }
}
