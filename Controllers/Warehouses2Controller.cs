using APBD5.Models;
using APBD5.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD5.Controllers
{
    [ApiController]
    [Route("api/warehouses2")]
    public class Warehouses2Controller : ControllerBase
    {
        private IDbService _dbService;

        public Warehouses2Controller(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouseProcedureAsync([FromBody] Sequence sequence)
        {
            _dbService.AddProductToWarehouseProcedureAsync(sequence);
            return Ok();
        }
    }

}
