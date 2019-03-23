using Dapper;
using Microsoft.Extensions.Options;
using Ncr.DeliveryKnapsack.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ncr.DeliveryKnapsack.Repositories
{
    public class Solution
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EMail { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string Submission { get; set; }

        [Required]
        public decimal Length { get; set; }
    }

    public class SolutionlessSolution : IComparable
    {
        public string EMail { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public decimal Length { get; set; }

        public int CompareTo(object obj)
        {
            var sobj = obj as SolutionlessSolution;
            if (sobj != null)
            {
                return sobj.Length < this.Length ? -1 : 1;
            }

            return -1;
        }
    }

    public class SolutionRepository
    {
        private readonly DbConfiguration _dbConfiguration;

        public SolutionRepository(IOptions<DbConfiguration> dbConfiguration)
        {
            _dbConfiguration = dbConfiguration?.Value ?? throw new ArgumentNullException(nameof(dbConfiguration));
        }

        public async Task<IList<SolutionlessSolution>> GetBestSolutions()
        {
            using (var conn = new NpgsqlConnection(_dbConfiguration.ConnectionString))
            {
                return (await conn
                    .QueryAsync<SolutionlessSolution>(@"SELECT ""EMail"", ""Name"", ""Created"", ""Length"" FROM ""Solutions"""))
                    .GroupBy(s => s.EMail.ToLower())
                    .Select(g => g.Max())
                    .OrderBy(s => s.Length)
                    .ToList();
            }
        }

        public async Task InsertSolution(Solution solution)
        {
            using (var conn = new NpgsqlConnection(_dbConfiguration.ConnectionString))
            {
                await conn.ExecuteAsync(
                    @"INSERT INTO ""Solutions""
                    (""EMail"", ""Name"", ""Created"", ""Submission"", ""Length"")
                    VALUES (@EMail, @Name, @Created, @Submission, @Length)",
                    solution);
            }
        }
    }
}
