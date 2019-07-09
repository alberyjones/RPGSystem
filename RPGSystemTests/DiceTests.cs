using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGSystem;

namespace RPGSystemTests
{
    [TestClass]
    public class DiceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            TestRoll("1d6+5", 6, 11);
            TestRoll("1d6 + 5", 6, 11);
            TestRoll("1d6+3d4+10", 14, 28);
            TestRoll("d6", 1, 6);
            TestRoll("d6-9", -8, -3);
            TestRoll("d20", 1, 20);
            TestRoll("d20+2d6", 3, 32);
            TestRoll("2d8", 2, 16);
            TestRoll("2d4", 2, 8);
        }

        private void TestRoll(string rollDetails, int expectedMin, int expectedMax)
        {
            int roll = Dice.Roll(rollDetails);
            Assert.IsTrue(roll >= expectedMin && roll <= expectedMax);

            Roller diceRoll = new Roller(rollDetails);
            Assert.IsTrue(diceRoll.Roll() >= expectedMin && roll <= expectedMax);
            Assert.AreEqual(expectedMin, diceRoll.GetMin());
            Assert.AreEqual(expectedMax, diceRoll.GetMax());
        }
    }
}
