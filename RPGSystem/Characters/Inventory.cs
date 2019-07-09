using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Inventory
    {
        [XmlArray]
        [XmlArrayItem("EquipmentInstance")]
        public List<EquipmentInstance> EquipmentInstances { get; } = new List<EquipmentInstance>();
    }
}
