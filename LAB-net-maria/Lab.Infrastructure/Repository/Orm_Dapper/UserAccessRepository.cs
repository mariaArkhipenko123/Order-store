using System.Data;
using Dapper;
using Lab.Application.Interfaces;
using Lab.Domain.Entities;
using Lab.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Lab.Infrastructure.Repository.Orm_Dapper
{
    public class UserAccessRepository : IUserAccessRepository
    {
        private readonly string _connectionString;

        public UserAccessRepository(PostgresDbContext context)
        {
            _connectionString = context.Database.GetDbConnection().ConnectionString;
        }
        public async Task<IEnumerable<UserAccess>> GetAllAsync()
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                return await dbConnection.QueryAsync<UserAccess>("SELECT * FROM \"UserAccesses\"");
            }
        }
        public async Task<UserAccess> GetByIdAsync(Guid id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<UserAccess>("SELECT * FROM \"UserAccesses\" WHERE Id = @Id", new { Id = id });
            }
        }
        public async Task AddAsync(UserAccess userAccess)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sqlQuery = @"
                            INSERT INTO public.""UserAccesses"" (""Id"", ""DeviceId"", ""UserId"", ""DeviceName"", ""IP"", ""Agent"", ""AccessTime"")
                            VALUES (@Id, @DeviceId, @UserId, @DeviceName, @IP, @Agent, @AccessTime);";
                await dbConnection.ExecuteAsync(sqlQuery, userAccess);
            }
        }
        public async Task UpdateAsync(UserAccess userAccess)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                var sqlQuery = @"
                        UPDATE public.""UserAccesses""
                        SET ""DeviceId"" = @DeviceId, ""DeviceName"" = @DeviceName, ""IP"" = @IP, ""Agent"" = @Agent, 
                            ""AccessTime"" = @AccessTime
                        WHERE ""Id"" = @Id;";
                await dbConnection.ExecuteAsync(sqlQuery, userAccess);
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                await dbConnection.ExecuteAsync(@"DELETE FROM public.""UserAccesses"" WHERE ""Id"" = @Id;", new { Id = id });
            }
        }
    }
}
