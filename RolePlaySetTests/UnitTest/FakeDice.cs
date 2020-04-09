﻿using RandomDice;

namespace RolePlaySetTests.UnitTest
{
    internal class FakeDice : Dice
    {
        public string getName()
        {
            return "fakeDice";
        }

        public DiceValue rollADice()
        {
            return DiceValue.Zero;
        }
    }
}