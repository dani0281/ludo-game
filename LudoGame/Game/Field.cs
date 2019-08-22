using System;
using System.Collections.Generic;
using System.Text;

namespace LudoGame.Game
{
    class Field
    {
        private int fieldID;
        private int row;
        private int column;
        private ConsoleColor fieldColor;

        public bool IsHome = false;
        public bool IsMoveable = false;
        public bool IsFinish = false;
        public bool IsSpawn = false;
        public bool JumpTo = false;

        public int jumpToField = 0;

        public bool IsFinishLine = false;
        public int FinishLineFor = 0;

        public Field(int fieldID, int row, int column, ConsoleColor fieldColor)
        {
            this.fieldID = fieldID;
            this.row = row;
            this.column = column;
            this.fieldColor = fieldColor;
        }

        public Field SetArguments(string[] args)
        {
            foreach(string arg in args)
            {
                switch(arg)
                {
                    case "H": this.IsHome = true; break;
                    case "M": this.IsMoveable = true; break;
                    case "F": this.IsFinish = true; break;
                    case "S": this.IsSpawn = true; break;
                }

                if(arg.StartsWith("L"))
                {
                    this.IsFinishLine = true;

                    try
                    {
                        this.FinishLineFor = int.Parse(arg.Substring(1, 1));
                    } catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                } else if(arg.StartsWith("J"))
                {
                    string jumpTo = arg.Substring(1, arg.Length - 1);

                    try
                    {
                        this.jumpToField = int.Parse(jumpTo);
                        this.JumpTo = true;
                    } catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return this;
        }

        public int GetFieldID()
        {
            return this.fieldID;
        }

        public int GetRow()
        {
            return this.row;
        }

        public int GetColumn()
        {
            return this.column;
        }

        public ConsoleColor GetColor()
        {
            return this.fieldColor;
        }
    }
}
