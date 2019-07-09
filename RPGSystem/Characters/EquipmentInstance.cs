using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class EquipmentInstance
    {
        [XmlAttribute]
        public string EquipmentItemIdentifier { get; set; }

        [XmlAttribute]
        public int Quantity { get; set; }
    }
}
