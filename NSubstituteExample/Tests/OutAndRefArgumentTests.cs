using System;
using NSubstitute;
using NSubstituteExample.Interfaces;
using NUnit.Framework;

namespace NSubstituteExample.Tests
{
    /// <summary>
    /// see: http://nsubstitute.github.io/help/setting-out-and-ref-arguments/
    /// </summary>
    public class OutAndRefArgumentTests
    {
        private ILookup lookup;

        [SetUp]
        public void Setup()
        {
            lookup = Substitute.For<ILookup>();
        }

        [Test]
        public void OutArgument()
        {
            var value = String.Empty;
            Assert.IsEmpty(value); // to keep stylecop happy

            // unsure how you would do this using the When...Do syntax, although the docs state you can?!
            lookup.TryLookup("Hello", out value)
                .Returns(x =>
                {
                    x[1] = "World";
                    return true;
                }); // configure both return value and out param

            var result = lookup.TryLookup("Hello", out value);

            // assert return value and out param value
            Assert.IsTrue(result);
            Assert.AreEqual(value, "World");
        }

        [TearDown]
        public void TearDown()
        {
            lookup = null;
        }    
    }
}
