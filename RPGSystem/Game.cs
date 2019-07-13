using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem
{
    public class Game : IdentifiableItemCollection<CharacterInstance>
    {
        private string displayName;
        [XmlElement]
        public string DisplayName { get => displayName; set => SetField(ref displayName, value); }

        private string description;
        [XmlElement]
        public string Description { get => description; set => SetField(ref description, value); }

        [XmlArray]
        [XmlArrayItem("Character")]
        public BindingList<CharacterInstance> Characters { get; } = new BindingList<CharacterInstance>();

        protected override IEnumerable<CharacterInstance> EnumerateItems()
        {
            return Characters;
        }

        public bool Save(string file)
        {
            return Utils.Save<Game>(this, file);
        }

        public static Game Load(string file)
        {
            Utils.TryLoad<Game>(file, out Game game);
            return game;
        }

        public static Game ActiveGame { get; set; }

        public static Game CreateExampleGame()
        {
            Game game = new Game();
            game.DisplayName = "Example Game";
            game.Description = "Generated example game";

            CharacterInstance c = new CharacterInstance();
            c.DisplayName = "Bob The Swordsman";
            c.RaceIdentifier = "Human";
            c.CharacterClassIdentifier = "Fighter";
            c.RollBasedOnRace();
            game.Characters.Add(c);

            c = new CharacterInstance();
            c.DisplayName = "Urgar The Deadly";
            c.RaceIdentifier = "HalfOrc";
            c.CharacterClassIdentifier = "Barbarian";
            c.RollBasedOnRace();
            game.Characters.Add(c);

            c = new CharacterInstance();
            c.DisplayName = "Tietron The Wise";
            c.RaceIdentifier = "HighElf";
            c.CharacterClassIdentifier = "Cleric";
            c.RollBasedOnRace();
            game.Characters.Add(c);

            return game;
        }
    }
}
