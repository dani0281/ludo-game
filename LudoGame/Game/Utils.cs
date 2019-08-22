using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudoGame.Game
{
    class Utils
    {
        public static List<Color> colors;

        public static void InitializeColors()
        {
            colors = new List<Color>();
            colors.Add(new Color() { index = 1, color = ConsoleColor.Green });
            colors.Add(new Color() { index = 2, color = ConsoleColor.Yellow });
            colors.Add(new Color() { index = 3, color = ConsoleColor.Blue });
            colors.Add(new Color() { index = 4, color = ConsoleColor.Red });
            colors.Add(new Color() { index = 5, color = ConsoleColor.White });
            colors.Add(new Color() { index = 6, color = ConsoleColor.Black });
        }

        public static ConsoleColor GetColor(int index)
        {
            return colors.Where(c => c.index == index).First().color;
        }
    }
}
