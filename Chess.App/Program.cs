// <copyright file="Program.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Chess.Domain;
    using Chess.DomainServices;

    /// <summary>
    /// Main program.
    /// </summary>
    public static class Program
    {
        private static readonly string DocumentBaseFolder = ConfigurationManager.AppSettings["DocumentBaseFolder"];
        private static readonly string PlayersFolder = System.IO.Path.Combine(DocumentBaseFolder, "Players");
        private static readonly string GamesFolder = System.IO.Path.Combine(DocumentBaseFolder, "games");

        private static readonly PlayerRepository PlayerRepository = new PlayerRepository(PlayersFolder);
        private static readonly GameRepository GameRepository = new GameRepository(GamesFolder, PlayerRepository);

        /// <summary>
        /// The main function.
        /// </summary>
        public static void Main()
        {
            // Ensure document folders are created
            System.IO.Directory.CreateDirectory(PlayersFolder);
            System.IO.Directory.CreateDirectory(GamesFolder);

            while (true)
            {
                Game game = ConsoleHelper.Menu(
                    "Chess 1.0",
                    string.Empty,
                    true,
                    ("New Game", NewGame),
                    ("Load Game", LoadGame),
                    ("Watch Fool's Mate", PlayFoolsMate),
                    ("Watch Stale Mate", PlayStaleMate));

                if (game == null)
                {
                    return;
                }

                while (!game.IsFinished)
                {
                    try
                    {
                        Console.Clear();
                        GameRepository.Persist(game).Wait();
                        RenderChessBoard(game.ChessBoard, ArmyKind.White);

                        ConsoleHelper.Display($"{game.Turn.Commander.Name} Turn", ConsoleColor.Magenta);
                        string position = ConsoleHelper.ReadNonEmptyString("Piece to move (e.g: D4)").ToUpperInvariant();

                        File file = new File(1 + position[0] - 'A');
                        Rank rank = new Rank(1 + position[1] - '1');

                        Piece pieceToMove = game.Turn.Pieces[file, rank];
                        position = ConsoleHelper.ReadNonEmptyString("Move to").ToUpperInvariant();

                        file = new File(1 + position[0] - 'A');
                        rank = new Rank(1 + position[1] - '1');

                        if (position.Length > 2 && pieceToMove is Pawn pawn)
                        {
                            // this is a promotion
                            switch (position[2].ToString().ToUpperInvariant())
                            {
                                case "B":
                                    pawn.Promote(new Cell(file, rank), PieceKind.Bishop);
                                    break;
                                case "N":
                                    pawn.Promote(new Cell(file, rank), PieceKind.Knight);
                                    break;
                                case "R":
                                    pawn.Promote(new Cell(file, rank), PieceKind.Rook);
                                    break;
                                case "Q":
                                    pawn.Promote(new Cell(file, rank), PieceKind.Queen);
                                    break;
                                default:
                                    throw new ArgumentException("Invalid piece to promote to");
                            }
                        }
                        else
                        {
                            pieceToMove.Move(file, rank);
                        }
                    }
                    catch (Exception e)
                    {
                        ConsoleHelper.Error(e.Message);
                    }
                }

                RenderChessBoard(game.ChessBoard, ArmyKind.White);

                if (game.Winner == GameWinner.None)
                {
                    ConsoleHelper.Display($"Stale mate!", ConsoleColor.Green);
                }
                else
                {
                    ConsoleHelper.Display($"{game.Winner} Won the game!", ConsoleColor.Green);
                }

                Console.ReadKey();
            }
        }

        /// <summary>
        /// Starts a new game.
        /// </summary>
        /// <returns>The game.</returns>
        private static Game NewGame()
        {
            Player whitePlayer = ConsoleHelper.Menu(
               "Choose White Player",
               string.Empty,
               false,
               ("New Player", NewPlayer),
               ("Load Player", LoadPlayer));

            Player blackPlayer = null;

            while (blackPlayer == null || blackPlayer == whitePlayer)
            {
                if (blackPlayer == whitePlayer)
                {
                    ConsoleHelper.Error("You can't play against yourself.");
                    Console.ReadKey();
                }

                blackPlayer = ConsoleHelper.Menu(
                   "Choose Black Player",
                   string.Empty,
                   false,
                   ("New Player", NewPlayer),
                   ("Load Player", LoadPlayer));
            }

            Game newGame = new Game();
            newGame.JoinAs(whitePlayer, ArmyKind.White);
            newGame.JoinAs(blackPlayer, ArmyKind.Black);

            return newGame;
        }

        /// <summary>
        /// Loads an existing game.
        /// </summary>
        /// <returns>The loaded game.</returns>
        private static Game LoadGame()
        {
            var games = GameRepository.RetrieveAll().Result.Results;

            return ConsoleHelper.Menu(
                "Select Game",
                "No games saved",
                true,
                games.Select<Game, (string, Func<Game>)>(
                    game => ($"{game.CreatedAt}: {game.WhiteArmy.Commander.Name} vs {game.BlackArmy.Commander.Name}", () => game)).ToArray());
        }

        /// <summary>
        /// Creates a new player.
        /// </summary>
        /// <returns>The player.</returns>
        private static Player NewPlayer()
        {
            return PlayerRepository.Persist(new Player
            {
                Name = ConsoleHelper.ReadNonEmptyString("Player Name"),
                Rating = ConsoleHelper.ReadNumber("Player rating"),
            }).Result;
        }

        /// <summary>
        /// Loads a player.
        /// </summary>
        /// <returns>The player.</returns>
        private static Player LoadPlayer()
        {
            var players = PlayerRepository.RetrieveAll().Result.Results;

            return ConsoleHelper.Menu(
                "Select Player",
                "No players saved",
                true,
                players.Select<Player, (string, Func<Player>)>(player => (player.Name, () => player)).ToArray());
        }

        private static Game PlayFoolsMate()
        {
            Game game = NewGame();

            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Pawn>(File.F).Move(File.F, Rank.Third));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Pawn>(File.E).Move(File.E, Rank.Fifth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Pawn>(File.G).Move(File.G, Rank.Fourth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Queen>(File.D).Move(File.H, Rank.Fourth));

            return game;
        }

        private static Game PlayStaleMate()
        {
            Game game = NewGame();

            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Pawn>(File.C).Move(File.C, Rank.Fourth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Pawn>(File.H).Move(File.H, Rank.Fifth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Pawn>(File.H).Move(File.H, Rank.Fourth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Pawn>(File.A).Move(File.A, Rank.Fifth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.D).Move(File.A, Rank.Fourth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Rook>(File.A).Move(File.A, Rank.Sixth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.A).Move(File.A, Rank.Fifth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Rook>(File.A).Move(File.H, Rank.Sixth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.A).Move(File.C, Rank.Seventh));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Pawn>(File.F).Move(File.F, Rank.Sixth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.C).Move(File.D, Rank.Seventh));
            AutoTurn(game, () => game.BlackArmy.King.Move(File.F, Rank.Seventh));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.D).Move(File.B, Rank.Seventh));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Queen>(File.D).Move(File.D, Rank.Third));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.B).Move(File.B, Rank.Eighth));
            AutoTurn(game, () => game.BlackArmy.Pieces.At<Queen>(File.D).Move(File.H, Rank.Seventh));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.B).Move(File.C, Rank.Eighth));
            AutoTurn(game, () => game.BlackArmy.King.Move(File.G, Rank.Sixth));
            AutoTurn(game, () => game.WhiteArmy.Pieces.At<Queen>(File.C).Move(File.E, Rank.Sixth));

            return game;
        }

        private static void AutoTurn(Game game, Action move)
        {
            move();
            RenderChessBoard(game.ChessBoard, ArmyKind.White);
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Renders the chess board.
        /// </summary>
        /// <param name="board">The chess board.</param>
        /// <param name="perspective">The player perspective.</param>
        private static void RenderChessBoard(ChessBoard board, ArmyKind perspective)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition((Console.WindowWidth / 2) - 14, Console.CursorTop);
            Console.Write(" ".PadLeft(3));

            for (File file = File.A; file != null; file++)
            {
                Console.Write(file.Name.PadLeft(2).PadRight(3));
            }

            Console.WriteLine();
            Console.ResetColor();
            ConsoleColor cellColor = ConsoleColor.DarkGray;

            for (Rank rank = Rank.Eighth; rank != null; rank--)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) - 14, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(rank.Name.PadLeft(2));
                Console.ResetColor();
                Console.Write(" ");

                cellColor = cellColor == ConsoleColor.DarkGray ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;

                for (File file = File.A; file != null; file++)
                {
                    cellColor = cellColor == ConsoleColor.DarkGray ? ConsoleColor.DarkGreen : ConsoleColor.DarkGray;
                    Cell cell = board[file, rank];
                    Console.BackgroundColor = cellColor;

                    if (cell.IsOccupied)
                    {
                        Console.ForegroundColor = cell.OccupyingPiece.Army.Kind == ArmyKind.White ? ConsoleColor.White : ConsoleColor.Black;
                        Console.Write(cell.OccupyingPiece.Symbol.PadLeft(2).PadRight(3));
                    }
                    else
                    {
                        Console.Write(" ".PadLeft(3));
                    }

                    Console.ResetColor();
                }

                Console.WriteLine();
            }

            Console.SetCursorPosition((Console.WindowWidth / 2) - 14, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(" ".PadLeft(3));

            for (File file = File.A; file != null; file++)
            {
                Console.Write(file.Name.PadLeft(2).PadRight(3));
            }

            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
