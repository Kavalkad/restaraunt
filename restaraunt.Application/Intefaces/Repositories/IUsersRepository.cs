using restaraunt.Application.Services;
using restaraunt.Core.Entities;

namespace restaraunt.Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        public Task AddAsync(UserEntity user);
        public Task<UserEntity> GetByEmail(string email);

    }
}