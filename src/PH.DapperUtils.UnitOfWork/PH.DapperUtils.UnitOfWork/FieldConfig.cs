using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{
    /// <summary>
    /// Field Config for Insert/Update/Delete op like DapperContrib
    /// </summary>
    public class FieldConfig
    {
        /// <summary>
        /// Gets or sets the name of the c# object property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        internal string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the SQL field.
        /// </summary>
        /// <value>
        /// The name of the SQL field.
        /// </value>
        public string SqlFieldName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is key; otherwise, <c>false</c>.
        /// </value>
        public bool IsKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is assigned if key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is assigned if key; otherwise, <c>false</c>.
        /// </value>
        public bool IsAssignedIfKey { get; set; }

        

    }

    public static class FieldConfigExtensions
    {
        public static object GetValue<T>(this FieldConfig fg, T instance) where T : class
        {
            var propAccesor = instance.GetType().GetProperties().First(x => x.Name == fg.PropertyName);
            return propAccesor.GetValue(instance, null);
        }

        public static object GetValue<T>(this FieldConfig<T> fg, T instance) where T : class
        {
            return fg.Accessor.Invoke(instance);
        }

    }

    
    
    public class FieldConfig<T> : FieldConfig where T : class
    {
        /// <summary>
        /// Gets or sets the accessor.
        /// </summary>
        /// <value>
        /// The accessor.
        /// </value>
        public Func<T, object> Accessor { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConfig{T}"/> class.
        /// </summary>
        /// <param name="propertyAccessor">The property accessor.</param>
        /// <param name="fieldName">Name of the field.</param>
        public FieldConfig(Func<T, object> propertyAccessor, string fieldName) : this(propertyAccessor, fieldName, false, false)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldConfig{T}"/> class.
        /// </summary>
        /// <param name="propertyAccessor">The property accessor.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isKey">if set to <c>true</c> [is key].</param>
        /// <param name="isAssignedIfKey">if set to <c>true</c> [is assigned if key].</param>
        public FieldConfig(Func<T, object> propertyAccessor, string fieldName, bool isKey, bool isAssignedIfKey)
        {
            Accessor        = propertyAccessor;
            SqlFieldName    = fieldName;
            IsKey           = isKey;
            IsAssignedIfKey = isAssignedIfKey;
        }
    }
}