using RPGSystem.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static RPGSystem.Utils;

namespace RPGSystem.Characters
{
    public class CharacterInstance : IdentifiableItem
    {
        public static CharacterInstance RollNew(string name, int level = 1, string race = "Human", string charClass = "Fighter")
        {
            CharacterInstance instance = new CharacterInstance
            {
                Identifier = Guid.NewGuid().ToString(),
                DisplayName = name,
                Level = level,
                RaceIdentifier = race,
                CharacterClassIdentifier = charClass,
                SizeIdentifier = GameConfiguration.CharacterSizes.DefaultSize
            };
            instance.RollBasedOnRace();
            return instance;
        }

        public CharacterInstance()
        {
        }

        [XmlAttribute]
        public int HitPoints { get; set; } // TODO take into account consitution p177

        [XmlAttribute]
        public int ExperiencePoints { get; set; }

        [XmlAttribute]
        public int Strength { get; set; }

        [XmlAttribute]
        public int Dexterity { get; set; }

        [XmlAttribute]
        public int Constitution { get; set; }

        [XmlAttribute]
        public int Intelligence { get; set; }

        [XmlAttribute]
        public int Wisdom { get; set; }

        [XmlAttribute]
        public int Charisma { get; set; }

        [XmlAttribute]
        public int Level { get; set; }

        [XmlAttribute]
        public int Age { get; set; }

        /// <summary>
        /// Height in inches
        /// </summary>
        [XmlAttribute]
        public int ActualHeight { get; set; }

        /// <summary>
        /// Weight in lbs
        /// </summary>
        [XmlAttribute]
        public int ActualWeight { get; set; }

        [XmlAttribute]
        public string SizeIdentifier { get; set; }

        [XmlIgnore]
        public CharacterSize Size
        {
            get
            {
                return GameConfiguration.CharacterSizes?.Find(SizeIdentifier);
            }
        }

        [XmlIgnore]
        protected double LiftCarryModifier
        {
            get
            {
                var size = Size;
                if (size != null)
                {
                    return size.LiftCarryModifier;
                }
                return 1;
            }
        }

        [XmlAttribute("GenderIdentity")]
        public string RawGenderIdentity { get; set; }

        [XmlIgnore]
        public IEnumerable<Gender> GenderIdentity
        {
            get
            {
                foreach (string genderId in ListFromCommaSeparated(RawGenderIdentity))
                {
                    Gender found = GameConfiguration.Genders?.Find(genderId);
                    if (genderId != null)
                    {
                        yield return found;
                    }
                }
            }
        }

        [XmlAttribute("AttractedTo")]
        public string RawAttractedTo { get; set; }

        [XmlIgnore]
        public IEnumerable<Gender> AttractedTo
        {
            get
            {
                foreach (string genderId in ListFromCommaSeparated(RawAttractedTo))
                {
                    Gender found = GameConfiguration.Genders?.Find(genderId);
                    if (genderId != null)
                    {
                        yield return found;
                    }
                }
            }
        }

        [XmlAttribute("CharacterClass")]
        public string CharacterClassIdentifier { get; set; }

        [XmlIgnore]
        public CharacterClass CharacterClass
        {
            get
            {
                return GameConfiguration.CharacterClasses?.Find(CharacterClassIdentifier);
            }
        }

        [XmlAttribute("Race")]
        public string RaceIdentifier { get; set; }

        [XmlIgnore]
        public Race Race
        {
            get
            {
                return GameConfiguration.Races?.Find(RaceIdentifier);
            }
        }

        [XmlAttribute("Alignment")]
        public string AlignmentIdentifier { get; set; }

        [XmlIgnore]
        public Alignment Alignment
        {
            get
            {
                return GameConfiguration.Alignments?.Find(AlignmentIdentifier);
            }
        }

        [XmlElement]
        public Inventory Inventory { get; } = new Inventory();

        [XmlIgnore]
        public int HitPointMaximum
        {
            get
            {
                if (Level <= 1)
                {
                    // TODO
                }
                return 0;
            }
        }

        [XmlIgnore]
        public int ArmorClass
        {
            get
            {
                // TODO account for equipped armour / shield and proficiency - see chapter 5
                return 10 + Dexterity;
            }
        }

