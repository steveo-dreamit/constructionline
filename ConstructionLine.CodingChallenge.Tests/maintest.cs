using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class Maintest
    {

        [Test]
        public void AddMethod()
        {
            var result = 1 + 1;
            Assert.IsTrue(result == 2);
        }
    }
}
