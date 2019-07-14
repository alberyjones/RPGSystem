using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RPGSystem.Utils;

namespace RPGSystem
{
    public class Dice
    {
        //private static Dictionary<string, Roller> rollerCache = new Dictionary<string, Roller>();

        private Dice() { } // static methods only

        public static Roller CreateRoller(string roll)
        {
            //string key = Roller.CleanUpRollDetails(roll);
            //Roller rollObj = null;
            //if (!rollerCache.TryGetValue(key, out rollObj))
            //{
            //    rollObj = new Roller(key);
            //    rollerCache[key] = rollObj;
            //}
            //return rollObj;
            return new Roller(roll);
        }

        public static int RollBasicAbility()
        {
            int[] rolls = RollMultiple(4, 6, 1);
            int[] best3 = GetHighest(rolls, 3);
            return Sum(best3);
        }

        public static int RollAdvantage(int sides = 20)
        {
            return Math.Max(RandomSingleton.Next(1, sides), RandomSingleton.Next(1, sides));
        }

        public static int RollAdvantage(string roll)
        {
            var rollObj = CreateRoller(roll);
            return Math.Max(rollObj.Roll(), rollObj.Roll());
        }

        public static int RollDisadvantage(int sides = 20)
        {
            return Math.Min(RandomSingleton.Next(1, sides), RandomSingleton.Next(1, sides));
        }

        public static int RollDisadvantage(string roll)
        {
            var rollObj = CreateRoller(roll);
            return Math.Min(rollObj.Roll(), rollObj.Roll());
        }

        public static int Roll(string roll)
        {
            return CreateRoller(roll).Roll();
        }

        public static int Roll(int numSidesPerDie = 6, int numDicePerRoll = 1, int modifier = 0)
        {
            int roll = 0;
            for (int i = 0; i < numDicePerRoll; i++)
            {
                roll += RandomSingleton.Next(1, numSidesPerDie);
            }
            return roll + modifier;
        }

        public static int[] RollMultiple(int numRolls, int numSidesPerDie = 6, int numDicePerRoll = 1)
        {
            int[] rolls = new int[numRolls];
            for (int i=0; i< numRolls; i++)
            {
                rolls[i] = Roll(numSidesPerDie, numDicePerRoll);
            }
            return rolls;
        }

        public static int[] Sort(int[] rolls, bool ascending = true)
        {
            List<int> list = new List<int>(rolls);
            list.Sort();
            if (!ascending)
            {
                list.Reverse();
            }
            return list.ToArray();
        }

        public static int[] GetHighest(int[] rolls, int numberToReturn = 1)
        {
            int[] highestRolls = new int[numberToReturn];
            int[] sorted = Sort(rolls, false);
            for (int i = 0; i < numberToReturn; i++)
            {
                highestRolls[i] = sorted[i];
            }
            return highestRolls;
        }

        public static int[] GetLowest(int[] rolls, int numberToReturn = 1)
        {
            int[] lowestRolls = new int[numberToReturn];
            int[] sorted = Sort(rolls, false);
            for (int i = 0; i < numberToReturn; i++)
            {
                lowestRolls[i] = sorted[i];
            }
            return lowestRolls;
        }

        public static int Sum(int[] rolls)
        {
            int total = 0;
            foreach (int roll in rolls)
            {
                total += roll;
            }
            return total;
        }
    }

    public class Die
    {
        public int Sides { get; set; }

        public int Roll()
        {
            return Dice.Roll(Sides);
        }

        public override string ToString()
        {
            return "d" + Sides;
        }
    }

    public class Roller
    {
        public Roller() { }

        public Roller(string rollDetails)
        {
            if (!String.IsNullOrEmpty(rollDetails))
            {
                string roll = rollDetails.ToLowerInvariant();
                if (!String.IsNullOrEmpty(roll))
                {
                    string nextVal = String.Empty;
                    char nextOperator = '+';
                    int nextNumRolls = 1;
                    bool isDiceRoll = false;
                    foreach (char c in roll.ToCharArray())
                    {
                        switch (c)
                        {
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                nextVal += c;
                                break;
                            case '+':
                            case '-':
                                AddNext(nextVal, isDiceRoll, nextNumRolls, nextOperator);
                                nextVal = String.Empty;
                                nextOperator = c;
                                isDiceRoll = false;
                                nextNumRolls = 1;
                                break;
                            case 'd':
                                if (!String.IsNullOrEmpty(nextVal))
                                {
                                    nextNumRolls = Convert.ToInt32(nextVal);
                                    nextVal = String.Empty;
                                }
                                isDiceRoll = true;
                                break;
                        }
                    }
                    AddNext(nextVal, isDiceRoll, nextNumRolls, nextOperator);
                }
            }
        }

