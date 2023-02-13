using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{

    public class MapConfig
    {
        public Dictionary<Type, TableConfig> Tables { get; }

        public MapConfig()
        {
            Tables = new Dictionary<Type, TableConfig>();
        }
    }

    public class MapConfig<T> : MapConfig where T:class
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableNameAttribute : Attribute
    {
        public string        TableName;

        public TableNameAttribute(string tableName)
        {
            if (string.IsNullOrWhiteSpace(value: tableName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tableName));
            }

            TableName = tableName;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldNameAttribute : Attribute
    {
        public FieldConfig Field;

        public FieldNameAttribute(string sqlFieldName) : this(sqlFieldName, false)
        {

        }

        public FieldNameAttribute(string sqlFieldName, bool isKey, bool isAssignedIfKey = false) :
            this(new FieldConfig() { SqlFieldName = sqlFieldName, IsKey = isKey, IsAssignedIfKey = isAssignedIfKey })
        {
            if (string.IsNullOrWhiteSpace(value: sqlFieldName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(sqlFieldName));
            }
        }

        public FieldNameAttribute(FieldConfig field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Field = field;
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcludedFieldAttribute : Attribute
    {
    }

    public class TableConfig
    {
        /// <summary>
        /// Gets or sets the name of the SQL table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string           TableName { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        /// <value>
        /// The fields.
        /// </value>
        public FieldConfig[] Fields    { get; set; }

        internal bool CanUseDapperContrib => GetCanUseDapperContrib();

        private bool GetCanUseDapperContrib()
        {
            var key      = Fields.Count(x => x.IsKey);
            var assigned = Fields.Count(x => x.IsAssignedIfKey);
            return key > 0 && assigned == 0;
        }

        internal string BuildSelect()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" select ");
            sql.AppendFormat(" {0}", string.Join(", ", Fields.Select(x => x.SqlFieldName)));
            sql.AppendFormat(" from {0}", TableName);

            return sql.ToString();
        }

        internal string BuildWhere()
        {
            
            var r = BuildWhereBinded<object>(null);
            return r.SqlWhere;

        }

        internal (string SqlWhere, object WhereParam) BuildWhereBinded<T>(T entity) where T : class
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" where 1=1");
            var param = new Dictionary<string, object>();
            foreach (var fieldConfig in Fields.Where(x => x.IsKey))
            {
                sql.AppendFormat(" and {0} = @{1}", fieldConfig.SqlFieldName,
                                 fieldConfig.SqlFieldName.ToUpperInvariant());
                if (null != entity)
                {
                    param.Add(fieldConfig.SqlFieldName.ToUpperInvariant(), fieldConfig.GetValue(entity));
                }

            }

            return (sql.ToString(), param);
        }

        internal string BuildInsert<T>(T entity) where T : class
        {
            StringBuilder sql = new StringBuilder();


            sql.AppendFormat(" insert into {0} (", TableName);
            var fieldsToInsert = Fields.Where(x => !x.IsKey || x.IsAssignedIfKey).Select(x => x.SqlFieldName)
                                       .OrderBy(x => x)
                                       .ToArray();
            sql.AppendFormat(" {0} )", string.Join(", ", fieldsToInsert));

            sql.Append(" values (");

            sql.AppendFormat(" {0} )", string.Join(", ", fieldsToInsert.Select(x => $"@{x}")));

            return sql.ToString();
        }

    }
}