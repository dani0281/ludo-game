using System;
using System.Collections.Generic;
using System.Text;

namespace LudoGame.Game
{
    class Piece
    {
        private string pieceID;
        private ConsoleColor playerColor;

        // cords
        private int homeRow = 0;
        private int homeColumn = 0;
        private int row = 0;
        private int column = 0;

        private int position = 0;

        public Piece(int i, ConsoleColor playerColor, int row, int column)
        {
            this.pieceID = string.Format(
                "{0}{1}",
                playerColor.ToString().Substring(0, 1).ToUpper(),
                i
            );

            this.row = row;
            this.homeRow = row;
            this.column = column;
            this.homeColumn = column;

            this.playerColor = playerColor;
        }

        public string GetPieceID()
        {
            return this.pieceID;
        }

        public int GetRow()
        {
            return this.row;
        }

        public void SetRow(int row)
        {
            this.row = row;
        }

        public int GetHomeRow()
        {
            return this.homeRow;
        }

        public int GetColumn()
        {
            return this.column;
        }

        public void SetColumn(int column)
        {
            this.column = column;
        }

        public int GetHomeColumn()
        {
            return this.homeColumn;
        }

        public int GetPosition()
        {
            return this.position;
        }

        public void SetPosition(int position, Field field)
        {
            this.position = position;
            this.SetRow(field.GetRow());
            this.SetColumn(field.GetColumn());
        }

        public ConsoleColor GetPlayerColor()
        {
            return this.playerColor;
        }

        public Piece Move(int row, int column, int position)
        {
            this.row = row;
            this.column = column;
            this.position = position;
            return this;
        }

        public bool IsHome()
        {
            return (this.row == this.homeRow && this.column == this.homeColumn);
        }
    }
}
