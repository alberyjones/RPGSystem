﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem
{
    public class IdentifiableItem
    {
        private string explicitDisplayName;

        [XmlAttribute]
        public string Identifier { get; set; }

        [XmlAttribute]
        public string DisplayName
        {
            get { return String.IsNullOrEmpty(explicitDisplayName) ? Identifier : explicitDisplayName; }
            set { explicitDisplayName = value; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override int GetHashCode()
        {
            return Identifier?.GetHashCode() ?? 0;
        }
    }

    public abstract class IdentifiableItemCollection<T> where T : IdentifiableItem
    {
        private Dictionary<string, T> mainLookup = new Dictionary<string, T>();

        protected abstract IEnumerable<T> EnumerateItems();

        protected virtual string GetIdentifier(T item)
        {
            return item?.Identifier;
        }

        protected virtual void PostLoad(T item)
        {
        }

        public T Find(string identifier)
        {
            if (mainLookup.ContainsKey(identifier))
            {
                return mainLookup[identifier];
            }
            return null;
        }

        internal protected virtual void BuildLookups()
        {
            mainLookup.Clear();
            var collection = EnumerateItems();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    string identifier = GetIdentifier(item);
                    if (!String.IsNullOrEmpty(identifier))
                    {
                        mainLookup[identifier] = item;
                        PostLoad(item);
                    }
                }
            }
        }

        protected void SetLookup(string identifier, T item)
        {
            if (!String.IsNullOrEmpty(identifier))
            {
                mainLookup[identifier] = item;
            }
        }
    }
}
