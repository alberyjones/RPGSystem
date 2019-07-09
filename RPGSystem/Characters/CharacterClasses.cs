using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPGSystem.Utils;

namespace RPGSystem.Characters
{
    public class CharacterClass : IdentifiableItem
    {
        public CharacterClass()
        {
        }

        [XmlElement]
        public string Description { get; set; }

        [XmlAttribute]
        public int MaxHit { get; set; }

        [XmlAttribute]
        public bool AllowMultiplePrimaryAbilities { get; set; }

        [XmlAttribute("PrimaryAbilities")]
        public string RawPrimaryAbilities { get; set; }

        [XmlIgnore]
        public IEnumerable<Ability> PrimaryAbilities
        {
            get
            {
                foreach (string ability in ListFromCommaSeparated(RawPrimaryAbilities))
                {
                    yield return (Ability)Enum.Parse(typeof(Ability), ability);
                }
            }
        }

        [XmlAttribute("SavingThrowProficiencies")]
        public string RawSavingThrowProficiencies { get; set; }

        [XmlIgnore]
        public IEnumerable<Ability> SavingThrowProficiencies
        {
            get
            {
                foreach (string ability in ListFromCommaSeparated(RawSavingThrowProficiencies))
                {
                    yield return (Ability)Enum.Parse(typeof(Ability), ability);
                }
            }
        }

        [XmlAttribute("EquipmentProficiencies")]
        public string RawEquipmentProficiencies { get; set; }

        [XmlIgnore]
        public IEnumerable<EquipmentType> EquipmentProficiencies
        {
            get
            {
                return GameConfiguration.EquipmentTypes.ItemsInAny(ListFromCommaSeparated(RawEquipmentProficiencies));
            }
        }

        [XmlArray]
        [XmlArrayItem("LevelModifier")]
        public List<LevelModifier> LevelModifiers { get; } = new List<LevelModifier>();

        public int ProficiencyBonusForLevel(int level)
        {
            int bonus = 0;
            // here, we assume the LevelModifiers collection is in ascending level order
            foreach (var levelModifier in LevelModifiers)
            {
                if (levelModifier.Level <= level && levelModifier.ProficiencyBonus > 0)
                {
                    bonus = levelModifier.ProficiencyBonus;
                }
            }
            return bonus;
        }
    }

    public class CharacterClasses : IdentifiableItemCollection<CharacterClass>
    {
        [XmlArray]
        [XmlArrayItem("Class")]
        public List<CharacterClass> AllClasses { get; } = new List<CharacterClass>();

        protected override IEnumerable<CharacterClass> EnumerateItems()
        {
            return AllClasses;
        }
    }
}
