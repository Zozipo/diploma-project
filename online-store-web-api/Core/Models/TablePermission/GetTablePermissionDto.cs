namespace Core.Models.Permission
{
    public class GetTablePermissionDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ModeratingTableId { get; set; }
    }
}