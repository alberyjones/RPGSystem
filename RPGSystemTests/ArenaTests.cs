using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RPGSystem;
using RPGSystem.Characters;
using RPGSystem.Combat;

namespace RPGSystemTests
{
    [TestClass]
    public class ArenaTests
    {
        [TestInitialize]
        public void Setup()
        {
            GameConfiguration.ConfigFolder = @"F:\Documents\Visual Studio Projects\RPGSystem\RPGSystem";
        }

        [TestMethod]
        public void ArenaTest1()
        {
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ X _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            // _ _ _ _ _ _ _ _ _ _
            //
            Arena arena = new Arena() { SizeX = 10, SizeY = 10 };
            arena.InaccessibleLocations.Add(new Coordinates() { X = 3, Y = 3 });

            CharacterInstance tinyCharacter = new CharacterInstance() { Identifier = "T", SizeIdentifier = "Tiny" }; // 0.5 x 0.5
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 0, 0));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 1, 1));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 2, 2));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(tinyCharacter, 3, 3));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 4, 4));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 5, 5));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 6, 6));

            CharacterInstance mediumCharacter = new CharacterInstance() { Identifier = "M", SizeIdentifier = "Medium" }; // 1 x 1
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 0, 0));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 1, 1));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 2, 2));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(mediumCharacter, 3, 3));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 4, 4));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 5, 5));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(mediumCharacter, 6, 6));

            CharacterInstance largeCharacter = new CharacterInstance() { Identifier = "L", SizeIdentifier = "Large" }; // 2 x 2
            Assert.IsFalse(arena.CanCharacterBePlacedAt(largeCharacter, 0, 0));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(largeCharacter, 1, 1));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(largeCharacter, 2, 2));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(largeCharacter, 3, 3));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(largeCharacter, 4, 4));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(largeCharacter, 5, 5));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(largeCharacter, 6, 6));

            CharacterInstance hugeCharacter = new CharacterInstance() { Identifier = "H", SizeIdentifier = "Huge" }; // 3 x 3
            Assert.IsFalse(arena.CanCharacterBePlacedAt(hugeCharacter, 0, 0));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(hugeCharacter, 1, 1));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(hugeCharacter, 2, 2));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(hugeCharacter, 3, 3));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(hugeCharacter, 4, 4));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(hugeCharacter, 5, 5));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(hugeCharacter, 6, 6));

            CharacterInstance gargantuanCharacter = new CharacterInstance() { Identifier = "G", SizeIdentifier = "Gargantuan" }; // 4 x 4
            Assert.IsFalse(arena.CanCharacterBePlacedAt(gargantuanCharacter, 1, 1));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(gargantuanCharacter, 2, 2));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(gargantuanCharacter, 3, 3));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(gargantuanCharacter, 4, 4));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(gargantuanCharacter, 5, 5));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(gargantuanCharacter, 6, 6));

            Assert.IsTrue(arena.MoveCharacterTo(mediumCharacter, 1, 1));
            Assert.IsFalse(arena.CanCharacterBePlacedAt(tinyCharacter, 1, 1));

            Assert.IsTrue(arena.MoveCharacterTo(mediumCharacter, 0, 0));
            Assert.IsTrue(arena.CanCharacterBePlacedAt(tinyCharacter, 1, 1));
        }


        [TestMethod]
        public void ArenaTest2()
        {
            Arena arena = new Arena() { SizeX = 10, SizeY = 10 };
            arena.InaccessibleLocations.Add(new Coordinates() { X = 3, Y = 3 });
            CharacterInstance mediumCharacter = new CharacterInstance() { Identifier = "M", SizeIdentifier = "Medium" }; // 1 x 1
            CombatArenaPath path = CombatArenaPath.FindPath(
                mediumCharacter, arena,
                new Coordinates { X = 0, Y = 0 },
                new Coordinates { X = 9, Y = 9 });
        }

        [TestMethod]
        public void ArenaTest3()
        {
            Arena arena = new Arena() { SizeX = 10, SizeY = 10 };
            arena.InaccessibleLocations.Add(new Coordinates() { X = 3, Y = 3 });
            arena.Doors.Add(new CombatArenaDoor()
            {
                Identifier = "MainEntrance",
                Type = CombatArenaDoorType.Any,
                Location = new Coordinates { X = 4, Y = 0},
                Facing = SimpleMapDirection.North
            });
            arena.Doors.Add(new CombatArenaDoor()
            {
                Identifier = "MainExit",
                Type = CombatArenaDoorType.Any,
                Location = new Coordinates { X = 4, Y = 9 },
                Facing = SimpleMapDirection.North
            });

            CharacterInstance mediumCharacter = new CharacterInstance() { Identifier = "M", SizeIdentifier = "Medium" }; // 1 x 1

            Assert.IsTrue(arena.TryCharacterEnter(mediumCharacter));
        }
    }
}
