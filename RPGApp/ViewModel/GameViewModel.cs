using RPGSystem;
using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private CharacterInstance selectedCharacter;
        public CharacterInstance SelectedCharacter
        {
            get => selectedCharacter;
            set => SetField(ref selectedCharacter, value);
        }
    }
}
