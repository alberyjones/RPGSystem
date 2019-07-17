using RPGSystem;
using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGApp.ViewModel
{
    public class CharacterInstanceViewModel : EditableViewModel
    {
        private CharacterInstance character;
        public CharacterInstance Character
        {
            get => character;
            set
            {
                if (SetField(ref character, value))
                {
                    // stop editing if the character changes
                    IsEditing = false;
                }
            }
        }

        private Ability abilityFrom;
        public Ability AbilityFrom
        {
            get => abilityFrom;
            set => SetField(ref abilityFrom, value);
        }

        private Ability abilityTo;
        public Ability AbilityTo
        {
            get => abilityTo;
            set => SetField(ref abilityTo, value);
        }

        public ICommand RollRaceAttributes { get; private set; }

        public ICommand RollClassAttributes { get; private set; }

        public ICommand SwitchAbilityScores { get; private set; }

        public CharacterInstanceViewModel()
        {
            RollRaceAttributes = new CustomCommand(DoRollRaceAttributes, IsEditingItem);
            RollClassAttributes = new CustomCommand(DoRollClassAttributes, IsEditingItem);
            SwitchAbilityScores = new CustomCommand(DoSwitchAbilityScores, IsEditingItem);
        }

        protected override bool CanBeginEdit(object parameters)
        {
            return base.CanBeginEdit(parameters) && Character != null;
        }

        protected override bool IsEditingItem(object parameters)
        {
            return base.IsEditingItem(parameters) && Character != null;
        }

        private void DoRollRaceAttributes(object parameters)
        {
            Character?.RollAttributesBasedOnRace();
        }

        private void DoRollClassAttributes(object parameters)
        {
            Character?.RollAttributesBasedOnClass();
        }

        private void DoSwitchAbilityScores(object parameters)
        {
            Character?.SwitchAbilityScores(AbilityFrom, AbilityTo);
        }
    }
}
