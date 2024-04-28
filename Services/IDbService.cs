using APBD5.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBD5.Services
{
    public interface IDbService
    {
        public Task<int> AddProductToWarehouseAsync(Sequence sequence);
        public void AddProductToWarehouseProcedureAsync(Sequence sequence);
    }

}
