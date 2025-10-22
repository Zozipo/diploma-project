using Core.Models.Permission;

namespace Core.Models.User
{
    public class GetUserDto
    {
        public string Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? Sex { get; set; }
        public string? Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public List<string> Roles { get; set; }
        public List<GetTablePermissionDto> Permissions { get; set; }
    }
}