using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Alignment : IdentifiableItem
    {
    }

    public class Alignments : IdentifiableItemCollection<Alignment>
    {
        [XmlArray]
        [XmlArrayItem("Alignment")]
        public List<Alignment> AllAlignments { get; } = new List<Alignment>();

        protected override IEnumerable<Alignment> EnumerateItems()
        {
            return this.AllAlignments;
        }
    }
}
