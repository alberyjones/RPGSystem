using RPGSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RPGApp.View
{
    public class CharacterClassComboBox : ComboBox
    {
        public CharacterClassComboBox()
        {
            if (GameConfiguration.CharacterClasses != null && GameConfiguration.CharacterClasses.AllClasses != null)
            {
                foreach (var charClass in GameConfiguration.CharacterClasses.AllClasses)
                {
                    Items.Add(charClass);
                }
            }
        }
    }
}
