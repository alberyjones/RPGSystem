using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGApp.ViewModel
{
    public class CharacterInstanceViewModel : ViewModelBase
    {
        private CharacterInstance character;
        public CharacterInstance Character
        {
            get => character;
            set => SetField(ref character, value);
        }
    }
}
