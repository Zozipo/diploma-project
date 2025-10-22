using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("TablePermissions")]
    public class TablePermission : BaseEntity<int>
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ModeratingTableId { get; set; }
        public ModeratingTable ModeratingTable { get; set; }
    }
}
