using VendingMachine;
using NUnit.Framework;

namespace VendingMachineTests
{
    [TestFixture]
    class StockItemUnitTests
    {
        public StockItem stockItem;

        /**
         * <summary>
         * Tests that a stock item is instantiated correctly, and that the attributes are populated.
         * </summary>
         */
        [Test]
        public void UnitTest_StockItem()
        {
            stockItem = new StockItem("Reese's", 1.5, 3);

            Assert.AreEqual(3, stockItem.count);
            Assert.AreEqual(1.5, stockItem.cost);
            Assert.AreEqual("Reese's", stockItem.name);
        }
    }
}
