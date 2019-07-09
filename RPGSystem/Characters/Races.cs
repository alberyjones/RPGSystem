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
    public class Race : IdentifiableItem
    {
        [XmlAttribute]
        public int BaseHeight { get; set; }

        [XmlAttribute]
        public string HeightModifier { get; set; }

        [XmlAttribute]
        public int BaseWeight { get; set; }

        [XmlAttribute]
        public string WeightModifier { get; set; }

        [XmlAttribute]
        public int MaturityAge { get; set; }

        [XmlAttribute]
        public int TypicalMaxAge { get; set; }

        [XmlAttribute]
        public int BaseWalkingSpeed { get; set; }

        [XmlAttribute]
        public int StrengthModifier { get; set; }

        [XmlAttribute]
        public int DexterityModifier { get; set; }

        [XmlAttribute]
        public int ConstitutionModifier { get; set; }

        [XmlAttribute]
        public int IntelligenceModifier { get; set; }

        [XmlAttribute]
        public int WisdomModifier { get; set; }

        [XmlAttribute]
        public int CharismaModifier { get; set; }

        [XmlAttribute]
        public int HitPointMaxModifier { get; set; }

        [XmlAttribute]
        public int HitPointMaxLevelModifier { get; set; }

        [XmlAttribute("EquipmentProficiencies")]
        public string RawEquipmentProficiencies { get; set; }

        [XmlAttribute("TypicalAlignment")]
        public string RawTypicalAlignment { get; set; }

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

        [XmlAttribute("Languages")]
        public string RawLanguages { get; set; }

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
        public List<LevelModifier> LevelModifiers { get; } = new List<LevelModifier>();

        [XmlArray]
        [XmlArrayItem("SubRace")]
        public List<Race> SubRaces { get; } = new List<Race>();

        [XmlIgnore]
        public Race SuperRace { get; set; }

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
                    otherRace.LevelModifiers.AddRange(mergedList);
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
                    otherRace.LevelModifiers.AddRange(LevelModifiers);
                    otherRace.MaturityAge = MaturityAge;
                    otherRace.RawEquipmentProficiencies = RawEquipmentProficiencies;
                    otherRace.RawLanguages = RawLanguages;
                    otherRace.RawTypicalAlignment = RawTypicalAlignment;
                    otherRace.StrengthModifier = StrengthModifier;
                    otherRace.SubRaces.Clear();
                    otherRace.SubRaces.AddRange(SubRaces);
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
        public List<Race> AllRaces { get; } = new List<Race>();

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
