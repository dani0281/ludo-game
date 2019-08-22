using System;
using System.Collections.Generic;
using System.Text;

namespace LudoGame.Game
{
    class Dice
    {
        static string[] possibleRolls = {
            "1", "2", "3", "4", "5", "6"
        };

        public static string Roll()
        {
            return possibleRolls[new Random().Next(0, possibleRolls.Length)];
        }

        public static int RollNormalDice()
        {
            return new Random().Next(1, 7);
        }
    }
}
