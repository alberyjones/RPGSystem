using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class CharacterSize : IdentifiableItem
    {
        private double footprintModifier;
        [XmlAttribute]
        public double FootprintModifier { get => footprintModifier; set => SetField(ref footprintModifier, value); }

        private double liftCarryModifier;
        [XmlAttribute]
        public double LiftCarryModifier { get => liftCarryModifier; set => SetField(ref liftCarryModifier, value); }
    }

    public class CharacterSizes : IdentifiableItemCollection<CharacterSize>
    {
        private string defaultSize;
        [XmlAttribute]
        public string DefaultSize { get => defaultSize; set => SetField(ref defaultSize, value); }

        [XmlArray]
        [XmlArrayItem("Size")]
        public BindingList<CharacterSize> AllSizes { get; } = new BindingList<CharacterSize>();

        protected override IEnumerable<CharacterSize> EnumerateItems()
        {
            return this.AllSizes;
        }
    }
}
