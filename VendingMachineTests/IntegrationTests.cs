using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using VendingMachine;

namespace VendingMachineTests
{
    [TestFixture]
    class IntegrationTests
    {
        string path = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\sampleStock";
        string path2 = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\sampleStock2.txt";
        string pathNull = "C:\\Users\\1315n\\Desktop\\School\\CIS\\CIS 640\\homework-4-whitebox-testing-sellersn8463-master\\nullStock.txt";


        [Test]
        public void IntegTest_CreateVendingMachine_StockItemCreatedCorrectly_OneItem()
        {
            // Arrange
            StockItem stockItem = new StockItem("Pibb", 1.25, 3);

            // Act
            VendingMachine.VendingMachine vendingMachine = makeVendingMachine(path2);

            // Assert
            StockItem item = vendingMachine.Stock[0];
            Assert.AreEqual(item.name, stockItem.name);
            Assert.AreEqual(item.cost, stockItem.cost);
            Assert.AreEqual(item.count, stockItem.count);
        }

        [Test]
        public void IntegTest_CreateVendingMachine_NoStockItems_EmptyStock()
        {
            // Act
            VendingMachine.VendingMachine vendingMachine = makeVendingMachine(pathNull);

            // Assert
            Assert.IsEmpty(vendingMachine.Stock);
        }

        [Test]
        public void IntegTest_CreateVendingMachine_MultipleItems()
        {
            // Arrange
            int size = 5;

            // Act
            VendingMachine.VendingMachine vendingMachine = makeVendingMachine(path);

            // Assert
            Assert.AreEqual(vendingMachine.Stock.Count, size);

        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_States_Start_Off()
        {
            RedirectSTDIn(path + "\ne");

            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);

            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.OFF);
        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_States_Start_Insert()
        {
            RedirectSTDIn(path + "\nq");

            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);

            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.INSERT);
        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_States_Start_Stock()
        {
            RedirectSTDIn(path + "\nr");

            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);

            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.STOCK);
        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_States_Stock_Start()
        {
            RedirectSTDIn(pathNull + "\nr\n" + path);
            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.STOCK);
            machine.Restock();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);
        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_FullSim1()
        {
            RedirectSTDIn(path + "\nq\n1\n1\n4");

            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);

            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.INSERT);

            machine.InsertMoney();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.INSERT);

            machine.InsertMoney();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.SELECT);

            machine.Select();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.DISPENSE);

            machine.DispenseSelection();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.CHANGE);
            
            machine.DispenseChange();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);
        }

        [Test]
        public void IntegTest_VendingMachine_Relationship_FullSim2()
        {
            RedirectSTDIn(path2 + "\nq\n1\n0.25\n0");

            VendingMachine.VendingMachine machine = new VendingMachine.VendingMachine();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);

            machine.Start();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.INSERT);

            machine.InsertMoney();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.INSERT);

            machine.InsertMoney();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.SELECT);

            machine.Select();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.DISPENSE);

            machine.DispenseSelection();
            Assert.That(machine.Status == VendingMachine.VendingMachine.State.START);
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
