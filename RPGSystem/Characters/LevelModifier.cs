using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPGSystem.Utils;

namespace RPGSystem.Characters
{
    public class LevelModifier : NotifyPropertyChangedBase, IComparable
    {
        private int cantripsKnown;
        [XmlAttribute]
        public int CantripsKnown { get => cantripsKnown; set => SetField(ref cantripsKnown, value); }

        private int invocationsKnown;
        [XmlAttribute]
        public int InvocationsKnown { get => invocationsKnown; set => SetField(ref invocationsKnown, value); }

        private int kiPoints;
        [XmlAttribute]
        public int KiPoints { get => kiPoints; set => SetField(ref kiPoints, value); }

        private int level;
        [XmlAttribute]
        public int Level { get => level; set => SetField(ref level, value); }

        private string martialArts;
        [XmlAttribute]
        public string MartialArts { get => martialArts; set => SetField(ref martialArts, value); }

        private int proficiencyBonus;
        [XmlAttribute]
        public int ProficiencyBonus { get => proficiencyBonus; set => SetField(ref proficiencyBonus, value); }

        private int rageDamage;
        [XmlAttribute]
        public int RageDamage { get => rageDamage; set => SetField(ref rageDamage, value); }

        private int rages;
        [XmlAttribute]
        public int Rages { get => rages; set => SetField(ref rages, value); }

        private string rawFeatures;
        [XmlAttribute("Features")]
        public string RawFeatures { get => rawFeatures; set => SetField(ref rawFeatures, value); }

        private string rawSpellSlots;
        [XmlAttribute("SpellSlots")]
        public string RawSpellSlots { get => rawSpellSlots; set => SetField(ref rawSpellSlots, value); }

        private int slotLevel;
        [XmlAttribute]
        public int SlotLevel { get => slotLevel; set => SetField(ref slotLevel, value); }

        private string sneakAttack;
        [XmlAttribute]
        public string SneakAttack { get => sneakAttack; set => SetField(ref sneakAttack, value); }

        private int sorceryPoints;
        [XmlAttribute]
        public int SorceryPoints { get => sorceryPoints; set => SetField(ref sorceryPoints, value); }

        private int spellsKnown;
        [XmlAttribute]
        public int SpellsKnown { get => spellsKnown; set => SetField(ref spellsKnown, value); }

        private int unarmoredMovement;
        [XmlAttribute]
        public int UnarmoredMovement { get => unarmoredMovement; set => SetField(ref unarmoredMovement, value); }

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

        public static BindingList<LevelModifier> MergeLists(BindingList<LevelModifier> childList, BindingList<LevelModifier> parentList)
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
            var result = new BindingList<LevelModifier>();
            AddRange(result, newList);
            return result;
        }
    }
}
