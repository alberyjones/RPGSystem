using RPGSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RPGApp.View
{
    public class RaceComboBox : ComboBox
    {
        public RaceComboBox()
        {
            if (GameConfiguration.Races != null && GameConfiguration.Races.AllRaces != null)
            {
                foreach (var race in GameConfiguration.Races.AllRaces)
                {
                    if (race.SubRaces.Count > 0)
                    {
                        foreach (var subRace in race.SubRaces)
                        {
                            Items.Add(subRace);
                        }
                    }
                    else
                    {
                        Items.Add(race);
                    }
                }
            }
        }
    }
}
