using restaraunt.Application.Interfaces.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using restaraunt.Core.Entities;

namespace restaraunt.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _dbcontext;
        public UsersRepository(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task AddAsync(UserEntity user)
        {
            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
        }
        public async Task<UserEntity> GetByEmail(string email)
        {
            var user = await _dbcontext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("Некорректный email");
            return user;
        }
    }
}