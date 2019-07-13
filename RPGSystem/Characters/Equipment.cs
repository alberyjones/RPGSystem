using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class EquipmentItem : IdentifiableItem
    {
        private double weight;
        [XmlAttribute]
        public double Weight { get => weight; set => SetField(ref weight, value); }

        private EquipmentType equipmentType;
        [XmlIgnore]
        public EquipmentType EquipmentType { get => equipmentType; set => SetField(ref equipmentType, value); }

        private string properties;
        [XmlAttribute]
        public string Properties { get => properties; set => SetField(ref properties, value); }
    }

    public class Weapon : EquipmentItem
    {
        private int cost;
        [XmlAttribute]
        public int Cost { get => cost; set => SetField(ref cost, value); }

        private string costUnit;
        [XmlAttribute]
        public string CostUnit { get => costUnit; set => SetField(ref costUnit, value); }

        private string damage;
        [XmlAttribute]
        public string Damage { get => damage; set => SetField(ref damage, value); }

        private string damageProperties;
        [XmlAttribute]
        public string DamageProperties { get => damageProperties; set => SetField(ref damageProperties, value); }

        private string range;
        [XmlAttribute]
        public string Range { get => range; set => SetField(ref range, value); }
    }

    public class Equipment : IdentifiableItemCollection<EquipmentItem>
    {
        [XmlArray]
        [XmlArrayItem("Weapon")]
        public BindingList<EquipmentItem> Weapons { get; } = new BindingList<EquipmentItem>();

        [XmlArray]
        [XmlArrayItem("Armor")]
        public BindingList<EquipmentItem> ArmorItems { get; } = new BindingList<EquipmentItem>();

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
