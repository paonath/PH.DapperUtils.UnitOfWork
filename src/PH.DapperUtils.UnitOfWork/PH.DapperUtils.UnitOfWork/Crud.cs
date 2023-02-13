using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace PH.DapperUtils.UnitOfWork
{
    
    public  class Crud
    {
        internal readonly DapperUnitOfWork Uow;


       


        public Crud(DapperUnitOfWork uow)
        {
            this.Uow = uow;
        }

        /// <summary>
        /// Throws Exception if read only.
        /// </summary>
        public void ThrowIfReadOnly() => Uow.ThrowIfReadOnly();

        #region INSERT / UPDATE / DELETE

        public  async Task<T> InsertEntityAsync<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            return await Uow.InsertEntityAsync(entity, commandTimeout);
        }

        public async  Task<T> UpdateEntityAsync<T>(T entity,  int? commandTimeout = null)
            where T : class
        {
            return await Uow.UpdateEntityAsync(entity, commandTimeout);
        }

        public async  Task<bool> DeleteEntityAsync<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            return await this.Uow.DbConnection.DeleteAsync(entity, this.Uow.DbTransaction, commandTimeout);
        }

        public async  Task<bool> DeleteEntityByIdAsync<T, TKey>(Func<TKey, Task<T>> entityById, TKey key, int? commandTimeout = null)
            where T : class where TKey : IEquatable<TKey>
        {
            var r = await DeleteEntityByIdAsync<T, TKey>(entityById, key, commandTimeout);
            return r;
        }

        public T Insert<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = this.InsertEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        public  T Update<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = this.UpdateEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }


        public  bool DeleteEntity<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = this.DeleteEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        public  bool DeleteEntityById<T, TKey>(Func<TKey, Task<T>> entityById, TKey key, int? commandTimeout = null)
            where T : class where TKey : IEquatable<TKey>
        {
            var r = DeleteEntityByIdAsync<T,TKey>(entityById, key, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        #endregion
    }
}