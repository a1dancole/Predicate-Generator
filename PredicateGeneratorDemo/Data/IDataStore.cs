using System.Collections.Generic;

namespace PredicateGeneratorDemo.Data
{
    public interface IDataStore
    {
        IEnumerable<Game> GetGames();
    }
}