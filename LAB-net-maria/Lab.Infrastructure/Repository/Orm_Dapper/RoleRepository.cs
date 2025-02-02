using System.Data;
using Npgsql;
using Dapper;
using Lab.Domain.Entities;
using Lab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Lab.Application.Interfaces;


namespace Lab.Infrastructure.Repository.Orm_Dapper
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(PostgresDbContext context)
        {
            _connectionString = context.Database.GetDbConnection().ConnectionString;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<Role>("SELECT * FROM AspNetRoles");
            }
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<Role>("SELECT * FROM AspNetRoles WHERE Id = @Id", new { Id = id });
            }
        }

        public async Task AddAsync(Role role)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sqlQuery = "INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES(@Id, @Name, @NormalizedName)";
                await dbConnection.ExecuteAsync(sqlQuery, role);
            }
        }

        public async Task UpdateAsync(Role role)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sqlQuery = "UPDATE AspNetRoles SET Name = @Name, NormalizedName = @NormalizedName WHERE Id = @Id";
                await dbConnection.ExecuteAsync(sqlQuery, role);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                await dbConnection.ExecuteAsync("DELETE FROM AspNetRoles WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
