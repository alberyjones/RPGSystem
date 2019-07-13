using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class EquipmentInstance : NotifyPropertyChangedBase
    {
        private string equipmentTypeIdentifier;
        [XmlAttribute]
        public string EquipmentItemIdentifier { get => equipmentTypeIdentifier; set => SetField(ref equipmentTypeIdentifier, value); }

        private int quantity;
        [XmlAttribute]
        public int Quantity { get => quantity; set => SetField(ref quantity, value); }
    }
}
