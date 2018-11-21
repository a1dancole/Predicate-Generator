using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PredicateGeneratorDemo.Data
{
    public class DataStore : IDataStore
    {
        private IEnumerable<Game> Games { get; set; }

        public DataStore()
        {
            Games = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Title = "Shoot 'em up",
                    Genre = Genre.FPS,
                    Multiplayer = true,
                    Rating = 8,
                    ReleaseDate = new DateTime(1999, 1, 19)
                },
                new Game
                {
                    Id = 2,
                    Title = "Adventure Conquest",
                    Genre = Genre.MMORPG,
                    Multiplayer = true,
                    Rating = 5,
                    ReleaseDate = new DateTime(2007, 5, 22)
                },
                new Game
                {
                    Id = 3,
                    Title = "Raging lanes",
                    Genre = Genre.MOBA,
                    Multiplayer = true,
                    Rating = 4,
                    ReleaseDate = new DateTime(2017, 12, 1)
                },
                new Game
                {
                    Id = 4,
                    Title = "SWrack your brains",
                    Genre = Genre.RTS,
                    Multiplayer = false,
                    Rating = 2,
                    ReleaseDate = new DateTime(2015, 10, 12)
                },
                new Game
                {
                    Id = 5,
                    Title = "Cool running",
                    Genre = Genre.RPG,
                    Multiplayer = false,
                    Rating = 10,
                    ReleaseDate = new DateTime(2018, 12, 2)
                }
            };
        }

        public IEnumerable<Game> GetGames() => Games;
    }
}
