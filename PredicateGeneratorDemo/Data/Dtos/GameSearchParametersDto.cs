using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PredicateGenerator.Attribute;
using PredicateGenerator.Enums;

namespace PredicateGeneratorDemo.Data.Dtos
{
    public class GameSearchParametersDto
    {
        [Predicate(PredicateExpressionType.Contains, nameof(Game.Title))]
        public string SearchTerm { get; set; }
        [Predicate(PredicateExpressionType.Contains, nameof(Game.Genre))]
        public List<int?> Genres { get; set; }
        public int? MinRating { get; set; }
        public int? MaxRating { get; set; }
        [Predicate(PredicateExpressionType.Equals, nameof(Game.Multiplayer))]
        public bool? IsMultiplayer { get; set; }
        [Predicate(PredicateExpressionType.GreaterThanOrEqual, nameof(Game.ReleaseDate))]
        public DateTime? MinimumReleaseDate { get; set; }

    }
}
