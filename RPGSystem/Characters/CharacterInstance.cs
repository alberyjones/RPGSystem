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

        // TODO take into account consitution p177
        private int hitPoints;
        [XmlAttribute]
        public int HitPoints { get => hitPoints; set => SetField(ref hitPoints, value); }

        private int experiencePoints;
        [XmlAttribute]
        public int ExperiencePoints { get => experiencePoints; set => SetField(ref experiencePoints, value); }

        private int strength;
        [XmlAttribute]
        public int Strength { get => strength; set => SetField(ref strength, value); }

        private int dexterity;
        [XmlAttribute]
        public int Dexterity { get => dexterity; set => SetField(ref dexterity, value); }

        private int constitution;
        [XmlAttribute]
        public int Constitution { get => constitution; set => SetField(ref constitution, value); }

        private int intelligence;
        [XmlAttribute]
        public int Intelligence { get => intelligence; set => SetField(ref intelligence, value); }

        private int wisdom;
        [XmlAttribute]
        public int Wisdom { get => wisdom; set => SetField(ref wisdom, value); }

        private int charisma;
        [XmlAttribute]
        public int Charisma { get => charisma; set => SetField(ref charisma, value); }

        private int level;
        [XmlAttribute]
        public int Level { get => level; set => SetField(ref level, value); }

        private int age;
        [XmlAttribute]
        public int Age { get => age; set => SetField(ref age, value); }

        private int actualHeight;
        /// <summary>
        /// Height in inches
        /// </summary>
        [XmlAttribute]
        public int ActualHeight { get => actualHeight; set => SetField(ref actualHeight, value); }

        private int actualWeight;
        /// <summary>
        /// Weight in lbs
        /// </summary>
        [XmlAttribute]
        public int ActualWeight { get => actualWeight; set => SetField(ref actualWeight, value); }

        private string sizeIdentifier;
        [XmlAttribute]
        public string SizeIdentifier
        {
            get => sizeIdentifier;
            set
            {
                SetField(ref sizeIdentifier, value);
                OnPropertyChanged(nameof(Size));
            }
        }

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

        private string rawGenderIdentity;
        [XmlAttribute("GenderIdentity")]
        public string RawGenderIdentity {
            get => rawGenderIdentity;
            set
            {
                SetField(ref rawGenderIdentity, value);
                OnPropertyChanged(nameof(GenderIdentity));
            }
        }

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

        private string rawAttractedTo;
        [XmlAttribute("AttractedTo")]
        public string RawAttractedTo
        {
            get => rawAttractedTo;
            set
            {
                SetField(ref rawAttractedTo, value);
                OnPropertyChanged(nameof(AttractedTo));
            }
        }

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

        private string characterClassIdentifier;
        [XmlAttribute("CharacterClass")]
        public string CharacterClassIdentifier
        {
            get => characterClassIdentifier; 
            set
            {
                SetField(ref characterClassIdentifier, value);
                OnPropertyChanged(nameof(CharacterClass));
            }
        }

        [XmlIgnore]
        public CharacterClass CharacterClass
        {
            get
            {
                return GameConfiguration.CharacterClasses?.Find(CharacterClassIdentifier);
            }
        }

        private string raceIdentifier;
        [XmlAttribute("Race")]
        public string RaceIdentifier
        {
            get => raceIdentifier;
            set
            {
                SetField(ref raceIdentifier, value);
                OnPropertyChanged(nameof(Race));
            }
        }

        [XmlIgnore]
        public Race Race
        {
            get
            {
                return GameConfiguration.Races?.Find(RaceIdentifier);
            }
        }

        private string alignmentIdentifier;
        [XmlAttribute("Alignment")]
        public string AlignmentIdentifier
        {
            get => alignmentIdentifier;
            set
            {
                SetField(ref alignmentIdentifier, value);
                OnPropertyChanged(nameof(Alignment));
            }
        }

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
