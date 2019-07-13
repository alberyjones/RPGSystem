using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem.Characters
{
    public class Party : IdentifiableItem
    {
        public BindingList<CharacterInstance> Members { get; } = new BindingList<CharacterInstance>();
    }
}
