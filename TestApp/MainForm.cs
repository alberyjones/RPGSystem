using RPGSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            int a = GameConfiguration.CharacterClasses.AllClasses.Count;
            a = GameConfiguration.EquipmentTypes.AllEquipment.Count;
            foreach(var race in GameConfiguration.Races.AllRaces)
            {
                if (race.SubRaces.Count > 0)
                {
                }
            }

        }
    }
}
