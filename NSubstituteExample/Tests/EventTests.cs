using System;
using System.ComponentModel;
using NSubstitute;
using NSubstituteExample.Classes;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/raising-events/
    /// </summary>
    public class EventTests
    {
        private IEngine engine;
        private INotifyPropertyChanged notify;

        [SetUp]
        public void Setup()
        {
            engine = Substitute.For<IEngine>();
            notify = Substitute.For<INotifyPropertyChanged>();
        }

        [Test]
        public void EventWasCalled()
        {
            // when event handled set flag to true
            var wasCalled = false;
            engine.Idling += (sender, args) => wasCalled = true;

            // any old sender, and any old args
            engine.Idling += Raise.EventWith(new object(), new EventArgs());

            // assert our subtitute event handler was hit/fired
            Assert.IsTrue(wasCalled);
        }

        [Test]
        public void EventWasCalledShorthand()
        {
            var wasCalled = false;
            engine.Idling += (sender, args) => wasCalled = true;

            // as we don't care about the sender or args, skip 'em
            engine.Idling += Raise.Event();

            Assert.True(wasCalled);
        }

        [Test]
        public void EventRaisedWhenArgumentHasNoDefaultConstructor()
        {
            // set our substitute event handler to keep count of events raised
            var numberOfEvents = 0;
            engine.LowFuelWarning += (sender, args) => numberOfEvents++;

            // we don't care about the sender, so skip it
            engine.LowFuelWarning += Raise.EventWith(new LowFuelWarningEventArgs(10));

            // now we do care about the sender so use one, albeit any old object for the purposes of this test!
            engine.LowFuelWarning += Raise.EventWith(new object(), new LowFuelWarningEventArgs(10));

            // Now we can assert the number of events raised matches the count from our substitute handler
            Assert.AreEqual(2, numberOfEvents);
        }

        [Test]
        public void RaiseDelegateEvent()
        {
            bool wasCalled = false;
            notify.PropertyChanged += (sender, args) => wasCalled = true;

            // raise event with a delegate that DOES NOT inherit from EventHandler or EventHandler<T>
            notify.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs("test"));

            Assert.That(wasCalled);
        }

        [Test]
        public void RaiseActionEvent()
        {
            // action is another form of delegate event, so when raised set our temp var to the value raised
            int revvedAt = 0;
            engine.RevvedAt += rpm => revvedAt = rpm;

            // raise our action
            engine.RevvedAt += Raise.Event<Action<int>>(123);

            // and now assert that we have expected value in our temp var
            Assert.AreEqual(123, revvedAt);
        }

        [TearDown]
        public void TearDown()
        {
            engine = null;
        }    
    }
}
