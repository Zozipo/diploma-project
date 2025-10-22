using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("ModeratingTables")]
    public class ModeratingTable : BaseEntity<int>
    {
        public string TableName { get; set; }
        public string Image { get; set; }
        public virtual ICollection<TablePermission> TablePermissions { get; set; } = new List<TablePermission>();
    }
}
