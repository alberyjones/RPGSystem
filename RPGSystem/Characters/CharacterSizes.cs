using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class CharacterSize : IdentifiableItem
    {
        [XmlAttribute]
        public double FootprintModifier { get; set; }

        [XmlAttribute]
        public double LiftCarryModifier { get; set; }
    }

    public class CharacterSizes : IdentifiableItemCollection<CharacterSize>
    {
        [XmlAttribute]
        public string DefaultSize { get; set; }

        [XmlArray]
        [XmlArrayItem("Size")]
        public List<CharacterSize> AllSizes { get; } = new List<CharacterSize>();

        protected override IEnumerable<CharacterSize> EnumerateItems()
        {
            return this.AllSizes;
        }
    }
}
