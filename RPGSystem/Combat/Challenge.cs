using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGSystem.Combat
{
    public class Challenge
    {
        /// <summary>
        /// 5  = Very Easy
        /// 10 = Easy
        /// 15 = Medium
        /// 20 = Hard
        /// 25 = Very Hard
        /// 30 = Nearly Impossible 
        /// </summary>
        public virtual int DifficultyClass { get; set; }

        public Ability AbilityRequired { get; set; }

        public bool CharacterHasAdvantage { get; set; }

        public bool CharacterHasDisadvantage { get; set; }

        public string Description { get; set; }
    }

    public enum ChallengeResult
    {
        Succeed,
        Tie,
        Fail
    }
}
