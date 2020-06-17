// <copyright file="GameRepository.cs" company="RJ">
// Copyright (c) RJ. All rights reserved.
// </copyright>

namespace Chess.DomainServices
{
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Chess.Domain;

    /// <summary>
    /// Manages game persistence.
    /// </summary>
    public partial class GameRepository : IRepository<Game, int>
    {
        private readonly string gameFolderPath;
        private readonly IRepository<Player, long> playerRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameRepository"/> class.
        /// </summary>
        /// <param name="gameFolderPath">The path at which to store game documents.</param>
        /// <param name="playerRepository">A player repository.</param>
        public GameRepository(string gameFolderPath, IRepository<Player, long> playerRepository)
        {
            this.gameFolderPath = gameFolderPath;
            this.playerRepository = playerRepository;
        }

        /// <inheritdoc/>
        public async Task<Game> Persist(Game game)
        {
            if (game == null)
            {
                return null;
            }

            var gameDto = new GameDto
            {
                Id = game.Id,
                CreatedAt = game.CreatedAt,
                Status = game.Status,
                Turn = game.Turn.Kind,
                Winner = game.Winner,
                Moves = game.Moves.Select(m => new MovePairDto
                {
                    WhiteMove = m.WhiteMove != null ? new MoveDto(m.WhiteMove) : null,
                    BlackMove = m.BlackMove != null ? new MoveDto(m.BlackMove) : null,
                }),
                WhiteArmy = new ArmyDto(game.WhiteArmy),
                BlackArmy = new ArmyDto(game.BlackArmy),
            };

            using (FileStream file = new FileStream($"{this.gameFolderPath}\\Game-{game.Id}.json", FileMode.OpenOrCreate))
            {
                await Task.Run(() => new DataContractJsonSerializer(typeof(GameDto)).WriteObject(file, gameDto))
                    .ConfigureAwait(false);

                return game;
            }
        }

        /// <inheritdoc/>
        public async Task<Game> Retrieve(int id)
        {
            Game game = new Game();

            using (FileStream file = new FileStream($"{this.gameFolderPath}\\Game-{id}.json", FileMode.Open))
            {
                GameDto gameDto = await Task.Run(() => new DataContractJsonSerializer(typeof(GameDto)).ReadObject(file) as GameDto)
                    .ConfigureAwait(false);

                Army whiteArmy = new WhiteArmy(
                    await this.playerRepository.Retrieve(gameDto.WhiteArmy.CommanderId).ConfigureAwait(false),
                    new GameState(game, ArmyKind.White),
                    gameDto.WhiteArmy.Pieces?.Select(p => p.ToPieceSpecification(game.ChessBoard)),
                    gameDto.WhiteArmy.Id);

                Army blackArmy = new BlackArmy(
                    await this.playerRepository.Retrieve(gameDto.BlackArmy.CommanderId).ConfigureAwait(false),
                    new GameState(game, ArmyKind.Black),
                    gameDto.BlackArmy.Pieces?.Select(p => p.ToPieceSpecification(game.ChessBoard)),
                    gameDto.BlackArmy.Id);

                game.LoadGame(
                    gameDto.Id,
                    gameDto.CreatedAt,
                    whiteArmy,
                    blackArmy,
                    gameDto.Turn == ArmyKind.White ? whiteArmy : blackArmy,
                    gameDto.Status,
                    gameDto.Winner,
                    gameDto.Moves?.Select(mp => new MovePair(mp.WhiteMove.ToMove(game.ChessBoard, whiteArmy), mp.BlackMove?.ToMove(game.ChessBoard, blackArmy))));
            }

            return game;
        }

        /// <inheritdoc/>
        public async Task<ResultPage<Game>> RetrieveAll(string continuationToken = null)
        {
            Game[] games = await Task.WhenAll(new DirectoryInfo(this.gameFolderPath).EnumerateFiles().Select(async fileInfo =>
            {
                using (FileStream file = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    GameDto gameDto = await Task.Run(() => new DataContractJsonSerializer(typeof(GameDto)).ReadObject(file) as GameDto)
                        .ConfigureAwait(false);

                    Game game = new Game();

                    Army whiteArmy = new WhiteArmy(
                        await this.playerRepository.Retrieve(gameDto.WhiteArmy.CommanderId).ConfigureAwait(false),
                        new GameState(game, ArmyKind.White),
                        gameDto.WhiteArmy.Pieces?.Select(p => p.ToPieceSpecification(game.ChessBoard)),
                        gameDto.WhiteArmy.Id);

                    Army blackArmy = new BlackArmy(
                        await this.playerRepository.Retrieve(gameDto.BlackArmy.CommanderId).ConfigureAwait(false),
                        new GameState(game, ArmyKind.Black),
                        gameDto.BlackArmy.Pieces?.Select(p => p.ToPieceSpecification(game.ChessBoard)),
                        gameDto.BlackArmy.Id);

                    game.LoadGame(
                        gameDto.Id,
                        gameDto.CreatedAt,
                        whiteArmy,
                        blackArmy,
                        gameDto.Turn == ArmyKind.White ? whiteArmy : blackArmy,
                        gameDto.Status,
                        gameDto.Winner,
                        gameDto.Moves?.Select(mp => new MovePair(mp.WhiteMove.ToMove(game.ChessBoard, whiteArmy), mp.BlackMove?.ToMove(game.ChessBoard, blackArmy))));

                    return game;
                }
            }));

            return new ResultPage<Game>(games, null);
        }
    }
}
