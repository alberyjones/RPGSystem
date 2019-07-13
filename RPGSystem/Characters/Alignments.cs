using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public BindingList<Alignment> AllAlignments { get; } = new BindingList<Alignment>();

        protected override IEnumerable<Alignment> EnumerateItems()
        {
            return this.AllAlignments;
        }
    }
}
