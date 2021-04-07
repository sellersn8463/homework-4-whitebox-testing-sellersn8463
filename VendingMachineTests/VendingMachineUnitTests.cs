using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace VendingMachineTests
{
    [TestFixture]
    class VendingMachineUnitTests
    {
        private VendingMachine.VendingMachine vendingMachine = null;
        string pathNull = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\nullStock.txt";
        string path = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\sampleStock";
        string path1 = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\sampleStock1";

        [SetUp]
        public void SetUp()
        {
            vendingMachine = makeVendingMachine(path);
        }

        /**
         * <summary>
         * Tests that when a user selects 'r' when a vending machine is started, the
         * state swithes to stock.
         * </summary>
         */
        [Test]
        [Timeout(1000)]
        [Category("Start")]
        public void UnitTest_Start_Restock()
        {
            // Arrange
            RedirectSTDIn("r\n");

            // Act
            vendingMachine.Start();

            // Assert
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.STOCK);
        }

        /**
         * <summary>
         * Tests that when a user selects 'e' when a vending machine is started, the
         * state swithes to OFF.  There is also a timeout of 1000ms.
         * </summary>
         */
        [Test]
        [Timeout(1000)]
        [Category("Start")]
        public void UnitTest_Start_Exit()
        {
            // Arrange
            RedirectSTDIn("e\n");

            // Act
            vendingMachine.Start();

            // Assert
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.OFF);
        }

        [Test]
        [Timeout(1000)]
        [Category("Start")]
        public void UnitTest_Start_AnythingElse()
        {
            // Arrange
            RedirectSTDIn("f\n");

            // Act
            vendingMachine.Start();

            // Assert
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.INSERT);
        }

        [Test]
        public void UnitTest_LoadStock()
        {
            // Arrange & Act
            VendingMachine.VendingMachine machine = makeVendingMachine(path);

            // Assert
            Assert.AreEqual(machine.MaxCost, 2.0);
        }

        [Test]
        public void UnitTest_GetFileName()
        {
            // Arrange
            RedirectSTDIn(path);

            // Act
            string result = vendingMachine.GetFileName();

            // Assert
            Assert.AreEqual(path, result);
        }

        [Test]
        public void UnitTest_ReadFile()
        {
            // Arrange
            RedirectSTDIn(path);
            List<string> list = new List<string>()
            {
                "Cola,1.25,3",
                "Diet Cola,1.25,5",
                "Surge,2.00,2",
                "Fruit Snacks,.75,7",
                "Jerky,1.50,10"
            };

            // Act
            List<string> result = vendingMachine.ReadFile();

            // Assert
            Assert.AreEqual(result, list);
        }

        [Test]
        [Category("InsertMoney")]
        public void UnitTest_InsertMoney_MultipleInserts()
        {
            // Arrange
            RedirectSTDIn(".25\n.25\n.25");

            // Act
            vendingMachine.InsertMoney();
            vendingMachine.InsertMoney();
            vendingMachine.InsertMoney();

            // Assert
            Assert.AreEqual(vendingMachine.Balance, 0.75);
        }

        [Test]
        [TestCase("1")]
        [TestCase("0.25")]
        [TestCase("0.1")]
        [TestCase("0.05")]
        [Category("InsertMoney")]
        public void UnitTest_InsertMoney_Parameterized(string amount)
        {
            // Arrange
            RedirectSTDIn(amount);

            // Act
            vendingMachine.InsertMoney();

            // Assert
            Assert.AreEqual(vendingMachine.Balance, Convert.ToDouble(amount));
        }

        [Test]
        [TestCase("2")]
        [TestCase("5")]
        [Category("InsertMoney")]
        public void UnitTest_InsertMoney_MaxCostExceeded(string amount)
        {
            // Arrange
            RedirectSTDIn(amount);

            // Act
            vendingMachine.InsertMoney();

            // Assert
            Assert.AreEqual(vendingMachine.Balance, Convert.ToDouble(amount));
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.SELECT);
        }

        [Test]
        public void UnitTest_DispenseChange()
        {
            vendingMachine.DispenseChange();

            Assert.AreEqual(vendingMachine.Balance, 0);
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.START);
        }

        [Test]
        public void UnitTest_DispenseSelection_NoChange()
        {
            // Arrange
            RedirectSTDIn("1\n1\n2");

            // Act
            vendingMachine.InsertMoney();
            vendingMachine.InsertMoney();
            vendingMachine.Select();
            vendingMachine.DispenseSelection();

            // Assert
            Assert.AreEqual(vendingMachine.Balance, 0);
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.START);
        }

        [Test]
        public void UnitTest_DispenseSelection_Change()
        {
            // Arrange
            RedirectSTDIn("1\n1\n1");

            // Act
            vendingMachine.InsertMoney();
            vendingMachine.InsertMoney();
            vendingMachine.Select();
            vendingMachine.DispenseSelection();

            // Assert
            Assert.AreEqual(vendingMachine.Balance, 0.75);
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.CHANGE);
        }

        [Test]
        public void UnitTest_Select_Refund()
        {
            // Arrange
            RedirectSTDIn("r");

            // Act
            vendingMachine.Select();

            // Assert
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.CHANGE);
        }

        [Test]
        public void UnitTest_Select_Valid_Number()
        {
            // Arrange
            RedirectSTDIn("2");

            // Act
            vendingMachine.Select();

            // Assert
            Assert.That(vendingMachine.Status == VendingMachine.VendingMachine.State.DISPENSE);
            Assert.AreEqual(vendingMachine.Selection, 2);
        }

        [Test]
        public void UnitTest_Select_Invalid_Number()
        {
            // Arrange
            RedirectSTDIn("50");

            // Act
            vendingMachine.Select();

            // Assert
            Assert.That(vendingMachine.Status != VendingMachine.VendingMachine.State.DISPENSE);
            Assert.AreEqual(vendingMachine.Selection, -1);
        }

        private void RedirectSTDIn(string input)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(input);
            writer.Flush();
            stream.Position = 0;
            Console.SetIn(new StreamReader(stream));
        }
        public VendingMachine.VendingMachine makeVendingMachine(string stockFilePath)
        {
            RedirectSTDIn(stockFilePath);
            VendingMachine.VendingMachine vm = new VendingMachine.VendingMachine();
            return vm;
        }
    }
}
