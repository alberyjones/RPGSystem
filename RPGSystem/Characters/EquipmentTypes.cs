using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPGSystem.Utils;

namespace RPGSystem.Characters
{
    public class EquipmentType : IdentifiableItem
    {
        private string groups;
        [XmlAttribute]
        public string Groups { get => groups; set => SetField(ref groups, value); }
    }

    public class EquipmentTypes : IdentifiableItemCollection<EquipmentType>
    {
        private ListDictionary<string, string> groupings = new ListDictionary<string, string>();

        [XmlArray]
        [XmlArrayItem("Equipment")]
        public BindingList<EquipmentType> AllEquipment { get; } = new BindingList<EquipmentType>();

        public IEnumerable<EquipmentType> ItemsInGroup(string group)
        {
            if (groupings.ContainsKey(group))
            {
                foreach (string key in groupings[group])
                {
                    EquipmentType found = Find(key);
                    if (found != null)
                    {
                        yield return found;
                    }
                }
            }
        }

        public IEnumerable<EquipmentType> ItemsInAny(params string[] items)
        {
            return InternalItemsInAny(items);
        }

        public IEnumerable<EquipmentType> ItemsInAny(IEnumerable<string> items)
        {
            return InternalItemsInAny(items);
        }

        private IEnumerable<EquipmentType> InternalItemsInAny(IEnumerable<string> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (IsGroup(item))
                    {
                        foreach (EquipmentType eqpt in ItemsInGroup(item))
                        {
                            yield return eqpt;
                        }
                    }
                    else
                    {
                        EquipmentType eqpt = Find(item);
                        if (eqpt != null)
                        {
                            yield return eqpt;
                        }
                    }
                }
            }
        }

        public bool IsGroup(string identifier)
        {
            return !String.IsNullOrEmpty(identifier) && groupings.ContainsKey(identifier);
        }

        protected override IEnumerable<EquipmentType> EnumerateItems()
        {
            return AllEquipment;
        }

        protected internal override void BuildLookups()
        {
            groupings.Clear();
            base.BuildLookups();
        }

        protected override void PostLoad(EquipmentType item)
        {
            base.PostLoad(item);
            if (!String.IsNullOrEmpty(item.Groups))
            {
                foreach (string group in ListFromCommaSeparated(item.Groups))
                {
                    groupings.AddUnique(group, item.Identifier);
                }
            }
        }
    }
}
