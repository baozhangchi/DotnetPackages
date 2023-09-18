#region

using NUnit.Framework;

#endregion

namespace System.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [Test]
        public void ToLunarStringTest()
        {
            Assert.IsNotEmpty(DateTime.Today.ToLunarString());
            Assert.IsNotEmpty(new DateTime(2023, 3, 23).ToLunarString());
            Assert.IsNotEmpty(new DateTime(2023, 3, 20).ToLunarString());
        }
    }
}