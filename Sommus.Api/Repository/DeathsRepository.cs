using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Serilog;
using Sommus.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sommus.Api.Repository
{
    public class DeathsRepository : IDeathsRepository
    {
        private readonly IConfiguration _configuration;

        public DeathsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Connection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("SommusConnection").Value;
            return connection;
        }

        public async Task<int> Add(Deaths Deaths)
        {
            var connectionString = this.Connection();
            int status = 0;
            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "INSERT INTO Deaths(Date, Cases) VALUES(@Date, @Cases);";
                status = await con.ExecuteAsync(query, Deaths);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return status;
        }

        public async Task<int> AddAll(List<Deaths> Deaths)
        {
            var connectionString = this.Connection();
            int status = 0;
            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "INSERT INTO Deaths(Date, Cases) VALUES ";
                foreach (var Death in Deaths)
                {
                    query += $"(\"{Death.Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss")}\", {Death.Cases}),";
                }
                query = query.Substring(0, query.Length - 1);
                query += ";";
                status = await con.ExecuteAsync(query);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return status;
        }

        public async Task<Deaths> Get(string date)
        {
            var connectionString = this.Connection();
            Deaths Deaths = new Deaths();

            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = $"SELECT * FROM Deaths WHERE Date ='{date}'";
                Deaths = await con.QueryFirstOrDefaultAsync<Deaths>(query);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return Deaths;
        }

        public async Task<Deaths> GetLast()
        {
            var connectionString = this.Connection();
            Deaths deaths = new Deaths();

            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "SELECT * FROM Deaths ORDER BY Date DESC LIMIT 1";
                deaths = await con.QueryFirstOrDefaultAsync<Deaths>(query);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return deaths;
        }
    }
}
