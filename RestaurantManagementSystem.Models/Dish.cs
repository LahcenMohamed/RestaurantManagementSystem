using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Models
{
    public sealed class Dish
    {
        public int Id { get; set; }
        [Required]
        [Length(3,60)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Range(10,1_000_000_000)]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public List<Order> Orders { get; set; }

    }
}
