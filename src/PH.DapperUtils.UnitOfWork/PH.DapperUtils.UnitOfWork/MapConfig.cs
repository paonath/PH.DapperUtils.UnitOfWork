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
        public bool CustomSetup { get; internal set; }

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
            if (CustomSetup)
            {
                return false;
            }

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
            
            var param = new Dictionary<string, object>();
            var keys  = Fields.Where(x => x.IsKey).OrderBy(x => x.SqlFieldName).ToArray();
            if (keys.Length == 1)
            {
                sql.AppendFormat(" where {0} = @{1}", keys[0].SqlFieldName,
                                 keys[0].PropertyName);
            }
            else
            {
                sql.Append(" where 1=1");
                foreach (var fieldConfig in keys)
                {
                    sql.AppendFormat(" and {0} = @{1}", fieldConfig.SqlFieldName,
                                     fieldConfig.PropertyName);
                    if (null != entity)
                    {
                        param.Add(fieldConfig.PropertyName, fieldConfig.GetValue(entity));
                    }

                }
            }

            

            return (sql.ToString(), param);
        }

        internal string BuildInsert<T>(T entity) where T : class
        {
            StringBuilder sql = new StringBuilder();


            sql.AppendFormat(" insert into {0} (", TableName);
            var fieldsToInsert = Fields.Where(x => !x.IsKey || x.IsAssignedIfKey)
                                       .OrderBy(x => x.SqlFieldName)
                                       .ToArray();
            sql.AppendFormat(" {0} )", string.Join(", ", fieldsToInsert.Select(x => x.SqlFieldName)));

            sql.Append(" values (");

            sql.AppendFormat(" {0} )", string.Join(", ", fieldsToInsert.Select(x => $"@{x.PropertyName}")));

            return sql.ToString();
        }

        internal string BuildUpdate<T>(T entity) where T : class
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(" update {0} SET ", TableName);

            var fieldsToInsert = Fields.Where(x => !x.IsKey || x.IsAssignedIfKey)
                                       .OrderBy(x => x.SqlFieldName)
                                       .ToArray();
            bool first = true;
            foreach (var fieldConfig in fieldsToInsert)
            {
                

                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(" , ");
                }

                sql.AppendFormat(" {0} = @{1} ", fieldConfig.SqlFieldName, fieldConfig.PropertyName);
            }

            sql.AppendFormat(" {0}", BuildWhere());

            return sql.ToString();
        }

    }
}