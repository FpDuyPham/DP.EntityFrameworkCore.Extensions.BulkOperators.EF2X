# DP.EntityFrameworkCore.Extensions.BulkOperators.EF2X
Add batch operator to Entity Framework Core 2.x
This lib has only been tested for EF Core 2.0.0

Use

await dbContext.BulkImportAsync<TEntity>(string connectionString, string destinationTableName, IEnumerable<TEntity> entities, int batchSize = 10000)

await connectionString.BulkImportAsync<TEntity>(string destinationTableName, IEnumerable<TEntity> entities, int batchSize = 10000)


config insert fields

-Limit fields
Note: When use limit fields, we have to return fields that cannot be null
Entity implement IBulkPropertyResolver
implement GetFields method
example only insert fields: Name, IsDeleted, CreatedDate: 

public class MyEntity : IPropertyResolver
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? NullData { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? MySonEntityId { get; set; }
        public virtual MySonEntity MySonEntity { get; set; }

        public string[] GetFields()
        {
            return new string[]
            {
                nameof(Id),
                nameof(Name),
                nameof(IsDeleted),
                nameof(CreatedDate)
            };
        }
    }
