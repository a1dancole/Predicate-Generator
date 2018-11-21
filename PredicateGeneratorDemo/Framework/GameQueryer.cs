using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PredicateGenerator.Extensions;
using PredicateGenerator.Generator;
using PredicateGeneratorDemo.Data;
using PredicateGeneratorDemo.Data.Dtos;
using PredicateGeneratorDemo.Framework.Interfaces;
using PredicateGeneratorDemo.Mappers;

namespace PredicateGeneratorDemo.Framework
{
    public class GameQueryer : IGameQueryer
    {
        private readonly IDataStore _dataStore;
        private readonly IPredicateGenerator<Game> _predicateGenerator;

        public GameQueryer(IDataStore dataStore, IPredicateGenerator<Game> predicateGenerator)
        {
            _dataStore = dataStore;
            _predicateGenerator = predicateGenerator;
        }
        public IEnumerable<GameDto> SearchGamesByPredicate(GameSearchParametersDto gameSearchParametersDto)
        {
            var predicate = _predicateGenerator.GeneratePredicate(gameSearchParametersDto).WithAdvancedPredicate(GenerateRatingRangePredicate(gameSearchParametersDto)).Compile();
            return _dataStore.GetGames().Where(predicate).ToGameDtos();
        }

        private Expression<Func<Game, bool>> GenerateRatingRangePredicate(GameSearchParametersDto gameSearchParametersDto)
        {
            if (gameSearchParametersDto.MinRating.HasValue && gameSearchParametersDto.MaxRating.HasValue)
            {
                return GetMoreThanMinRatingExpression(gameSearchParametersDto.MinRating.Value).And(GetLessThanMaxRatingExpression(gameSearchParametersDto.MaxRating.Value));
            }

            if (gameSearchParametersDto.MinRating.HasValue)
            {
                return GetMoreThanMinRatingExpression(gameSearchParametersDto.MinRating.Value);
            }

            if (gameSearchParametersDto.MaxRating.HasValue)
            {
                return GetLessThanMaxRatingExpression(gameSearchParametersDto.MaxRating.Value);
            }

            return o => true;

            Expression<Func<Game, bool>> GetMoreThanMinRatingExpression(int minRating)
            {
                return o => o.Rating >= minRating;
            }

            Expression<Func<Game, bool>> GetLessThanMaxRatingExpression(int maxRating)
            {
                return o => o.Rating <= maxRating;
            }
        }
    }
}
