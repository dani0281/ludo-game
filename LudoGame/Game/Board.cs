using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudoGame.Game
{
    class Board
    {
        /// <summary>
        /// Variables:
        ///   1 = Green
        ///   2 = Yellow
        ///   3 = Blue
        ///   4 = Red
        ///   5 = White
        ///   6 = Black
        ///   M = Moveable
        ///   S = Start/Spawn
        ///   H = Home
        ///   J = Skip finishline
        ///   F = Finishfield
        ///   L{1-4} = Finishline for specified color index
        ///   
        /// Usage:
        ///   color index {1-6}
        ///   movement attribute (moveable/home/etc.)
        ///   field ID
        ///   other arguments
        /// </summary>
        private List<string> map = new List<string>() {
            "1",                "1",            "1",            "1",            "1",            "1",            "5,M,11",       "5,M,12,J59,L2",    "5,M,13",       "2",            "2",            "2",            "2",            "2",            "2",
            "1",                "1",            "1",            "1",            "1",            "1",            "5,M,10",       "2,M,59,L2",        "2,M,14,S",     "2",            "2",            "2",            "2",            "2",            "2",
            "1",                "1",            "1,H",          "1,H",          "1",            "1",            "5,M,9",        "2,M,60,L2",        "5,M,15",       "2",            "2",            "2,H",          "2,H",          "2",            "2",
            "1",                "1",            "1,H",          "1,H",          "1",            "1",            "5,M,8",        "2,M,61,L2",        "5,M,16",       "2",            "2",            "2,H",          "2,H",          "2",            "2",
            "1",                "1",            "1",            "1",            "1",            "1",            "5,M,7",        "2,M,62,L2",        "5,M,17",       "2",            "2",            "2",            "2",            "2",            "2",
            "1",                "1",            "1",            "1",            "1",            "1",            "5,M,6",        "2,M,63,L2",        "5,M,18",       "2",            "2",            "2",            "2",            "2",            "2",
            "5,M,52",           "1,M,1,S",      "5,M,2",        "5,M,3",        "5,M,4",        "5,M,5",        "6",            "2,M,64,F,L2",      "6",            "5,M,19",       "5,M,20",       "5,M,21",       "5,M,22",       "5,M,23",       "5,M,24",
            "5,M,51,J53,L1",    "1,M,53,L1",    "1,M,54,L1",    "1,M,55,L1",    "1,M,56,L1",    "1,M,57,L1",    "1,M,58,F,L1",  "6",                "3,M,70,F,L3",  "3,M,69,L3",    "3,M,68,L3",    "3,M,67,L3",    "3,M,66,L3",    "3,M,65,L3",    "5,M,25,J65,L3",
            "5,M,50",           "5,M,49",       "5,M,48",       "5,M,47",       "5,M,46",       "5,M,45",       "6",            "4,M,76,F,L4",      "6",            "5,M,31",       "5,M,30",       "5,M,29",       "5,M,28",       "3,M,27,S",     "5,M,26",
            "4",                "4",            "4",            "4",            "4",            "4",            "5,M,44",       "4,M,75,L4",        "5,M,32",       "3",            "3",            "3",            "3",            "3",            "3",
            "4",                "4",            "4",            "4",            "4",            "4",            "5,M,43",       "4,M,74,L4",        "5,M,33",       "3",            "3",            "3",            "3",            "3",            "3",
            "4",                "4",            "4,H",          "4,H",          "4",            "4",            "5,M,42",       "4,M,73,L4",        "5,M,34",       "3",            "3",            "3,H",          "3,H",          "3",            "3",
            "4",                "4",            "4,H",          "4,H",          "4",            "4",            "5,M,41",       "4,M,72,L4",        "5,M,35",       "3",            "3",            "3,H",          "3,H",          "3",            "3",
            "4",                "4",            "4",            "4",            "4",            "4",            "4,M,40,S",     "4,M,71,L4",        "5,M,36",       "3",            "3",            "3",            "3",            "3",            "3",
            "4",                "4",            "4",            "4",            "4",            "4",            "5,M,39",       "5,M,38,J71,L4",    "5,M,37",       "3",            "3",            "3",            "3",            "3",            "3"
        };

        public List<Field> fields = new List<Field>();

        public Board()
        {
            this.CreateFields();
        }

        public Cords FindSpawnOfPlayer(ConsoleColor playerColor)
        {
            Field field = this.fields
                        .Where(f => f.IsSpawn == true)
                        .Where(f => f.GetColor() == playerColor)
                        .First();

            return new Cords(){
                row = field.GetRow(),
                column = field.GetColumn()
            };
        }

        public void DrawBoard(List<Player> players)
        {
            int row     = 1,
                column  = 1;

            this.fields.ForEach(field =>
            {
                List<Piece> piecesOnField = new List<Piece>();

                players.ForEach(player =>
                {
                    player.pieces
                        .Where(piece => piece.GetRow() == field.GetRow())
                        .Where(piece => piece.GetColumn() == field.GetColumn())
                        .ToList()
                        .ForEach(piece =>
                        {
                            piecesOnField.Add(piece);
                        }
                    );
                });

                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = field.GetColor();

                string output = (piecesOnField.Count() == 0) ? "" : String.Join(",", piecesOnField.Select(x => x.GetPieceID()));

                Console.Write(output.PadLeft(2).PadRight(2));

                if (field.GetColumn() == 15) Console.Write(Environment.NewLine);
            });

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        private void CreateFields()
        {
            int row     = 1,
                column  = 1;

            this.map.ForEach(m =>
            {
                string[] arguments = m.Split(',');

                try
                {
                    int colorIndex = int.Parse(arguments[0]);

                    int fieldID = (arguments.Length >= 3) ? int.Parse(arguments[2]) : 0;

                    this.fields.Add(new Field(
                        fieldID: fieldID,
                        row: row,
                        column: column,
                        fieldColor: Utils.GetColor(colorIndex)
                    ).SetArguments(arguments));
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(String.Join(", ", arguments));
                }

                if(column++ == 15)
                {
                    row++;
                    column = 1;
                }
            });
        }
    }
}
