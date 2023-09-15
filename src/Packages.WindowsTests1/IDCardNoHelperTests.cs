#region

using System.Linq;
using NUnit.Framework;

#endregion

namespace System.Tests
{
    [TestFixture]
    public class IDCardNoHelperTests
    {
        [Test]
        public void GenerateIdCardNosTest()
        {
            var idCardNos = IDCardNoHelper.GenerateIdCardNos(1000);
            Assert.AreEqual(idCardNos.Count(), 1000);
        }
    }
}