        private void AddNext(string nextVal, bool isDiceRoll, int nextNumRolls, char nextOperator)
        {
            if (!String.IsNullOrEmpty(nextVal))
            {
                int val = Convert.ToInt32(nextVal);
                bool isSubtract = nextOperator == '-';
                if (isDiceRoll)
                {
                    AddDie(val, nextNumRolls, isSubtract);
                }
                else
                {
                    UpdateModifier(val, isSubtract);
                }
            }
        }

        private void AddDie(int sides, int numRolls, bool isSubtract)
        {
            for (int i = 0; i < numRolls; i++)
            {
                if (isSubtract)
                {
                    SubtractDice.Add(new Die { Sides = sides });
                }
                else
                {
                    Dice.Add(new Die { Sides = sides });
                }
            }
        }

        private void UpdateModifier(int modifier, bool isSubtract)
        {
            if (isSubtract)
            {
                Modifier -= modifier;
            }
            else
            {
                Modifier += modifier;
            }
        }

        public List<Die> Dice { get; } = new List<Die>();

        public List<Die> SubtractDice { get; } = new List<Die>();

        public int Modifier { get; set; }

        public int Roll()
        {
            int total = 0;
            foreach (var die in Dice)
            {
                total += die.Roll();
            }
            foreach (var die in SubtractDice)
            {
                total -= die.Roll();
            }
            return total + Modifier;
        }

        public int GetMin()
        {
            int diceTotal = Dice.Count;
            if (SubtractDice.Count > 0)
            {
                diceTotal -= SubtractDice.Sum(d => d.Sides);
            }
            return diceTotal + Modifier;
        }

        public int GetMax()
        {
            int diceTotal = 0;
            if (Dice.Count > 0)
            {
                diceTotal += Dice.Sum(d => d.Sides);
            }
            diceTotal -= SubtractDice.Count;
            return diceTotal + Modifier;
        }

        public override string ToString()
        {
            StringBuilder desc = new StringBuilder();
            int currentSides = 0;
            int dieCount = 0;
            foreach (var die in Dice)
            {
                if (die.Sides != currentSides)
                {
                    if (Append(desc, currentSides, dieCount, false))
                    {
                        dieCount = 0;
                    }
                    currentSides = die.Sides;
                }
                dieCount++;
            }
            Append(desc, currentSides, dieCount, false);
            currentSides = 0;
            dieCount = 0;
            foreach (var die in SubtractDice)
            {
                if (die.Sides != currentSides)
                {
                    if (Append(desc, currentSides, dieCount, true))
                    {
                        dieCount = 0;
                    }
                    currentSides = die.Sides;
                }
                dieCount++;
            }
            Append(desc, currentSides, dieCount, true);
            if (Modifier < 0)
            {
                desc.Append(Modifier);
            }
            else if(Modifier > 0)
            {
                desc.Append("+" + Modifier);
            }
            return desc.ToString();
        }

        private bool Append(StringBuilder desc, int sides, int dieCount, bool isSubtract)
        {
            if (dieCount > 0)
            {
                if (desc.Length > 0 || isSubtract)
                {
                    desc.Append(isSubtract ? "-" : "+");
                }
                if (dieCount > 1)
                {
                    desc.Append(dieCount);
                }
                desc.Append("d" + sides);
                return true;
            }
            return false;
        }

        //public static string CleanUpRollDetails(string rollDetails)
        //{
        //    StringBuilder cleaned = new StringBuilder();
        //    if (!String.IsNullOrEmpty(rollDetails))
        //    {
        //        foreach (char c in rollDetails.ToLowerInvariant().ToCharArray())
        //        {
        //            switch (c)
        //            {
        //                case '0':
        //                case '1':
        //                case '2':
        //                case '3':
        //                case '4':
        //                case '5':
        //                case '6':
        //                case '7':
        //                case '8':
        //                case '9':
        //                case '+':
        //                case '-':
        //                case 'd':
        //                    cleaned.Append(c);
        //                    break;
        //            }
        //        }
        //    }
        //    return cleaned.ToString();
        //}
    }
}
