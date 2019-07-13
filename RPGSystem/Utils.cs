using RPGSystem.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem
{
    public class Utils
    {
        private Utils() { }

        public static Random RandomSingleton { get; } = new Random();

        public static bool TryLoad<T>(string file, out T item) where T : class
        {
            item = null;
            try
            {
                if (!String.IsNullOrEmpty(file) && File.Exists(file))
                {
                    using (var fs = File.OpenRead(file))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        item = serializer.Deserialize(fs) as T;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static bool Save<T>(T item, string file) where T : class
        {
            try
            {
                if (item != null)
                {
                    using (var fs = File.OpenWrite(file))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(fs, item);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public static List<string> ListFromCommaSeparated(string text)
        {
            List<string> list = new List<string>();
            if (!String.IsNullOrEmpty(text))
            {
                list.AddRange(text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
            }
            return list;
        }

        public static string CommaSeparatedFromList(List<string> list)
        {
            if (list == null || list.Count == 0)
            {
                return String.Empty;
            }
            return String.Join(",", list);
        }

        public static List<string> MergeUnique(List<string> list1, List<string> list2)
        {
            if (list1 == null)
            {
                return list2;
            }
            if (list2 == null)
            {
                return list1;
            }
            List<string> merged = new List<string>(list1);
            foreach(string item in list2)
            {
                if (!merged.Contains(item))
                {
                    merged.Add(item);
                }
            }
            return merged;
        }

        public static string MergeUniqueCommaSeparated(string list1, string list2)
        {
            if (String.IsNullOrEmpty(list1))
            {
                return list2;
            }
            if (String.IsNullOrEmpty(list2))
            {
                return list1;
            }
            return String.Join(",", MergeUnique(ListFromCommaSeparated(list1), ListFromCommaSeparated(list2)));
        }

        public static bool IsNullOrDefault<T>(T value)
        {
            return value == null || value.Equals(default(T));
        }

        public static T OverrideIfNullOrDefault<T>(T currentValue, T overrideValue)
        {
            return IsNullOrDefault<T>(currentValue) ? overrideValue : currentValue;
        }

        public static int AbilityModifierFromScore(int score)
        {
            return (score - 10) / 2;
        }

        public static ChallengeResult GetResult(int charScore, int difficultyClass)
        {
            if (charScore > difficultyClass)
            {
                return ChallengeResult.Succeed;
            }
            else if (charScore < difficultyClass)
            {
                return ChallengeResult.Fail;
            }
            return ChallengeResult.Tie;
        }

        public static void AddRange<T>(BindingList<T> list, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (T item in items)
                {
                    list.Add(item);
                }
            }
        }

        //public static void BasicReflectCopy(object child, object parent, params string[] ignoreProps)
        //{
        //    if (child != null && parent != null)
        //    {
        //        Type sharedType = child.GetType();
        //        if (sharedType == parent.GetType())
        //        {
        //            List<string> toIgnore = new List<string>();
        //            if (ignoreProps != null && ignoreProps.Length > 0)
        //            {
        //                toIgnore.AddRange(ignoreProps);
        //            }
        //            var properties = sharedType.GetProperties();
        //            foreach (var prop in properties)
        //            {
        //                if (!toIgnore.Contains(prop.Name) && prop.CanRead && prop.CanWrite)
        //                {
        //                    if (prop.PropertyType == typeof(string))
        //                    {
        //                        UpdateProperty<string>(child, parent, prop);
        //                    }
        //                    else if (prop.PropertyType == typeof(int))
        //                    {
        //                        UpdateProperty<int>(child, parent, prop);
        //                    }
        //                    else if (prop.PropertyType == typeof(bool))
        //                    {
        //                        UpdateProperty<bool>(child, parent, prop);
        //                    }
        //                    else 
        //                    {
        //                        UpdateProperty<object>(child, parent, prop);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void UpdateProperty<T>(object child, object parent, PropertyInfo propertyInfo)
        //{
        //    T val = (T)propertyInfo.GetValue(child);
        //    if (val == null || val.Equals(default(T)))
        //    {
        //        val = (T)propertyInfo.GetValue(parent);
        //        propertyInfo.SetValue(child, val);
        //    }
        //}
    }

    public class ListDictionary<TKey, TItem> : Dictionary<TKey, List<TItem>>
    {
        public List<TItem> FindOrCreate(TKey key)
        {
            List<TItem> items = null;
            if (!this.TryGetValue(key, out items))
            {
                items = new List<TItem>();
                this[key] = items;
            }
            return items;
        }

        public bool AddUnique(TKey key, TItem item)
        {
            List<TItem> list = FindOrCreate(key);
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }
            return false;
        }
    }

    public class GameException : Exception
    {
        public GameException(string message = null)
            : base(message)
        {
        }
    }

    public enum SimpleMapDirection
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}
