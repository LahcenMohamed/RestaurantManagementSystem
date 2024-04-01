using RestaurantManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Repositories.IReposioriesHelpers
{
    public interface IDishRepository : IBaseRepository<Dish>
    {
        public Task<Dish> GetByNameAsync(string Name);

    }
}
