using Core.Entities;
using Ardalis.Specification;

namespace Core.Specifications
{
    public static class ModeratingTables
    {
        public class All : Specification<ModeratingTable>
        {
            public All()
            {
                Query.OrderBy(mt => mt.TableName);
            }
        }

        public class ById : Specification<ModeratingTable>
        {
            public ById(int id)
            {
                Query.Where(mt => mt.Id == id);
            }
        }

        public class ByName : Specification<ModeratingTable>
        {
            public ByName(string tableName)
            {
                Query.Where(mt => mt.TableName == tableName);
            }
        }
    }
}
