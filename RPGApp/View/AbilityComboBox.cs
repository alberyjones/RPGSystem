using RPGSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RPGApp.View
{
    public class AbilityComboBox : ComboBox
    {
        public AbilityComboBox()
        {
            foreach (var ability in Enum.GetValues(typeof(Ability)))
            {
                Items.Add(ability);
            }
        }
    }
}
