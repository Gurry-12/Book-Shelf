using Book_Shelf.Models;

namespace Book_Shelf.ViewModel
{
    public class UserDetailsViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; internal set; }
        public string? Phone { get; internal set; }
    }
}
