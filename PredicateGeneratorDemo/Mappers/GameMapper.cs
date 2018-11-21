using System.Collections.Generic;
using System.Linq;
using PredicateGeneratorDemo.Data;

namespace PredicateGeneratorDemo.Mappers
{
    public static class GameMapper
    {
        public static IEnumerable<GameDto> ToGameDtos(this IEnumerable<Game> source)
        {
            return source.Select(o => new GameDto
            {
                Title = o.Title,
                Genre = o.Genre,
                Multiplayer = o.Multiplayer,
                Rating = o.Rating,
                ReleaseDate = o.ReleaseDate
            });
        }
    }
}
