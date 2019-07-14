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
            return GameConfiguration.DataLoader.Save<Game>(this, file);
        }

        public static Game Load(string file)
        {
            GameConfiguration.DataLoader.TryLoad<Game>(file, out Game game);
            return game;
        }

        public static Game ActiveGame { get; set; }

        public static Game CreateExampleGame()
        {
            Game game = new Game();
            game.DisplayName = "Example Game";
            game.Description = "Generated example game";

            CharacterInstance c = CharacterInstance.RollNew("Bob The Swordsman", 1);
            game.Characters.Add(c);

            c = CharacterInstance.RollNew("Urgar The Deadly", 5, "HalfOrc", "Barbarian");
            game.Characters.Add(c);

            c = CharacterInstance.RollNew("Tietron The Wise", 9, "HighElf", "Cleric");
            game.Characters.Add(c);

            return game;
        }
    }
}
