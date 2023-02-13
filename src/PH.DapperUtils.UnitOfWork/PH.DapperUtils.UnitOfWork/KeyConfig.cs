using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{
    /// <summary>
    /// Key Field config
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="PH.DapperUtils.UnitOfWork.FieldConfig{T}" />
    public class KeyConfig<T> : FieldConfig<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyConfig{T}"/> class.
        /// </summary>
        /// <param name="propertyAccessor">The property accessor.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="isAssigned">if set to <c>true</c> [is assigned].</param>
        public KeyConfig(Func<T, object> propertyAccessor, string fieldName, bool isAssigned): base(propertyAccessor, fieldName, true, isAssigned)
        {
            
        }
    }
}