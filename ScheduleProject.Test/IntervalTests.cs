using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScheduleProject.search;

namespace ScheduleProject.Tests
{
    [TestClass]
    public class IntervalTest
    {
        [TestMethod]
        public void NearestUp()
        {
            Interval interval = new Interval(10, 20, 3);

            int input = 20;
            int expected = -1;
            int actual = interval.NearestUp(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
