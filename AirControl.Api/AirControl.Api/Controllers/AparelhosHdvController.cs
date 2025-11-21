using AirControl.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AirControl.Api.Controllers
{
    [ApiController]
    [Route("api/aparelhos-hdv")]
    public class AparelhosHdvController : ControllerBase
    {
        // GET api/aparelhos-hdv
        [HttpGet]
        public ActionResult<IEnumerable<AparelhoHdv>> GetTodos()
        {
            return Ok(AparelhosHdvSeed.Itens);
        }

        // GET api/aparelhos-hdv/por-unidade/3
        [HttpGet("por-unidade/{unidade}")]
        public ActionResult<IEnumerable<AparelhoHdv>> GetPorUnidade(int unidade)
        {
            var itens = AparelhosHdvSeed.Itens
                .Where(x => x.UnidadeExterna == unidade)
                .ToList();

            return Ok(itens);
        }
    }
}
