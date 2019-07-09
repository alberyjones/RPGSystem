using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RPGSystem.Utils;

namespace RPGSystem
{
    public class GameConfiguration
    {
        public static Alignments Alignments
        {
            get
            {
                return GetSingleton<Alignments>(@"Characters\Alignments.xml", (a) => { a.BuildLookups(); });
            }
        }

        public static CharacterClasses CharacterClasses
        {
            get
            {
                return GetSingleton<CharacterClasses>(@"Characters\CharacterClasses.xml", (cc) => { cc.BuildLookups(); });
            }
        }

        public static CharacterSizes CharacterSizes
        {
            get
            {
                return GetSingleton<CharacterSizes>(@"Characters\CharacterSizes.xml", (cs) => { cs.BuildLookups(); });
            }
        }

        public static Equipment Equipment
        {
            get
            {
                return GetSingleton<Equipment>(@"Characters\Equipment.xml", (eq) => { eq.BuildLookups(); });
            }
        }

        public static EquipmentTypes EquipmentTypes
        {
            get
            {
                return GetSingleton<EquipmentTypes>(@"Characters\EquipmentTypes.xml", (et) => { et.BuildLookups(); });
            }
        }

        public static Genders Genders
        {
            get
            {
                return GetSingleton<Genders>(@"Characters\Genders.xml", (g) => { g.BuildLookups(); });
            }
        }

        public static Races Races
        {
            get
            {
                return GetSingleton<Races>(@"Characters\Races.xml", (r) => { r.BuildLookups(); });
            }
        }

        public static SkillTypes SkillTypes
        {
            get
            {
                return GetSingleton<SkillTypes>(@"Characters\SkillTypes.xml", (st) => { st.BuildLookups(); });
            }
        }

        #region Singleton storage / caching / lookup
        private static Dictionary<string, object> singletons = new Dictionary<string, object>();

        private static string GetKey(Type type, string fileName)
        {
            return type.FullName + "#" + fileName;
        }

        private static bool TryGetValue<T>(string key, out T value) where T : class
        {
            object singleton = null;
            if (singletons.TryGetValue(key, out singleton))
            {
                value = singleton as T;
                return (value != null);
            }
            value = null;
            return false;
        }

        public static string ConfigFolder { get; set; } = Environment.CurrentDirectory;

        private static T GetSingleton<T>(string relativeFilePath, Action<T> postLoadAction = null) where T : class
        {
            if (String.IsNullOrEmpty(relativeFilePath))
            {
                return null;
            }
            string key = GetKey(typeof(T), relativeFilePath);
            T singleton = null;
            if (!TryGetValue(key, out singleton))
            {
                if (TryLoad<T>(Path.Combine(ConfigFolder, relativeFilePath), out singleton))
                {
                    singletons[key] = singleton;
                    postLoadAction?.Invoke(singleton);
                }
            }
            return singleton;
        }
        #endregion
    }
}
