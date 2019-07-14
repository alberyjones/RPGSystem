using RPGSystem;
using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace RPGApp.Converter
{
    public class CharacterAbilityDisplayConverter : IValueConverter
    {
        private Ability ability;

        public CharacterAbilityDisplayConverter(Ability ability)
        {
            this.ability = ability;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CharacterInstance character)
            {
                int baseValue = character[ability];
                int modifier = character.Race.AbilityModifier(ability);
                if (modifier == 0)
                {
                    return $"{baseValue}";
                }
                else if (modifier > 0)
                {
                    return $"{baseValue} (+{modifier})";
                }
                return $"{baseValue} ({modifier})";
            }
            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CharacterStrengthDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterStrengthDisplayConverter() : base(Ability.Strength) { }
    }

    public class CharacterDexterityDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterDexterityDisplayConverter() : base(Ability.Dexterity) { }
    }

    public class CharacterConstitutionDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterConstitutionDisplayConverter() : base(Ability.Constitution) { }
    }

    public class CharacterIntelligenceDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterIntelligenceDisplayConverter() : base(Ability.Intelligence) { }
    }

    public class CharacterWisdomDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterWisdomDisplayConverter() : base(Ability.Wisdom) { }
    }

    public class CharacterCharismaDisplayConverter : CharacterAbilityDisplayConverter
    {
        public CharacterCharismaDisplayConverter() : base(Ability.Charisma) { }
    }
}
