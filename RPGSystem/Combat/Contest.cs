using RPGSystem.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSystem.Combat
{
    public class Contest : Challenge
    {
        public CharacterInstance Foe { get; set; }

        public bool FoeHasAdvantage { get; set; }

        public bool FoeHasDisadvantage { get; set; }

        public override int DifficultyClass
        {
            get
            {
                if (Foe != null)
                {
                    return Foe.AbilityCheck(AbilityRequired, FoeHasAdvantage, FoeHasDisadvantage);
                }
                return 0;
            }
            set
            {
                // ignore
            }
        }
    }
}
