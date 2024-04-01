using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories.IReposioriesHelpers
{
    public interface IBaseRepository<Model>
    {
        public Task CreateAsync(Model model);
        public Task UpdateAsync(Model model);
        public Task DeleteAsync(int Id);
        public IAsyncEnumerable<Model> GetAllAsync();
        public Task<Model> GetByIdAsync(int id);
        public IAsyncEnumerable<Model> Search(string item);

    }
}
