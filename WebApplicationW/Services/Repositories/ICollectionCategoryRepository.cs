using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplicationW.Models;

namespace WebApplicationW.Services.Repositories
{
    public interface ICollectionCategoryRepository
    {
        Task<IEnumerable<CollectionCategory>> GetAllAsync();
    }
}