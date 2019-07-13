using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class SkillType : IdentifiableItem
    {
    }

    public class SkillTypes : IdentifiableItemCollection<SkillType>
    {
        [XmlArray]
        [XmlArrayItem("SkillType")]
        public BindingList<SkillType> AllSkillTypes { get; } = new BindingList<SkillType>();

        protected override IEnumerable<SkillType> EnumerateItems()
        {
            return this.AllSkillTypes;
        }
    }
}
