using Dapper;
using Microsoft.Extensions.Options;
using Ncr.DeliveryKnapsack.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Ncr.DeliveryKnapsack.Repositories
{
    public class Registration
    {
        public string EMail { get; set; }

        public string Name { get; set; }

        public string OpenDoors { get; set; }
    }

    public class RegistrationRepository
    {
        private readonly DbConfiguration _dbConfiguration;

        public RegistrationRepository(IOptions<DbConfiguration> dbConfiguration)
        {
            _dbConfiguration = dbConfiguration?.Value ?? throw new ArgumentNullException(nameof(dbConfiguration));
        }

        public async Task InsertRegistration(Registration registration)
        {
            using (var conn = new NpgsqlConnection(_dbConfiguration.ConnectionString))
            {
                await conn.ExecuteAsync(
                    @"INSERT INTO ""Registrations""
                    (""EMail"", ""Name"", ""OpenDoors"")
                    VALUES (@EMail, @Name, @OpenDoors)",
                    registration);
            }
        }
    }
}