        [XmlIgnore]
        public int ProficiencyBonus
        {
            get
            {
                return CharacterClass?.ProficiencyBonusForLevel(Level) ?? 0;
            }
        }

        /// <summary>
        /// The weight in lbs you are able to carry over a distance
        /// </summary>
        public int CarryingCapacity
        {
            get { return Convert.ToInt32(Strength * 15 * LiftCarryModifier); }
        }

        /// <summary>
        /// The weight in lbs at which point you are encumbered while carrying
        /// </summary>
        public int EncumberedCarryLimit
        {
            get { return Convert.ToInt32(Strength * 5 * LiftCarryModifier); }
        }

        /// <summary>
        /// The weight in lbs at which point you are heavily encumbered while carrying
        /// </summary>
        public int HeavilyEncumberedCarryLimit
        {
            get { return Convert.ToInt32(Strength * 10 * LiftCarryModifier); }
        }

        /// <summary>
        /// The weight in lbs you are able to push, drag or lift
        /// </summary>
        public int PushDragLiftLimit
        {
            get { return Convert.ToInt32(Strength * 30 * LiftCarryModifier); }
        }

        public int PassiveCheck(Ability ability, bool advantage = false, bool disadvantage = false)
        {
            int score = 0;
            if (advantage == disadvantage)
            {
                score = 10;
            }
            else if (advantage)
            {
                score = 15;
            }
            else
            {
                score = 5;
            }
            score += AbilityModifier(ability);
            return score;
        }

        public int SavingThrow(Ability ability)
        {
            int score = Dice.Roll(20);
            score += AbilityModifier(ability);
            var charClass = CharacterClass;
            if (charClass != null && charClass.SavingThrowProficiencies.Contains(ability))
            {
                score += charClass.ProficiencyBonusForLevel(Level);
            }
            return score;
        }

        /// <summary>
        /// Compare result to difficulty class to determine success or failure
        /// </summary>
        public int AbilityCheck(Ability ability, bool advantage = false, bool disadvantage = false, bool applyProficiencyBonus= false)
        {
            int score = 0;
            if (advantage == disadvantage)
            {
                score = Dice.Roll(20);
            }
            else if (advantage)
            {
                score = Dice.RollAdvantage(20);
            }
            else
            {
                score = Dice.RollDisadvantage(20);
            }
            score += AbilityModifier(ability);
            if (applyProficiencyBonus)
            {
                var charClass = CharacterClass;
                if (charClass != null && charClass.PrimaryAbilities.Contains(ability))
                {
                    score += charClass.ProficiencyBonusForLevel(Level);
                }
            }
            return score;
        }

        public int AbilityModifier(Ability ability)
        {
            return AbilityModifierFromScore(AbilityScore(ability));
        }

        public int AbilityScore(Ability ability)
        {
            int raceModifier = Race?.AbilityModifier(ability) ?? 0;
            switch (ability)
            {
                case Ability.Strength:
                    return Strength + raceModifier;
                case Ability.Dexterity:
                    return Dexterity + raceModifier;
                case Ability.Constitution:
                    return Constitution + raceModifier;
                case Ability.Intelligence:
                    return Intelligence + raceModifier;
                case Ability.Wisdom:
                    return Wisdom + raceModifier;
                case Ability.Charisma:
                    return Charisma + raceModifier;
            }
            return 0;
        }

        public bool RollBasedOnRace()
        {
            var race = Race;
            if (race != null)
            {
                int additionalHeight = Dice.Roll(race.HeightModifier);
                ActualHeight = race.BaseHeight + additionalHeight;
                ActualWeight = race.BaseWeight + (additionalHeight * Dice.Roll(race.WeightModifier));
                Age = RandomSingleton.Next(race.MaturityAge * 3 / 4, race.TypicalMaxAge);
                return true;
            }
            return false;
        }

        public ChallengeResult Attempt(Challenge challenge)
        {
            if (challenge != null)
            {
                return GetResult(AbilityCheck(challenge.AbilityRequired, 
                    challenge.CharacterHasAdvantage, challenge.CharacterHasDisadvantage), 
                    challenge.DifficultyClass);
            }
            return ChallengeResult.Tie;
        }
    }
}
