using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPGSystem.Utils;

namespace RPGSystem.Characters
{
    public class LevelModifier : IComparable
    {
        [XmlAttribute]
        public int CantripsKnown { get; set; }

        [XmlAttribute]
        public int InvocationsKnown { get; set; }

        [XmlAttribute]
        public int KiPoints { get; set; }

        [XmlAttribute]
        public int Level { get; set; }

        [XmlAttribute]
        public string MartialArts { get; set; }

        [XmlAttribute]
        public int ProficiencyBonus { get; set; }

        [XmlAttribute]
        public int RageDamage { get; set; }

        [XmlAttribute]
        public int Rages { get; set; }

        [XmlAttribute("Features")]
        public string RawFeatures { get; set; }

        [XmlAttribute("SpellSlots")]
        public string RawSpellSlots { get; set; }

        [XmlAttribute]
        public int SlotLevel { get; set; }

        [XmlAttribute]
        public string SneakAttack { get; set; }

        [XmlAttribute]
        public int SorceryPoints { get; set; }

        [XmlAttribute]
        public int SpellsKnown { get; set; }

        [XmlAttribute]
        public int UnarmoredMovement { get; set; }

        public void CopyTo(LevelModifier otherModifier, bool merge = false)
        {
            if (otherModifier != null)
            {
                if (merge)
                {
                    otherModifier.CantripsKnown = OverrideIfNullOrDefault<int>(otherModifier.CantripsKnown, CantripsKnown);
                    otherModifier.InvocationsKnown = OverrideIfNullOrDefault<int>(otherModifier.InvocationsKnown, InvocationsKnown);
                    otherModifier.KiPoints = OverrideIfNullOrDefault<int>(otherModifier.KiPoints, KiPoints);
                    otherModifier.Level = OverrideIfNullOrDefault<int>(otherModifier.Level, Level);
                    otherModifier.MartialArts = OverrideIfNullOrDefault<string>(otherModifier.MartialArts, MartialArts);
                    otherModifier.ProficiencyBonus = OverrideIfNullOrDefault<int>(otherModifier.ProficiencyBonus, ProficiencyBonus);
                    otherModifier.RageDamage = OverrideIfNullOrDefault<int>(otherModifier.RageDamage, RageDamage);
                    otherModifier.Rages = OverrideIfNullOrDefault<int>(otherModifier.Rages, Rages);
                    otherModifier.RawFeatures = MergeUniqueCommaSeparated(otherModifier.RawFeatures, RawFeatures);
                    otherModifier.RawSpellSlots = MergeUniqueCommaSeparated(otherModifier.RawSpellSlots, RawSpellSlots);
                    otherModifier.SlotLevel = OverrideIfNullOrDefault<int>(otherModifier.SlotLevel, SlotLevel);
                    otherModifier.SneakAttack = OverrideIfNullOrDefault<string>(otherModifier.SneakAttack, SneakAttack);
                    otherModifier.SorceryPoints = OverrideIfNullOrDefault<int>(otherModifier.SorceryPoints, SorceryPoints);
                    otherModifier.SpellsKnown = OverrideIfNullOrDefault<int>(otherModifier.SpellsKnown, SpellsKnown);
                    otherModifier.UnarmoredMovement = OverrideIfNullOrDefault<int>(otherModifier.UnarmoredMovement, UnarmoredMovement);
                }
                else
                {
                    otherModifier.CantripsKnown = CantripsKnown;
                    otherModifier.InvocationsKnown = InvocationsKnown;
                    otherModifier.KiPoints = KiPoints;
                    otherModifier.Level = Level;
                    otherModifier.MartialArts = MartialArts;
                    otherModifier.ProficiencyBonus = ProficiencyBonus;
                    otherModifier.RageDamage = RageDamage;
                    otherModifier.Rages = Rages;
                    otherModifier.RawFeatures = RawFeatures;
                    otherModifier.RawSpellSlots = RawSpellSlots;
                    otherModifier.SlotLevel = SlotLevel;
                    otherModifier.SneakAttack = SneakAttack;
                    otherModifier.SorceryPoints = SorceryPoints;
                    otherModifier.SpellsKnown = SpellsKnown;
                    otherModifier.UnarmoredMovement = UnarmoredMovement;
                }
            }
        }

        public int CompareTo(object obj)
        {
            LevelModifier otherMod = obj as LevelModifier;
            if (otherMod != null)
            {
                return Level.CompareTo(otherMod.Level);
            }
            return -1;
        }

        public static List<LevelModifier> MergeLists(List<LevelModifier> childList, List<LevelModifier> parentList)
        {
            if (childList == null)
            {
                return parentList;
            }
            if (parentList == null)
            {
                return childList;
            }
            Dictionary<int, LevelModifier> modifiers = new Dictionary<int, LevelModifier>();
            foreach (var item in childList)
            {
                modifiers[item.Level] = item;
            }
            foreach (var item in parentList)
            {
                if (modifiers.ContainsKey(item.Level))
                {
                    item.CopyTo(modifiers[item.Level], true);
                }
                else
                {
                    modifiers[item.Level] = item;
                }
            }
            var newList = new List<LevelModifier>(modifiers.Values);
            newList.Sort();
            return newList;
        }
    }
}
