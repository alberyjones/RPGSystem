using RPGSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RPGApp.View
{
    public class AlignmentComboBox : ComboBox
    {
        public AlignmentComboBox()
        {
            if (GameConfiguration.Alignments != null && GameConfiguration.Alignments.AllAlignments != null)
            {
                foreach (var alignment in GameConfiguration.Alignments.AllAlignments)
                {
                    Items.Add(alignment);
                }
            }
        }
    }
}
