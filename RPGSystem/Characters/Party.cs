using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Party : IdentifiableItem
    {
        public List<CharacterInstance> Members { get; } = new List<CharacterInstance>();
    }
}
