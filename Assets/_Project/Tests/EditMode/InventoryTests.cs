using System;
using NUnit.Framework;
using SurvivalCore.Inventory;

namespace SurvivalCore.Tests
{
    public class InventoryTests
    {
        private ItemDefinition _wood;
        private ItemDefinition _sword;

        [SetUp]
        public void SetUp()
        {
            _wood = new ItemDefinition("wood", maxStack: 10);
            _sword = new ItemDefinition("sword");
        }

        [Test]
        public void Add_StacksIntoExistingSlot()
        {
            var inv = new Inventory.Inventory(2);
            inv.Add(_wood, 4);
            inv.Add(_wood, 5);
            Assert.AreEqual(9, inv.CountOf("wood"));
        }

        [Test]
        public void Add_OverflowsToNewSlot()
        {
            var inv = new Inventory.Inventory(2);
            Assert.AreEqual(0, inv.Add(_wood, 15));
            Assert.AreEqual(15, inv.CountOf("wood"));
        }

        [Test]
        public void Add_WhenFull_ReturnsLeftover()
        {
            var inv = new Inventory.Inventory(1);
            Assert.AreEqual(4, inv.Add(_wood, 14));
            Assert.AreEqual(10, inv.CountOf("wood"));
        }

        [Test]
        public void Add_NonStackable_UsesOneSlotEach()
        {
            var inv = new Inventory.Inventory(2);
            Assert.AreEqual(1, inv.Add(_sword, 3));
            Assert.AreEqual(2, inv.CountOf("sword"));
        }

        [Test]
        public void Remove_Succeeds_AcrossStacks()
        {
            var inv = new Inventory.Inventory(3);
            inv.Add(_wood, 25);
            Assert.IsTrue(inv.Remove("wood", 12));
            Assert.AreEqual(13, inv.CountOf("wood"));
        }

        [Test]
        public void Remove_Insufficient_IsAtomic()
        {
            var inv = new Inventory.Inventory(2);
            inv.Add(_wood, 5);
            Assert.IsFalse(inv.Remove("wood", 6));
            Assert.AreEqual(5, inv.CountOf("wood"));
        }

        [Test]
        public void Remove_FreesSlot_ForOtherItems()
        {
            var inv = new Inventory.Inventory(1);
            inv.Add(_wood, 3);
            inv.Remove("wood", 3);
            Assert.AreEqual(0, inv.Add(_sword, 1));
        }

        [Test]
        public void Changed_FiresOnlyWhenContentsChange()
        {
            var inv = new Inventory.Inventory(1);
            int fired = 0;
            inv.Changed += () => fired++;

            inv.Add(_wood, 10);
            inv.Add(_wood, 1);
            inv.Remove("wood", 99);

            Assert.AreEqual(1, fired);
        }

        [Test]
        public void InvalidArguments_Throw()
        {
            var inv = new Inventory.Inventory(1);
            Assert.Throws<ArgumentNullException>(() => inv.Add(null, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => inv.Add(_wood, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Inventory.Inventory(0));
        }
    }
}
