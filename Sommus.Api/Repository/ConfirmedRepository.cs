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
    public class ConfirmedRepository : IConfirmedRepository
    {
        private readonly IConfiguration _configuration;

        public ConfirmedRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Connection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("SommusConnection").Value;
            return connection;
        }

        public async Task<int> Add(Confirmed confirmed)
        {
            var connectionString = this.Connection();
            int status = 0;
            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "INSERT INTO Confirmed(Date, Cases) VALUES(@Date, @Cases);";
                status = await con.ExecuteAsync(query, confirmed);
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

        public async Task<int> AddAll(List<Confirmed> confirmeds)
        {
            var connectionString = this.Connection();
            int status = 0;
            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "INSERT INTO Confirmed(Date, Cases) VALUES ";
                foreach (var confirmed in confirmeds)
                {
                    query += $"(\"{confirmed.Date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss")}\", {confirmed.Cases}),";
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

        public async Task<Confirmed> Get(string date)
        {
            var connectionString = this.Connection();
            Confirmed confirmed = new Confirmed();

            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = $"SELECT * FROM Confirmed WHERE Date ='{date}'";
                confirmed = await con.QueryFirstOrDefaultAsync<Confirmed>(query);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return confirmed;
        }

        public async Task<Confirmed> GetLast()
        {
            var connectionString = this.Connection();
            Confirmed confirmed = new Confirmed();

            using var con = new SqliteConnection(connectionString);
            try
            {
                con.Open();
                var query = "SELECT * FROM Confirmed ORDER BY Date DESC LIMIT 1";
                confirmed = await con.QueryFirstOrDefaultAsync<Confirmed>(query);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            finally
            {
                con.Close();
            }

            return confirmed;
        }
    }
}
