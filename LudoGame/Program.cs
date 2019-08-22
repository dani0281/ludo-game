using LudoGame.Game;
using System;

namespace LudoGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Ludo ludo = new Ludo();
            ludo.InitializingGame();
            ludo.StartGame();
        }
    }
}
