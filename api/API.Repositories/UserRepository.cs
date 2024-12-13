using API.Contracts.Services;
using API.Entities.Dedicated;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataService _dataService;

        public UserRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Fren> GetUserDetailsByIdAsync(int userId)
        {
            string query = "SELECT * FROM tblMembers WHERE Id = @Id";
            return await _dataService.QueryFirstOrDefaultAsync<Fren>(query, new { UserId = userId });
        }

        public async Task<int> GetNextId()
        {
            string query = "SELECT ISNULL(MAX(Id), 0) AS MaxVal FROM tblMembers";
            return await _dataService.QueryFirstOrDefaultAsync<int>(query);
        }

        public async Task<Fren> GetUserByProp(string propertyName, object value)
        {
            string query = $"SELECT * FROM tblMembers WHERE {propertyName} = @Value";
            int count = await _dataService.ExecuteAsync(query, new { Value = value });

            return await _dataService.QueryFirstOrDefaultAsync<Fren>(query, new { Value = value });
        }

        public async Task<Fren> AddUserAsync(Fren member)
        {
            member.Id = (await GetNextId() + 1);

            var query = @"
                INSERT INTO [dbo].[tblMembers] 
                (Id, FirstName, LastName, Username, Email, Avatar, Role, GoogleId, IsActive, ExpPoints, TimeSpent, DateAdded, DateEdited) 
                VALUES 
                (@Id, @FirstName, @LastName, @Username, @Email, @Avatar, @Role, @GoogleId, @IsActive, @ExpPoints, @TimeSpent, @DateAdded, @DateEdited)";

            await _dataService.ExecuteAsync(query, member);
            return member;
        }

        public async Task UpdateTimeSpent(int Id)
        {
            var query = @"
            UPDATE [dbo].[tblMembers] 
            SET TimeSpent = TimeSpent + 5
            WHERE Id = @Id;";
            await _dataService.ExecuteAsync(query, new { Id = Id });
        }



    }
}
