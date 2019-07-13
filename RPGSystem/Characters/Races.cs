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
    public class Race : IdentifiableItem
    {
        private int baseHeight;
        [XmlAttribute]
        public int BaseHeight { get => baseHeight; set => SetField(ref baseHeight, value); }

        private string heightModifier;
        [XmlAttribute]
        public string HeightModifier { get => heightModifier; set => SetField(ref heightModifier, value); }

        private int baseWeight;
        [XmlAttribute]
        public int BaseWeight { get => baseWeight; set => SetField(ref baseWeight, value); }

        private string weightModifier;
        [XmlAttribute]
        public string WeightModifier { get => weightModifier; set => SetField(ref weightModifier, value); }

        private int maturityAge;
        [XmlAttribute]
        public int MaturityAge { get => maturityAge; set => SetField(ref maturityAge, value); }

        private int typicalMaxAge;
        [XmlAttribute]
        public int TypicalMaxAge { get => typicalMaxAge; set => SetField(ref typicalMaxAge, value); }

        private int baseWalkingSpeed;
        [XmlAttribute]
        public int BaseWalkingSpeed { get => baseWalkingSpeed; set => SetField(ref baseWalkingSpeed, value); }

        private int strengthModifier;
        [XmlAttribute]
        public int StrengthModifier { get => strengthModifier; set => SetField(ref strengthModifier, value); }

        private int dexterityModifier;
        [XmlAttribute]
        public int DexterityModifier { get => dexterityModifier; set => SetField(ref dexterityModifier, value); }

        private int constitutionModifier;
        [XmlAttribute]
        public int ConstitutionModifier { get => constitutionModifier; set => SetField(ref constitutionModifier, value); }

        private int intelligenceModifier;
        [XmlAttribute]
        public int IntelligenceModifier { get => intelligenceModifier; set => SetField(ref intelligenceModifier, value); }

        private int wisdomModifier;
        [XmlAttribute]
        public int WisdomModifier { get => wisdomModifier; set => SetField(ref wisdomModifier, value); }

        private int charismaModifier;
        [XmlAttribute]
        public int CharismaModifier { get => charismaModifier; set => SetField(ref charismaModifier, value); }

        private int hitPointMaxModifier;
        [XmlAttribute]
        public int HitPointMaxModifier { get => hitPointMaxModifier; set => SetField(ref hitPointMaxModifier, value); }

        private int hitPointMaxLevelModifier;
        [XmlAttribute]
        public int HitPointMaxLevelModifier { get => hitPointMaxLevelModifier; set => SetField(ref hitPointMaxLevelModifier, value); }

        private string rawEquipmentProficiencies;
        [XmlAttribute("EquipmentProficiencies")]
        public string RawEquipmentProficiencies { get => rawEquipmentProficiencies; set => SetField(ref rawEquipmentProficiencies, value); }

        private string rawTypicalAlignment;
        [XmlAttribute("TypicalAlignment")]
        public string RawTypicalAlignment { get => rawTypicalAlignment; set => SetField(ref rawTypicalAlignment, value); }

        [XmlIgnore]
        public IEnumerable<string> TypicalAlignment
        {
            get
            {
                return ListFromCommaSeparated(RawTypicalAlignment);
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

        private string rawLanguages;
        [XmlAttribute("Languages")]
        public string RawLanguages { get => rawLanguages; set => SetField(ref rawLanguages, value); }

        [XmlIgnore]
        public IEnumerable<string> Languages
        {
            get
            {
                return ListFromCommaSeparated(RawLanguages);
            }
        }

        [XmlArray]
        [XmlArrayItem("LevelModifier")]
        public BindingList<LevelModifier> LevelModifiers { get; } = new BindingList<LevelModifier>();

        [XmlArray]
        [XmlArrayItem("SubRace")]
        public BindingList<Race> SubRaces { get; } = new BindingList<Race>();

        private Race superRace;
        [XmlIgnore]
        public Race SuperRace { get => superRace; set => SetField(ref superRace, value); }

        public int AbilityModifier(Ability ability)
        {
            switch (ability)
            {
                case Ability.Strength:
                    return StrengthModifier;
                case Ability.Dexterity:
                    return DexterityModifier;
                case Ability.Constitution:
                    return ConstitutionModifier;
                case Ability.Intelligence:
                    return IntelligenceModifier;
                case Ability.Wisdom:
                    return WisdomModifier;
                case Ability.Charisma:
                    return CharismaModifier;
            }
            return 0;
        }

        public void CopyTo(Race otherRace, bool merge = false)
        {
            if (otherRace != null)
            {
                if (merge)
                {
                    otherRace.BaseHeight = OverrideIfNullOrDefault<int>(otherRace.BaseHeight, BaseHeight);
                    otherRace.BaseWalkingSpeed = OverrideIfNullOrDefault<int>(otherRace.BaseWalkingSpeed, BaseWalkingSpeed);
                    otherRace.BaseWeight = OverrideIfNullOrDefault<int>(otherRace.BaseWeight, BaseWeight);
                    otherRace.CharismaModifier = OverrideIfNullOrDefault<int>(otherRace.CharismaModifier, CharismaModifier);
                    otherRace.ConstitutionModifier = OverrideIfNullOrDefault<int>(otherRace.ConstitutionModifier, ConstitutionModifier);
                    otherRace.DexterityModifier = OverrideIfNullOrDefault<int>(otherRace.DexterityModifier, DexterityModifier);
                    otherRace.DisplayName = OverrideIfNullOrDefault<string>(otherRace.DisplayName, DisplayName);
                    otherRace.HeightModifier = OverrideIfNullOrDefault<string>(otherRace.HeightModifier, HeightModifier);
                    otherRace.HitPointMaxLevelModifier = OverrideIfNullOrDefault<int>(otherRace.HitPointMaxLevelModifier, HitPointMaxLevelModifier);
                    otherRace.HitPointMaxModifier = OverrideIfNullOrDefault<int>(otherRace.HitPointMaxModifier, HitPointMaxModifier);
                    otherRace.Identifier = OverrideIfNullOrDefault<string>(otherRace.Identifier, Identifier);
                    otherRace.IntelligenceModifier = OverrideIfNullOrDefault<int>(otherRace.IntelligenceModifier, IntelligenceModifier);
                    var mergedList = LevelModifier.MergeLists(otherRace.LevelModifiers, LevelModifiers);
                    otherRace.LevelModifiers.Clear();
                    AddRange(otherRace.LevelModifiers, mergedList);
                    otherRace.MaturityAge = OverrideIfNullOrDefault<int>(otherRace.MaturityAge, MaturityAge);
                    otherRace.RawEquipmentProficiencies = MergeUniqueCommaSeparated(otherRace.RawEquipmentProficiencies, RawEquipmentProficiencies);
                    otherRace.RawLanguages = MergeUniqueCommaSeparated(otherRace.RawLanguages, RawLanguages);
                    otherRace.RawTypicalAlignment = OverrideIfNullOrDefault<string>(otherRace.RawTypicalAlignment, RawTypicalAlignment);
                    otherRace.StrengthModifier = OverrideIfNullOrDefault<int>(otherRace.StrengthModifier, StrengthModifier);
                    // don't merge SubRaces and SuperRace
                    otherRace.TypicalMaxAge = OverrideIfNullOrDefault<int>(otherRace.TypicalMaxAge, TypicalMaxAge);
                    otherRace.WeightModifier = OverrideIfNullOrDefault<string>(otherRace.WeightModifier, WeightModifier);
                    otherRace.WisdomModifier = OverrideIfNullOrDefault<int>(otherRace.WisdomModifier, WisdomModifier);
                }
                else
                {
                    otherRace.BaseHeight = BaseHeight;
                    otherRace.BaseWalkingSpeed = BaseWalkingSpeed;
                    otherRace.BaseWeight = BaseWeight;
                    otherRace.CharismaModifier = CharismaModifier;
                    otherRace.ConstitutionModifier = ConstitutionModifier;
                    otherRace.DexterityModifier = DexterityModifier;
                    otherRace.DisplayName = DisplayName;
                    otherRace.HeightModifier = HeightModifier;
                    otherRace.HitPointMaxLevelModifier = HitPointMaxLevelModifier;
                    otherRace.HitPointMaxModifier = HitPointMaxModifier;
                    otherRace.Identifier = Identifier;
                    otherRace.IntelligenceModifier = IntelligenceModifier;
                    otherRace.LevelModifiers.Clear();
                    AddRange(otherRace.LevelModifiers, LevelModifiers);
                    otherRace.MaturityAge = MaturityAge;
                    otherRace.RawEquipmentProficiencies = RawEquipmentProficiencies;
                    otherRace.RawLanguages = RawLanguages;
                    otherRace.RawTypicalAlignment = RawTypicalAlignment;
                    otherRace.StrengthModifier = StrengthModifier;
                    otherRace.SubRaces.Clear();
                    AddRange(otherRace.SubRaces, SubRaces);
                    otherRace.SuperRace = SuperRace;
                    otherRace.TypicalMaxAge = TypicalMaxAge;
                    otherRace.WeightModifier = WeightModifier;
                    otherRace.WisdomModifier = WisdomModifier;
                }
            }
        }

        public void DefaultFromSuperRace()
        {
            if (SuperRace != null)
            {
                SuperRace.CopyTo(this, true);
            }
        }
    }

    public class Races : IdentifiableItemCollection<Race>
    {
        [XmlArray]
        [XmlArrayItem("Race")]
        public BindingList<Race> AllRaces { get; } = new BindingList<Race>();

        protected override IEnumerable<Race> EnumerateItems()
        {
            return this.AllRaces;
        }

        protected override void PostLoad(Race item)
        {
            base.PostLoad(item);
            if (item.SubRaces.Count > 0)
            {
                foreach (var subRace in item.SubRaces)
                {
                    subRace.SuperRace = item;
                    subRace.DefaultFromSuperRace();
                    SetLookup(subRace.Identifier, subRace);
                }
            }
        }
    }
}
