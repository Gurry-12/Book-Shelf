using System.ComponentModel.DataAnnotations;

namespace Book_Shelf.Models
{
    public enum UserRole
    {
        Admin,
        Customer
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
