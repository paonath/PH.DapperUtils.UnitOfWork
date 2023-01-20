using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork.SqlServerLocalDbTest
{
	public class UnitTest : LocalDbTest
	{
		[Fact]
		public async void InsertPerson()
		{
			var uow = await GetUnitOfWorkAsync();

			var p = await InsertAPersonNoCommit(uow);
			Assert.True(uow.Active);
			Assert.True(uow.Disposed == false);
			Assert.True(uow.ReadOnly == false);

			uow.Commit();
			Assert.False(uow.Active);
			Assert.False(uow.Disposed);
			Assert.True(p.Id > 0);

			uow.Dispose();
			Assert.True(uow.Disposed);

		}

		[Fact]
		public async void ProvideStudent()
		{
			using (var uow = await GetUnitOfWorkAsync())
			{
				var u = Guid.NewGuid();
				var p = new Person() { FirstName = $"first-{u:N}", LastName = $"last-{u}" };
				await uow.InsertAsync(p);

				var c = new ClassRoom() { Id = Guid.NewGuid(), Name = $"A sample class {DateTime.Now.Ticks}" };
				await uow.InsertAsync(c);

				var s = new Student() { Id = Guid.NewGuid(), PersonId = p.Id };
				await uow.InsertAsync(s);

				var test = await uow.QueryFirstOrDefaultAsync<Student>(Student.SqlAll + " where Id=@ID", new { ID = s.Id });
				Assert.NotNull(test);

				test.ClassRoomId = c.Id;
				await uow.UpdateAsync(test);

				uow.Commit();
			}
		}

		private async Task<Person> InsertAPersonNoCommit(DapperUnitOfWork uow)
		{
			var p = new Person() { FirstName = "Paolo", LastName = "Last" };
			await uow.InsertAsync(p);

			return p;
		}

		[Fact]
		public void QuerySomeData()
		{




		}
	}
}