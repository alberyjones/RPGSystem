using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class EquipmentItem : IdentifiableItem
    {
        [XmlAttribute]
        public double Weight { get; set; }

        [XmlIgnore]
        public EquipmentType EquipmentType { get; set; }

        [XmlAttribute]
        public string Properties { get; set; }
    }

    public class Weapon : EquipmentItem
    {
        [XmlAttribute]
        public int Cost { get; set; }

        [XmlAttribute]
        public string CostUnit { get; set; }

        [XmlAttribute]
        public string Damage { get; set; }

        [XmlAttribute]
        public string DamageProperties { get; set; }

        [XmlAttribute]
        public string Range { get; set; }
    }

    public class Equipment : IdentifiableItemCollection<EquipmentItem>
    {
        [XmlArray]
        [XmlArrayItem("Weapon")]
        public List<EquipmentItem> Weapons { get; } = new List<EquipmentItem>();

        [XmlArray]
        [XmlArrayItem("Armor")]
        public List<EquipmentItem> ArmorItems { get; } = new List<EquipmentItem>();

        protected override IEnumerable<EquipmentItem> EnumerateItems()
        {
            foreach (var item in Weapons)
            {
                yield return item;
            }
            foreach (var item in ArmorItems)
            {
                yield return item;
            }
        }
    }
}
