using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork.SqlServerLocalDbTest
{
	[TableName("persons")]
    public class PersonWithAttributeMap
    {
		[FieldName("Id", true)]
        public int    Key        { get; set; }
		
        [FieldName("FirstName")]
        public string Identifier { get; set; }

        [FieldName("LastName")]
        public string AlternateIdentifier { get; set; }

		[ExcludedField]
        public Guid? NotMappedField { get; set; }
    }


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
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
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
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
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

    public class StudentExt : Student
    {
        public ClassRoom ClassRoom { get; set; }
    }


    public class ClassRoomExt : ClassRoom
    {
        public List<Student> Students { get; set; }
    }
}