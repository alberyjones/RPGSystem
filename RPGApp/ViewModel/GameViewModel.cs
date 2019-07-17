using RPGSystem;
using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGApp.ViewModel
{
    public class GameViewModel : EditableViewModel
    {
        private Game activeGame;
        public Game ActiveGame
        {
            get => activeGame;
            set => SetField(ref activeGame, value);
        }

        private CharacterInstanceViewModel selectedCharacterViewModel;
        public CharacterInstanceViewModel SelectedCharacterViewModel
        {
            get => selectedCharacterViewModel;
            set => SetField(ref selectedCharacterViewModel, value);
        }

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set => SetField(ref filePath, value);
        }

        public ICommand AddCharacter { get; private set; } 

        public ICommand LoadGame { get; private set; }

        public ICommand SaveGame { get; private set; }

        public GameViewModel()
        {
            SelectedCharacterViewModel = new CharacterInstanceViewModel();
            FilePath = "ExampleGame";
            CanEdit = true;

            LoadGame = new CustomCommand(DoLoadGame);
            SaveGame = new CustomCommand(DoSaveGame, IsActiveGameSet);
            AddCharacter = new CustomCommand(NewCharacter, IsEditingItem);
        }

        protected override bool CanBeginEdit(object parameters)
        {
            return base.CanBeginEdit(parameters) && ActiveGame != null;
        }

        protected override bool IsEditingItem(object parameters)
        {
            return base.IsEditingItem(parameters) && ActiveGame != null;
        }

        protected override void DoBeginEdit(object parameters)
        {
            base.DoBeginEdit(parameters);
            SelectedCharacterViewModel.CanEdit = true;
        }

        protected override void DoEndEdit(object parameters)
        {
            base.DoEndEdit(parameters);
            SelectedCharacterViewModel.CanEdit = false;
        }

        private bool IsActiveGameSet(object parameters)
        {
            return ActiveGame != null;
        }

        private void NewCharacter(object parameters)
        {
            var newChar = CharacterInstance.RollNew("New Character");
            ActiveGame.Characters.Add(newChar);
            SelectedCharacterViewModel.Character = newChar;
            SelectedCharacterViewModel.BeginEdit.Execute(null);
        }

        private void DoLoadGame(object parameters)
        {
            if (!String.IsNullOrEmpty(FilePath))
            {
                if (GameConfiguration.DataLoader.TryLoad<Game>(FilePath, out var game))
                {
                    ActiveGame = game;
                }
                else
                {
                    var g = new Game();
                    g.DisplayName = FilePath;
                    g.Description = $"Description of {FilePath}";
                    ActiveGame = g;
                }
            }
        }

        private void DoSaveGame(object parameters)
        {
            if (ActiveGame != null && !String.IsNullOrEmpty(FilePath))
            {
                GameConfiguration.DataLoader.Save<Game>(ActiveGame, FilePath);
            }
        }
    }
}
