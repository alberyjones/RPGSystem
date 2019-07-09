using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem
{
    public class Game : IdentifiableItemCollection<CharacterInstance>
    {
        [XmlArray]
        [XmlArrayItem("Character")]
        public List<CharacterInstance> Characters { get; } = new List<CharacterInstance>();

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
    }
}
