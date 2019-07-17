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
    public class CharacterInstanceViewModel : ViewModelBase
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

        private bool canEdit;
        public bool CanEdit
        {
            get => canEdit;
            set
            {
                if (SetField(ref canEdit, value))
                {
                    // we can no longer edit, switch out of edit mode if needed
                    if (!canEdit) IsEditing = false;
                }
            }
        }

        private bool isEditing;
        public bool IsEditing
        {
            get => isEditing;
            set
            {
                if (SetField(ref isEditing, value))
                {
                    OnPropertyChanged(nameof(IsReadOnly));
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

        public bool IsReadOnly
        {
            get { return !IsEditing; }
        }

        public ICommand BeginEdit { get; private set; }

        public ICommand EndEdit { get; private set; }

        public ICommand RollRaceAttributes { get; private set; }

        public ICommand RollClassAttributes { get; private set; }

        public ICommand SwitchAbilityScores { get; private set; }

        public CharacterInstanceViewModel()
        {
            BeginEdit = new CustomCommand(DoBeginEdit, CanBeginEdit);
            EndEdit = new CustomCommand(DoEndEdit, IsEditingCharacter);
            RollRaceAttributes = new CustomCommand(DoRollRaceAttributes, IsEditingCharacter);
            RollClassAttributes = new CustomCommand(DoRollClassAttributes, IsEditingCharacter);
            SwitchAbilityScores = new CustomCommand(DoSwitchAbilityScores, IsEditingCharacter);
        }

        private bool CanBeginEdit(object parameters)
        {
            return CanEdit && !IsEditing && Character != null;
        }

        private bool IsEditingCharacter(object parameters)
        {
            return IsEditing && Character != null;
        }

        private void DoRollRaceAttributes(object parameters)
        {
            Character?.RollAttributesBasedOnRace();
        }

        private void DoRollClassAttributes(object parameters)
        {
            Character?.RollAttributesBasedOnClass();
        }

        private void DoBeginEdit(object parameters)
        {
            IsEditing = true;
        }

        private void DoEndEdit(object parameters)
        {
            IsEditing = false;
        }

        private void DoSwitchAbilityScores(object parameters)
        {
            Character?.SwitchAbilityScores(AbilityFrom, AbilityTo);
        }
    }
}
