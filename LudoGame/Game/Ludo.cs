using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudoGame.Game
{
    class Ludo
    {
        private int numberOfPlayers = 0;

        private List<Player> players;
        private Board board;

        private bool gameIsOver = false;

        private Player nextPlayer = null;

        public Ludo()
        {
            Utils.InitializeColors();
            this.players = new List<Player>();
        }

        public void InitializingGame()
        {
            // Step 1 - Create board logic
            this.board = new Board();

            // Step 2 - Ask for number of players
            this.AskForNumberOfPlayers();

            // Step 3 - Choose colors
            this.ChooseColorPerPlayer();

            // Step 4 - Find starting player
            Player playerToStart = this.FindStartingPlayer();
            this.nextPlayer = playerToStart;
        }

        public void StartGame()
        {
            while(!gameIsOver)
            {
                this.Play();
            }

            // TEST
            // For at teste manuelt, husk at udkommentér while-løkken
            /*this.MoveOutOfHome("G1");
            this.MoveF("G1", 4);

            this.MoveOutOfHome("G2");
            this.MoveF("G2", 6);

            this.MoveOutOfHome("Y1");
            this.MoveOutOfHome("Y2");

            this.MoveF("G2", 9);*/

            this.RefreshFrame();
            Console.ReadLine();
        }

        private void Play(Player playerToPlay = null, int turns = 1)
        {
            this.RefreshFrame();

            playerToPlay = (playerToPlay == null) ? this.nextPlayer : playerToPlay;

            List<Piece> allPieces       = playerToPlay.pieces.ToList(),
                        piecesAtHome    = allPieces.Where(p => p.IsHome()).ToList(),
                        piecesInPlay    = allPieces.Where(p => !p.IsHome()).ToList();

            int rolled = this.Roll(Dice.Roll());

            if (rolled == 6) turns++;
            Console.WriteLine("{0} rolled {1}", playerToPlay.GetColor(), rolled);

            if(piecesInPlay.Count() == 0)
            {
                bool hasRolledSix = (rolled == 6);
                int tries = 1;

                while(!hasRolledSix)
                {
                    rolled = this.Roll(Dice.Roll());

                    Console.WriteLine("{0} rolled {1}", playerToPlay.GetColor(), rolled);

                    if (rolled == 6)
                    {
                        hasRolledSix = true;
                        turns++;
                    }

                    if(tries-- == 0)
                    {
                        break;
                    }
                }

                if(hasRolledSix)
                {
                    Console.WriteLine("-- Moving {0} outside of home", piecesAtHome.First().GetPieceID());
                    this.MoveOutOfHome(piecesAtHome.First().GetPieceID());
                    Console.ReadLine();
                } else
                {
                    Console.ReadLine();
                }
            } else if (piecesAtHome.Count() == 0)
            {
                // can only move around
                Console.WriteLine("Choose a piece to move:");
                Console.WriteLine("-----------------------");

                Console.WriteLine("Piece(s) in play");
                this.PrintPiecesAvailable(piecesInPlay);

                string input = string.Empty;
                bool valid = false;

                while (!valid)
                {
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (rolled == 6)
                    {
                        if (piecesAtHome.Any(p => p.GetPieceID().ToString() == input))
                        {
                            valid = true;

                            this.MoveOutOfHome(input);
                        }
                    }

                    if (piecesInPlay.Any(p => p.GetPieceID().ToString() == input))
                    {
                        valid = true;

                        this.MoveF(input, rolled);
                    }
                }
            }
            else if (piecesAtHome.Count() > 0 && piecesInPlay.Count() > 0)
            {
                Console.WriteLine("Choose a piece to move:");
                Console.WriteLine("-----------------------");

                if (rolled == 6)
                {
                    Console.WriteLine("Piece(s) from home");
                    this.PrintPiecesAvailable(piecesAtHome);
                }

                Console.WriteLine("Piece(s) in play");
                this.PrintPiecesAvailable(piecesInPlay);

                string input = string.Empty;
                bool valid = false;

                while (!valid)
                {
                    Console.Write("> ");
                    input = Console.ReadLine();

                    if (rolled == 6)
                    {
                        if (piecesAtHome.Any(p => p.GetPieceID().ToString() == input))
                        {
                            valid = true;

                            this.MoveOutOfHome(input);
                        }
                    }

                    if (piecesInPlay.Any(p => p.GetPieceID().ToString() == input))
                    {
                        valid = true;

                        this.MoveF(input, rolled);
                    }
                }
            }
            else
            {
                Console.ReadLine();
            }

            if (turns > 1) this.Play(playerToPlay, (turns - 1));
            else this.NextTurn();
        }

        private void PrintPiecesAvailable(List<Piece> pieces)
        {
            pieces.ForEach(p =>
            {
                Console.WriteLine("- {0}", p.GetPieceID());
            });
        }

        private int Roll(string rolledString)
        {
            try
            {
                return int.Parse(rolledString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        private void NextTurn()
        {
            int playerID = this.nextPlayer.GetID();
            int nextID = (playerID == this.numberOfPlayers) ? 1 : playerID + 1;

            this.nextPlayer = this.players.Where(player => player.GetID() == nextID).First();
        }

        private void RefreshFrame()
        {
            Console.Clear();
            this.board.DrawBoard(this.players);
        }

        private void AskForNumberOfPlayers()
        {
            while(this.numberOfPlayers == 0)
            {
                Console.Write("How many players? (2-4): ");

                string input = Console.ReadLine();

                try
                {
                    int no = int.Parse(input);

                    if(Enumerable.Range(2,3).Contains(no))
                    {
                        this.numberOfPlayers = no;
                    } else
                    {
                        Console.WriteLine("Number of players has to be between 2 and 4");
                    }
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.Clear();
        }

        private void ChooseColorPerPlayer()
        {
            Dictionary<int, string> availableColors = new Dictionary<int, string>()
            {
                { 1, "Green" },
                { 2, "Yellow" },
                { 3, "Blue" },
                { 4, "Red" },
            };

            for (int i = 1; i <= this.numberOfPlayers; i++)
            {
                ConsoleColor playerColor = ConsoleColor.White;

                while(playerColor == ConsoleColor.White)
                {
                    Console.WriteLine("Available colors:");

                    foreach(KeyValuePair<int, string> data in availableColors)
                    {
                        Console.WriteLine("{0} = {1}", data.Key, data.Value);
                    }

                    Console.Write("{0}Pick a color for Player {1}: ", Environment.NewLine, i);

                    string input = Console.ReadLine();

                    try
                    {
                        int no = int.Parse(input);

                        if (availableColors.ContainsKey(no))
                        {
                            playerColor = Utils.GetColor(no);

                            availableColors.Remove(no);
                            Console.Clear();

                            this.CreatePlayer(i, playerColor);
                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("The choosen color isn't available");
                        }
                    } catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            Console.Clear();
        }

        private void CreatePlayer(int playerID, ConsoleColor playerColor)
        {
            this.players.Add(
                new Player()
                    .WithID(playerID)
                    .WithColor(playerColor)
                    .CreatePieces(playerID, playerColor, this.board)
            );
        }

        private Player FindStartingPlayer(List<Player> overloadPlayers = null)
        {
            List<Player> playersToSearchThrough = (overloadPlayers == null)
                ? this.players
                : overloadPlayers;

            playersToSearchThrough.ForEach(player =>
            {
                player.lastRolled = Dice.RollNormalDice();
            });

            int findMaxValue = playersToSearchThrough
                .Max(x => x.lastRolled);

            List<Player> playersWithMaxValue = playersToSearchThrough.Where(p => p.lastRolled == findMaxValue).ToList();

            return (playersWithMaxValue.Count() > 1)
                ? FindStartingPlayer(playersWithMaxValue)
                : playersWithMaxValue.First();
        }

        private void MoveOutOfHome(string pieceID)
        {
            Piece piece = this.GetPieceFromID(pieceID);

            if(piece != null)
            {
                Cords spawnFieldCords = this.board.FindSpawnOfPlayer(piece.GetPlayerColor());
                piece.Move(spawnFieldCords.row, spawnFieldCords.column, this.GetPositionFromCords(spawnFieldCords));
            } else
            {
                Console.WriteLine("No piece found");
            }
        }

        private void MoveToHome(string pieceID)
        {
            Piece piece = this.GetPieceFromID(pieceID);

            if(piece != null)
            {
                Cords homeCords = new Cords() { row = piece.GetHomeRow(), column = piece.GetHomeColumn() };
                piece.Move(homeCords.row, homeCords.column, this.GetPositionFromCords(homeCords));
            }
        }

        private void MoveF(string pieceID, int moves, bool reversed = false)
        {
            Piece piece = this.GetPieceFromID(pieceID);
            Player player = this.GetPlayerFromPiece(piece);

            bool hasFinishedMoving = false;

            int i = 0;
            int movesLeft = moves;

            while(!hasFinishedMoving)
            {
                Field currentField = this.GetFieldFromPosition(piece.GetPosition());
                int nextPos = (reversed) ? currentField.GetFieldID() - 1 : currentField.GetFieldID() + 1;

                if(nextPos == 53)
                    nextPos = 1;

                if(currentField.IsFinish && moves > 0)
                {
                    reversed = true;
                    nextPos -= 2;
                } else if(currentField.IsFinish && moves == 0)
                {
                    player.pieces.Remove(piece);
                }

                Field nextField = this.GetFieldFromPosition(nextPos);

                if(currentField.IsFinishLine && (piece.GetPlayerColor() == Utils.GetColor(currentField.FinishLineFor)) && currentField.JumpTo)
                {
                    nextField = this.GetFieldFromPosition(currentField.jumpToField);
                }

                // RULE: Cannot jump over own pieces
                // RULE: Beats enemy home, when only one enemy on field
                // RULE: Get's beating home, when more than one enemy on field

                List<Piece> piecesOnField = this.GetPiecesFromField(nextField);

                if(piecesOnField.Count() >= 1)
                {
                    piecesOnField.ForEach(p =>
                    {
                        if(p.GetPlayerColor() == piece.GetPlayerColor())
                        {
                            hasFinishedMoving = true;
                            moves = 0;
                        } else
                        {
                            if((movesLeft - 1) == 0)
                            {
                                if (piecesOnField.Count() > 1)
                                {
                                    this.MoveToHome(piece.GetPieceID());
                                }
                                else
                                {
                                    this.MoveToHome(piecesOnField.First().GetPieceID());
                                }
                            }
                        }
                    });
                }

                // Register move
                piece.SetPosition(nextField.GetFieldID(), nextField);
                movesLeft--;
                i++;

                if (i == moves) hasFinishedMoving = true;
            }
        }

        private Field GetFieldFromPosition(int position)
        {
            return this.board.fields
                .Where(f => f.GetFieldID() == position)
                .First();
        }

        private int GetPositionFromCords(Cords cordinates)
        {
            return this.board.fields
                .Where(f => f.GetRow() == cordinates.row)
                .Where(f => f.GetColumn() == cordinates.column)
                .First()
                .GetFieldID();
        }

        private Piece GetPieceFromID(string pieceID)
        {
            List<Piece> pieces = new List<Piece>();

            this.players.ForEach(player =>
            {
                player.pieces.ForEach(p =>
                {
                    pieces.Add(p);
                });
            });

            return pieces.Where(p => p.GetPieceID() == pieceID).First();
        }

        private Player GetPlayerFromPiece(Piece piece)
        {
            Player player = null;

            this.players.ForEach(pl =>
            {
                pl.pieces.ForEach(pi =>
                {
                    if(pi.GetPieceID() == piece.GetPieceID())
                    {
                        player = pl;
                    }
                });
            });

            return player;
        }

        private List<Piece> GetPiecesFromField(Field field)
        {
            List<Piece> pieces = new List<Piece>();

            this.players.ForEach(pl =>
            {
                pl.pieces
                .Where(pi => pi.GetPosition() == field.GetFieldID())
                .ToList()
                .ForEach(piece =>
                {
                    pieces.Add(piece);
                });
            });

            return pieces;
        }
    }
}
