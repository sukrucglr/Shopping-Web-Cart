using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PinCode { get; set; }
        //public int UserId { get; set; }
        //public int RoleId { get; set; }
        //public int Id { get; set; }
        //public string UserName { get; set; }
        //public string NormalizedUserName { get; set; }
        //public string Email { get; set; }
        //public string NormalizedEmail { get; set; }
        //public string EmailConfirmed { get; set; }
        //public string PasswordHash { get; set; }
        //public string SecurityStamp { get; set; }

        //public int ConcurrencyStamp { get; set; }
        //public string PhoneNumber { get; set; }
        //public bool PhoneNumberConfirmed { get; set; }
        //public bool TwoFactorEnabled { get; set; }
        //public DateTime LockoutEnd { get; set; }
        //public bool LockoutEnabled { get; set; }

        //public int AccessFailedCount { get; set; }
    }
}
