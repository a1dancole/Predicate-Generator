using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PredicateGeneratorDemo.Data.Dtos;
using PredicateGeneratorDemo.Framework;
using PredicateGeneratorDemo.Framework.Interfaces;

namespace PredicateGeneratorDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IGameQueryer _gameQueryer;

        public DemoController(IGameQueryer gameQueryer)
        {
            _gameQueryer = gameQueryer;
        }

        [HttpPost]
        [Route("api/[controller]/search")]
        public async Task<JsonResult> Search(GameSearchParametersDto searchParameters)
        {
            return new JsonResult(_gameQueryer.SearchGamesByPredicate(searchParameters));
        }
    }
}