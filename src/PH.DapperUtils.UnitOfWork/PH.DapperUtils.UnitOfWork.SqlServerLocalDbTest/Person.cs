using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork.SqlServerLocalDbTest
{
	public class Person
	{
		[Key]
		public int Id { get; set; }

		public string? FirstName { get; set; }
		public string? LastName  { get; set; }



		internal static string Table => "persons";

		/// <summary>
		/// SQL select
		/// </summary>
		/// <value>
		/// select Id, FirstName, LastName from persons
		/// </value>
		internal static string SqlAll => $"select Id, FirstName, LastName from {Table} ";
	}


	public class ClassRoom
	{
		[Key]
		public Guid   Id   { get; set; }
		public string Name { get; set; }

		internal static string Table => "classrooms";

		/// <summary>
		/// Gets the SQL all.
		/// </summary>
		/// <value>
		/// select Id, Name from classrooms
		/// </value>
		internal static string SqlAll => $"select Id, Name from {Table} ";
	}

	public class Student
	{
		[Key]
		public Guid Id { get; set; }

		public int PersonId { get; set; }

		public Guid? ClassRoomId { get; set; }

		internal static string Table => "students";

		/// <summary>
		/// Gets the SQL all.
		/// </summary>
		/// <value>
		/// select Id, PersonId, ClassRoomId from students
		/// </value>
		internal static string SqlAll => $"select Id, PersonId, ClassRoomId from {Table}";
	}
}