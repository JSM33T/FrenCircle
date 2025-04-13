using Dapper;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Models;
using FrenCircle.Infra.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Repositories
{
    public class ProfileRepository(IDapperFactory dapperFactory) : IProfileRepository
    {
        public async Task<UserProfileDetailsDto?> GetUserProfileById(int id)
        {
            using var conn = dapperFactory.CreateConnection();

            return await conn.QuerySingleOrDefaultAsync<UserProfileDetailsDto>(
                "[GetProfileDetailsById]",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

    }
}
