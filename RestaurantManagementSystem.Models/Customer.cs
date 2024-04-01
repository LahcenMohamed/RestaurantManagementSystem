using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Models
{
    public sealed class Customer
    {
        public int Id { get; set; }
        [Required]
        [Length(3, 60)]
        public string FullName { get; set; }
        [Required]
        [Length(3, 20)]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool IsLoyal { get; set; }
        public List<Order> Orders { get; set; }
    }
}
