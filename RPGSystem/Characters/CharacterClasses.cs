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
    public class CharacterClass : IdentifiableItem
    {
        public CharacterClass()
        {
        }

        private string description;
        [XmlElement]
        public string Description { get => description; set => SetField(ref description, value); }

        private int maxHit;
        [XmlAttribute]
        public int MaxHit { get => maxHit; set => SetField(ref maxHit, value); }

        private bool allowMultiplePrimaryAbilities;
        [XmlAttribute]
        public bool AllowMultiplePrimaryAbilities { get => allowMultiplePrimaryAbilities; set => SetField(ref allowMultiplePrimaryAbilities, value); }

        private string rawPrimaryAbilities;
        [XmlAttribute("PrimaryAbilities")]
        public string RawPrimaryAbilities
        {
            get => rawPrimaryAbilities;
            set
            {
                SetField(ref rawPrimaryAbilities, value);
                OnPropertyChanged(nameof(PrimaryAbilities));
            }
        }

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

        private string rawSavingThrowProficiencies;
        [XmlAttribute("SavingThrowProficiencies")]
        public string RawSavingThrowProficiencies
        {
            get => rawSavingThrowProficiencies;
            set
            {
                SetField(ref rawSavingThrowProficiencies, value);
                OnPropertyChanged(nameof(SavingThrowProficiencies));
            }
        }

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

        private string rawEquipmentProficiencies;
        [XmlAttribute("EquipmentProficiencies")]
        public string RawEquipmentProficiencies
        {
            get => rawEquipmentProficiencies;
            set
            {
                SetField(ref rawEquipmentProficiencies, value);
                OnPropertyChanged(nameof(EquipmentProficiencies));
            }
        }

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
        public BindingList<LevelModifier> LevelModifiers { get; } = new BindingList<LevelModifier>();

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
        public BindingList<CharacterClass> AllClasses { get; } = new BindingList<CharacterClass>();

        protected override IEnumerable<CharacterClass> EnumerateItems()
        {
            return AllClasses;
        }
    }
}
