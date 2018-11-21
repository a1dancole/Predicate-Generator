using System.Collections.Generic;
using System.Threading.Tasks;
using PredicateGeneratorDemo.Data;
using PredicateGeneratorDemo.Data.Dtos;

namespace PredicateGeneratorDemo.Framework.Interfaces
{
    public interface IGameQueryer
    {
        IEnumerable<GameDto> SearchGamesByPredicate(GameSearchParametersDto gameSearchParametersDto);
    }
}