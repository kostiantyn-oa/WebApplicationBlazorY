using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public interface ICollectionRepository
    {
        Task<IEnumerable<Collection>> GetAllAsync();
        Task<Collection> GetByIdAsync(int id);
        Task<Collection> AddAsync(Collection collection);
        Task UpdateAsync(int id, Collection collection);
        Task DeleteAsync(int id);
        Task<bool> CollectionExistsAsync(int id);
    }
}