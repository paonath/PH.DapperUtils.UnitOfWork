using System.Data;

namespace PH.DapperUtils.UnitOfWork.SqlServerLocalDbTest
{
	public abstract class LocalDbTest
	{
		private const string connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DapperUtlisUnitOfWorkDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		protected readonly CancellationTokenSource CancellationTokenSource;

		protected LocalDbTest()
		{
			CancellationTokenSource = new CancellationTokenSource();
		}

		public DapperUtils.UnitOfWork.DapperUnitOfWork GetDapperUnitOfWork()
		{
			var u = GetUnitOfWorkAsync().GetAwaiter().GetResult();
			return u;
		}

		public async Task<DapperUtils.UnitOfWork.DapperUnitOfWork> GetUnitOfWorkAsync()
		{
			var conn = new System.Data.SqlClient.SqlConnection(connString);
			await conn.OpenAsync(CancellationTokenSource.Token);
			var tran = await conn.BeginTransactionAsync(IsolationLevel.ReadCommitted, CancellationTokenSource.Token);

			return new DapperUnitOfWork(conn, tran, $"{Guid.NewGuid()}", CancellationTokenSource.Token);

		}
	}
}