using restaraunt.Core.Entities;

namespace restaraunt.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Task AddAsync(OrderEntity user);

    }
}