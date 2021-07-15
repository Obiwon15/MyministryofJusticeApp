using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ministryofjusticeDomain.Repositories;
using NUnit.Framework;

namespace TestMinistryOfJusticeapp.UnitTests
{
    [TestFixture]
    public class MathsTest
    {
        private Maths _math;

        [SetUp]
        public void SetUP()
        {
            _math = new Maths();
        }

        [Test]
        public void Add_WhenCalled_ReturnTheSumOfArguments()
        {
            var result = _math.Add(1, 2);

            Assert.That(result, Is.EqualTo(expected: 3));
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 1, 1)]
        public void Max_WhenArgumentIsPassed_ReturnGreaterArgumentOrTheSameArgument(int a, int b, int exceptedResult)
        {
            var result = _math.Max(a, b);
            Assert.That(result, Is.EqualTo(exceptedResult));
        }

        [Test]
        [Ignore("I chose to Ignore")]
        public void Max_WhenSecondArgumentIsGreater_ReturnSecondArgument()
        {
            var result = _math.Max(1, 2);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void GetOddNumbers_WhenReachLimit_ReturnTheOldNumber()
        {
            var result = _math.GetOddNumbers(10);
            Assert.That(result.Count(), Is.EqualTo(5));
        }
    }
}