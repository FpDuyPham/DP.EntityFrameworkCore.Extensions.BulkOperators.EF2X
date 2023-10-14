# DP.EntityFrameworkCore.Extensions.BulkOperators.EF2X
## For the latest .NET version use the new lib: DP.EntityFrameworkCore.Extensions.BulkOperators.EF
This repository provides a batch operator extension for Entity Framework Core 2.x, enabling efficient bulk operations. Please note that this library has been specifically tested with EF Core 2.0.0.
## Must config Implement ITableNameResolver
```csharp
public class MyEntity : ITableNameResolver
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? NullData { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? MySonEntityId { get; set; }
        public virtual MySonEntity MySonEntity { get; set; }

        public string GetTableName()
        {
            return "dbo.MyEntities"; or return "MyEntities"; 
        }
    }
```
## Some configs
### Configuration of Insert Fields
#### Implement IPropertyResolver
You have the option to limit the fields to be inserted, ensuring that the specified fields cannot be null. To achieve this, the entity should implement the IBulkPropertyResolver interface and define the fields to be included using the GetFields method.

Example of limiting insert fields to Name, IsDeleted, and CreatedDate:

```csharp
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
```
In this example, only the specified fields (Name, IsDeleted, CreatedDate) will be included during bulk insert operations.
## Usage

### Bulk Import

The `BulkImportAsync` method allows for bulk importing of entities into the database. There are two available overloads:

- Using a connection string:
  ```csharp
  await dbContext.BulkImportAsync<TEntity>(string connectionString, IEnumerable<TEntity> entities, int batchSize = 10000);
  await connectionString.BulkImportAsync<TEntity>(IEnumerable<TEntity> entities, int batchSize = 10000)
