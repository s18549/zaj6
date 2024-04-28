using APBD5.Models;
using APBD5.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace APBD5.Services
{
    public class DbService : IDbService
    {
        private readonly string _connectionString =
            "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s21142;Integrated Security=True";

        public async Task<int> AddProductToWarehouseAsync(Sequence sequence)
        {
            var idProductResult = 0;
            var idWarehouseResult = 0;
            var returnId = 0;

            await using (var connection = new SqlConnection(_connectionString))
            {
                await using (var command = new SqlCommand("select count (*) idProductCount from Product where idProduct = @idProduct", connection))
                {
                    command.Parameters.AddWithValue("idProduct", sequence.IdProduct);

                    await connection.OpenAsync();
                    await using (var dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            idProductResult = int.Parse(dataReader["idProductCount"].ToString());
                        }
                    }

                    command.Parameters.Clear();
                    command.CommandText = "select count (*) from Warehouse where IdWarehouse = @idWarehouse";
                    command.Parameters.AddWithValue("idWarehouse", sequence.IdWarehouse);
                    await command.ExecuteNonQueryAsync();

                    await using (var dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            idWarehouseResult = int.Parse(dataReader["idWarehouseCount"].ToString());
                        }
                    }

                    if (sequence.Amount == 0 || idProductResult == 0 || idWarehouseResult == 0)
                    {
                        return -1;
                    }

                    var doesExists = 0;

                    command.Parameters.Clear();
                    command.CommandText = "select count (*) doesExists from \"Order\" o where o.IdProduct = @idProduct and o.Amount = @Amount and o.CreatedAt < @CreatedAt ";
                    command.Parameters.AddWithValue("@idWarehouse", sequence.IdWarehouse);
                    command.Parameters.AddWithValue("@Amount", sequence.IdWarehouse);
                    command.Parameters.AddWithValue("@CreatedAt", sequence.CreatedAt);
                    await command.ExecuteNonQueryAsync();

                    await using (var dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            doesExists = int.Parse((dataReader["doesExists"].ToString()));
                        }
                    }

                    if (doesExists == 0)
                        return -2;

                    command.Parameters.Clear();
                    command.CommandText = "update \"Order\" set FullfilledAt = @CreatedAt where IdWarehouse = @idWarehouse and Amount = @Amount";
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString());
                    command.Parameters.AddWithValue("@idWarehouse", sequence.IdWarehouse);
                    command.Parameters.AddWithValue("@Amount", sequence.Amount);
                    await command.ExecuteNonQueryAsync();

                    command.Parameters.Clear();
                    command.CommandText = "insert into Product_Warehouse values(@idWarehouse, @idProduct,(select IdOrder from \"Order\" where IdProduct = @idProduct and Amount = @Amount), @Amount,((select Price from Product where IdProduct = @idProduct) *  @Amount) , @CreatedAt)";
                    command.Parameters.AddWithValue("@idWarehouse", sequence.IdWarehouse);
                    command.Parameters.AddWithValue("@Amount", sequence.Amount);
                    command.Parameters.AddWithValue("@idProduct", sequence.IdProduct);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString());
                    await command.ExecuteNonQueryAsync();

                    command.Parameters.Clear();
                    command.CommandText = "select IdProductWarehouse from Product_Warehouse where IdOrder=(select IdOrder from \"Order\" where IdProduct = @idProduct and Amount = @Amount)";
                    command.Parameters.AddWithValue("@idProduct", sequence.IdProduct);
                    command.Parameters.AddWithValue("@Amount", sequence.Amount);
                    await command.ExecuteNonQueryAsync();

                    await using (var dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            returnId = int.Parse(dataReader["IdProductWarehouse"].ToString());
                        }
                    }
                }
                connection.Close();
            }
            return returnId;
        }

        public async void AddProductToWarehouseProcedureAsync(Sequence sequence)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var command = new SqlCommand("AddProductToWarehouse", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@idProduct", sequence.IdProduct);
            command.Parameters.AddWithValue("@idWarehouse", sequence.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", sequence.Amount);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString());
            await command.ExecuteNonQueryAsync();

            connection.Close();

        }
    }



}
