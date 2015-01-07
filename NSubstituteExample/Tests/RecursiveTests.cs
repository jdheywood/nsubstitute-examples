using System;
using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/auto-and-recursive-mocks/
    /// </summary>
    public class RecursiveTests
    {
        private INumberParserFactory factory;
        private INumberParser parser;

        [SetUp]
        public void Setup()
        {
            // Moved to tests to demonstrate difference in manual and auto substitute creation
        }

        [Test]
        public void ManuallyCreateSubstitutes()
        {
            // create both subs
            factory = Substitute.For<INumberParserFactory>();
            parser = Substitute.For<INumberParser>();

            // now set up both separately
            factory.Create(',').Returns(parser);
            parser.Parse("an expression").Returns(new[] { 1, 2, 3 });

            Assert.AreEqual(
                factory.Create(',').Parse("an expression"),
                new[] { 1, 2, 3 });
        }

        [Test]
        public void AutomaticallyCreateSubstitutes()
        {
            // just create factory sub
            factory = Substitute.For<INumberParserFactory>();

            // chain set up, as factory creates parser, nice!
            factory.Create(',').Parse("an expression").Returns(new[] { 1, 2, 3 });

            Assert.AreEqual(
                factory.Create(',').Parse("an expression"),
                new[] { 1, 2, 3 });
        }

        [Test]
        public void RecursivelySubbedMethodCalledMultipleTimes()
        {
            factory = Substitute.For<INumberParserFactory>();

            // subsequent calls with same args return same sub
            var firstCall = factory.Create(',');
            var secondCall = factory.Create(',');

            // subsequent call with different args return different sub
            var thirdCallWithDiffArg = factory.Create('x');

            // prove this via assertions
            Assert.AreSame(firstCall, secondCall);
            Assert.AreNotSame(firstCall, thirdCallWithDiffArg);
        }

        [Test]
        public void SubstituteChaining()
        {
            var context = Substitute.For<IContext>();

            // recursively created substitutes can be chained like this madness, which breaks the LoD http://www.wikiwand.com/en/Law_of_Demeter 
            context.CurrentRequest.Identity.Name.Returns("My pet fish Eric");
            
            Assert.AreEqual(
                "My pet fish Eric",
                context.CurrentRequest.Identity.Name);
        }

        [Test]
        public void AutomaticallyAssignedValues()
        {
            // String and Array properties of substitutes get non-null default values
            var identity = Substitute.For<IIdentity>();

            // remember when asserting on substitutes you have not explicitly set all properties for
            Assert.AreEqual(String.Empty, identity.Name);
            Assert.AreEqual(0, identity.Roles().Length);
        }
        
        [TearDown]
        public void TearDown()
        {
            // meh no one tells me nothing...
        }    
    }
}
