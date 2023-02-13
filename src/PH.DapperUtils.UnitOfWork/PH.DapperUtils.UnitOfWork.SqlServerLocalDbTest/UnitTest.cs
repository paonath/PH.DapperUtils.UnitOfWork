using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
                var crud = new Crud(uow);
				var u    = Guid.NewGuid();
				var p    = new Person() { FirstName = $"first-{u:N}", LastName = $"last-{u}" };
                await crud.InsertAsync(p);

                var cId = Guid.NewGuid();

				var c = new ClassRoom() { Id = cId, Name = $"A sample class {DateTime.Now.Ticks}" };
				await crud.InsertAsync(c);

                var c2 = new ClassRoom() { Id = Guid.NewGuid(), Name = $"Another sample class {DateTime.Now.Ticks}" };
                await crud.InsertAsync(c2);

				var s = new Student() { Id = Guid.NewGuid(), PersonId = p.Id };
				await crud.InsertAsync(s);

				var test = await crud.QueryFirstOrDefaultAsync<Student>(Student.SqlAll + " where Id=@ID", new { ID = s.Id });
				Assert.NotNull(test);

				test.ClassRoomId = c.Id;
				await crud.UpdateAsync(test);

				uow.Commit();
			}
		}

        [Fact]
        public async void InsertClassWithManyStudents()
        {
            var cId = Guid.NewGuid();


            using var uow  = await GetUnitOfWorkAsync();
            var       crud = new Crud(uow);
            var       c    = new ClassRoom() { Id = cId, Name = $"A sample class {DateTime.Now.Ticks}" };
            await crud.InsertAsync(c);

            for (int i = 0; i < 15; i++)
            {
                var u = Guid.NewGuid();
                var p = new Person() { FirstName = $"{i}", LastName = $"{i}" };
                await crud.InsertAsync(p);

                var s = new Student() { Id = Guid.NewGuid(), PersonId = p.Id, ClassRoomId = cId};
                await crud.InsertAsync(s);

            }

            uow.Commit();

        }

        private async Task<Person> InsertAPersonNoCommit(DapperUnitOfWork uow)
		{
			var  p    = new Person() { FirstName = "Paolo", LastName = "Last" };
            Crud crud = new Crud(uow);
			await crud.InsertAsync(p);

			return p;
		}

		[Fact]
		public  void QuerySomeData()
        {
            using var uow  = GetUnitOfWorkAsync().GetAwaiter().GetResult();
            var crud = new Crud(uow);
			var       sql  = "select * from students inner join classrooms on classrooms.id = students.ClassRoomId";
            var       data =  crud.Query<StudentExt, ClassRoom,StudentExt>(sql, (s, c) => { s.ClassRoom = c; return s; } );

			Assert.NotNull(data);

        }


        [Fact]
        public void QueryListDataJoin()
        {
			//
            using var uow  = GetUnitOfWorkAsync().GetAwaiter().GetResult();
            var       guid = Guid.Parse("afd51131-b39a-4907-bf28-f7d7d1d2181f");
            var sql =
                "select * from classrooms left join students on classrooms.id = students.ClassRoomId where classrooms.Id = @MyId ";

			var data = new Crud(uow).Query <ClassRoomExt, List<Student>,ClassRoomExt >
                (sql, (c, s) => { c.Students = s;
                    return c;
                }, new { MyId = guid });


            var result = data.First();

        }

        [Fact]
        public async void FullCrud()
        {
            using var uow              = await GetUnitOfWorkAsync();
            var       crud             = new Crud(uow);
            var       id               = 0;

            var       personToFullCrud = new Person() { FirstName = "To Insert", LastName = "To Insert" };
            await crud.InsertEntityAsync(personToFullCrud);
            id = personToFullCrud.Id;

            Assert.True(id > 0);

            var tpUpd = "To Update";
            personToFullCrud.FirstName = tpUpd;
            personToFullCrud.LastName  = tpUpd;

            await crud.UpdateEntityAsync(personToFullCrud);

            var s = "select LastName from persons where Id = @Id";
            var r = await crud.QuerySingleAsync<string>(s, new { Id = id });

            Assert.Equal(tpUpd, r);

            var del = await crud.DeleteEntityAsync(personToFullCrud);

            var t = "select id from persons where Id = @Id";
            var y = await crud.QuerySingleOrDefaultAsync(t, new { Id = id });

            Assert.Null(y);


            Assert.True(del);
        }
    }
}