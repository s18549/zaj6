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
    [Route("api/warehouses")]
    public class WarehousesController : ControllerBase
    {
        private IDbService _dbService;

        public WarehousesController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouseAsync([FromBody] Sequence sequence)
        {
            var returned = await _dbService.AddProductToWarehouseAsync(sequence);

            switch (returned)
            {
                case -1:
                    return NotFound("Wrong data (IdProduct/IdWarehouse/Amount");
                case -2:
                    return NotFound("Order has already been fulfilled or it does not exist");
                default:
                    return Ok("IdProduct_Warehouses : " + returned);
            }
        }
    }

}
