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
    public class GameViewModel : ViewModelBase
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

        public ICommand AddCharacter { get; private set; } 

        public ICommand LoadGame { get; private set; }

        public ICommand SaveGame { get; private set; }

        public GameViewModel()
        {
            SelectedCharacterViewModel = new CharacterInstanceViewModel();
            SelectedCharacterViewModel.CanEdit = true;

            LoadGame = new CustomCommand(DoLoadGame);
            SaveGame = new CustomCommand(DoSaveGame, IsActiveGameSet);
            AddCharacter = new CustomCommand(NewCharacter, IsActiveGameSet);
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
            if (GameConfiguration.DataLoader.TryLoad<Game>("ExampleGame", out var game))
            {
                ActiveGame = game;
            }
        }

        private void DoSaveGame(object parameters)
        {
            if (ActiveGame != null)
            {
                GameConfiguration.DataLoader.Save<Game>(ActiveGame, "ExampleGame");
            }
        }
    }
}
