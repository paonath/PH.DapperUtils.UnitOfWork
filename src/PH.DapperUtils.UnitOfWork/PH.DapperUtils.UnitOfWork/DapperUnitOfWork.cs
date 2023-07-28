#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

#endregion

namespace PH.DapperUtils.UnitOfWork
{
    /// <summary>
    ///     Dapper Unit Of Work
    ///     <para>
    ///         <see cref="IDisposable">IDisposable Wrapper</see> for hiding <see cref="IDbConnection" /> and
    ///         <see cref="IDbTransaction" /> exposing only dapper method for data manipulation
    ///     </para>
    /// </summary>
    /// <seealso cref="PH.DapperUtils.UnitOfWork.DapperBase" />
    /// <seealso cref="PH.DapperUtils.UnitOfWork.IDapperUnitOfWork" />
    public class DapperUnitOfWork : DapperBase, IDapperUnitOfWork
    {
        private string _uowOp = string.Empty;

        /// <summary>
        ///     Trows if inactive.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unit OF Work not Active - DapperUnitOfWork</exception>
        /// <exception cref="System.ObjectDisposedException">DapperUnitOfWork - Unit Of Work already perform a {_uowOp}</exception>
        public virtual void TrowIfInactive()
        {
            if (!Active)
            {
                throw new InvalidOperationException("Unit OF Work not Active",
                                                    new ObjectDisposedException(nameof(DapperUnitOfWork),
                                                                                $"Unit Of Work already perform a {_uowOp}"));
            }
        }

        /// <summary>
        ///     Throws if read only. (IsolationLevel.Chaos,IsolationLevel.ReadUncommitted,IsolationLevel.Unspecified)
        /// </summary>
        /// <exception cref="System.NotSupportedException">CRUD Operation not allowed: IsolationLevel</exception>
        public override void ThrowIfReadOnly()
        {
            base.ThrowIfReadOnly();
            TrowIfInactive();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DapperUnitOfWork" /> class.
        /// </summary>
        /// <param name="dbConnection">The database connection.</param>
        /// <param name="dbTransaction">The database transaction.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="System.ArgumentNullException">
        ///     dbConnection
        ///     or
        ///     dbTransaction
        ///     or
        ///     id
        /// </exception>
        public DapperUnitOfWork(IDbConnection dbConnection, IDbTransaction dbTransaction, string id,
                                CancellationToken cancellationToken = default(CancellationToken)) : base(dbConnection,
                                                                                                         dbTransaction, id, cancellationToken)
        {
        }


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Commits the database transaction.
        /// </summary>
        public void Commit()
        {
            Active = false;
            _uowOp = nameof(Commit);
            DbTransaction.Commit();
        }

        /// <summary>
        ///     Rolls back a transaction from a pending state.
        /// </summary>
        public void RollBack()
        {
            Active = false;
            _uowOp = nameof(RollBack);
            DbTransaction.Rollback();
        }


        #region INSERT / UPDATE / DELETE

        internal virtual async Task<T> InsertEntityAsync<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var fields = GetTableConfigConfig(entity);

            if (fields.CanUseDapperContrib)
            {
                ThrowIfReadOnly();
                await DbConnection.InsertAsync(entity,
                                               DbTransaction, commandTimeout);
                return entity;
            }

            var insert = fields.BuildInsert(entity);

            await DbConnection.ExecuteAsync(insert, entity, DbTransaction, commandTimeout);
            var result = await Read(entity, commandTimeout);
            return result;
        }

        protected internal async Task<T> Read<T>(T entity, int? commandTimeout = null) where T : class
        {
            var fields      = GetTableConfigConfig(entity);
            var whereBinded = fields.BuildWhereBinded(entity);
            var query       = $"{fields.BuildSelect()} {whereBinded.SqlWhere} ;";
            var entityRead =
                await DbConnection.QuerySingleOrDefaultAsync<T>(query, whereBinded.WhereParam,
                                                                DbTransaction,
                                                                commandTimeout, CommandType.Text);
            return entityRead;
        }

        internal virtual async Task<T> UpdateEntityAsync<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var fields = GetTableConfigConfig(entity);
            if (fields.CanUseDapperContrib)
            {
                await DbConnection.UpdateAsync(entity,
                                               DbTransaction, commandTimeout);
            }
            else
            {
                var update = fields.BuildUpdate(entity);
                await DbConnection.ExecuteAsync(update, entity, DbTransaction, commandTimeout);
            }


            return entity;
        }

        internal virtual async Task<bool> DeleteEntityAsync<T>(T entity, int? commandTimeout = null)
            where T : class =>
            await DbConnection.DeleteAsync(entity, DbTransaction, commandTimeout);

        internal virtual async Task<bool> DeleteEntityByIdAsync<T, TKey>(
            Func<TKey, Task<T>> entityById, TKey key, int? commandTimeout = null)
            where T : class where TKey : IEquatable<TKey>
        {
            var e = await entityById.Invoke(key);
            return await DeleteEntityAsync(e, commandTimeout);
        }

        internal virtual T Insert<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = InsertEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        internal virtual T Update<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = UpdateEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }


        internal virtual bool DeleteEntity<T>(T entity, int? commandTimeout = null)
            where T : class
        {
            var r = DeleteEntityAsync(entity, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        internal virtual bool DeleteEntityById<T, TKey>(Func<TKey, Task<T>> entityById, TKey key,
                                                        int? commandTimeout = null)
            where T : class where TKey : IEquatable<TKey>
        {
            var r = DeleteEntityByIdAsync(entityById, key, commandTimeout).GetAwaiter().GetResult();
            return r;
        }

        #endregion
    }
}