using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PredicateGeneratorDemo.Data
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Genre Genre { get; set; }
        public int Rating { get; set; }
        public bool Multiplayer { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
