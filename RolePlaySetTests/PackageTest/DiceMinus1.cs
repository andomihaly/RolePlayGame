﻿using RandomDice;

namespace RolePlaySetTests
{
    public class DiceMinus1 : Dice
    {
        public string getName()
        {
            return "dM1";
        }

        public DiceValue throwADice()
        {
            return DiceValue.MinusOne;
        }
    }
}