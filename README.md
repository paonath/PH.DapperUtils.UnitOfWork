# PH.DapperUtils.UnitOfWork
DapperUnitOfWork is a `IDisposable` wrapper for hiding `IDbConnection` and `IDbTransaction` exposing only dapper method for data manipulation and `Commit()` or `RollBack()`

## How to use

1) open a new instance of `IDbConnection`
2) start a `IDbTransaction` from opened connection
3) init new instance of `DapperUnitOfWork` passing connection, transaction, and a string identifier
> ```csharp 
> var uow = new var uow = new DapperUnitOfWork(connection, transaction, "some id...");
> ```


4) pay attention: on disposing `DapperUnitOfWork` your  `IDbConnection` and `IDbTransaction` are already disposed

## Examples

`DapperUnitOfWork` 

### Init new Unit Of Work for MySql (based on [MySqlConnector](https://github.com/mysql-net/MySqlConnector))

```csharp
var  connection = new MySqlConnector.MySqlConnection();
connection.ConnectionString =	"server=***;database=dapperutils.unitofwork;user=***;password=***;SslMode=none";
connection.Open();
var tr = connection.BeginTransaction(IsolationLevel.ReadCommitted);

var uow = new DapperUnitOfWork(connection, tr, $"{Guid.NewGuid()}", CancellationTokenSource.Token);
//other code omitted...
```

### Query Example

```csharp
//other code omitted...
if (Uow is DapperUnitOfWork uow)
{
	var firstData = uow.QueryFirst<Person>("select * from persons where LastName = @LastName",
				                            new { LastName = "Last" });

}

if (Uow is DapperBase db)
{
	var array = db.Query<Person>("select * from persons where LastName = @LastName",
				                    new { LastName = "Last" }).ToArray();
}
```
