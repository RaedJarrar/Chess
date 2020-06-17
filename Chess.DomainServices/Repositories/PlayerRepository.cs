// <copyright file="PlayerRepository.cs" company="RJ">
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
    /// Player repository.
    /// </summary>
    public partial class PlayerRepository : IRepository<Player, long>
    {
        private readonly string playerFolderPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRepository"/> class.
        /// </summary>
        /// <param name="playerFolderPath">The folder path in which to store the player documents.</param>
        public PlayerRepository(string playerFolderPath)
        {
            this.playerFolderPath = playerFolderPath;
        }

        /// <inheritdoc/>
        public async Task<Player> Persist(Player player)
        {
            if (player == null)
            {
                return null;
            }

            var playerDto = new PlayerDto
            {
                Id = player.Id,
                Name = player.Name,
                Rating = player.Rating,
            };

            using (FileStream file = new FileStream($"{this.playerFolderPath}\\Player-{player.Id}.json", FileMode.OpenOrCreate))
            {
                await Task.Run(() => new DataContractJsonSerializer(typeof(PlayerDto)).WriteObject(file, playerDto))
                    .ConfigureAwait(false);

                return player;
            }
        }

        /// <inheritdoc/>
        public async Task<Player> Retrieve(long id)
        {
            using (FileStream file = new FileStream($"{this.playerFolderPath}\\Player-{id}.json", FileMode.Open, FileAccess.Read))
            {
                PlayerDto playerDto = await Task.Run(() => new DataContractJsonSerializer(typeof(PlayerDto)).ReadObject(file) as PlayerDto)
                    .ConfigureAwait(false);

                return new Player(playerDto.Id)
                {
                    Name = playerDto.Name,
                    Rating = playerDto.Rating,
                };
            }
        }

        /// <inheritdoc/>
        public async Task<ResultPage<Player>> RetrieveAll(string continuationToken = null)
        {
            Player[] players = await Task.WhenAll(new DirectoryInfo(this.playerFolderPath).EnumerateFiles().Select(async fileInfo =>
            {
                using (FileStream file = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    PlayerDto playerDto = await Task.Run(() => new DataContractJsonSerializer(typeof(PlayerDto)).ReadObject(file) as PlayerDto)
                        .ConfigureAwait(false);

                    return new Player(playerDto.Id)
                    {
                        Name = playerDto.Name,
                        Rating = playerDto.Rating,
                    };
                }
            }));

            return new ResultPage<Player>(players, null);
        }
    }
}
