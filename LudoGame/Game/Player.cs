using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudoGame.Game
{
    class Player
    {
        private int playerID;
        private ConsoleColor playerColor;

        public List<Piece> pieces = new List<Piece>();
        public int lastRolled { get; set; }

        public Player()
        {
        }

        public Player WithID(int playerID)
        {
            this.playerID = playerID;
            return this;
        }
        public Player WithColor(ConsoleColor playerColor)
        {
            this.playerColor = playerColor;
            return this;
        }
        public Player CreatePieces(int playerID, ConsoleColor playerColor, Board board)
        {
            List<Field> findCords = (
                from x in board.fields
                where
                    x.GetColor() == playerColor &&
                    x.IsHome == true
                select x
            ).ToList();

            int i = 1;

            findCords.ForEach(f =>
            {
                this.pieces.Add(
                    new Piece(
                        i: i++,
                        playerColor: this.playerColor,
                        row: f.GetRow(),
                        column: f.GetColumn()
                    )
                );
            });

            return this;
        }

        public int GetID()
        {
            return this.playerID;
        }
        public ConsoleColor GetColor()
        {
            return this.playerColor;
        }
    }
}
