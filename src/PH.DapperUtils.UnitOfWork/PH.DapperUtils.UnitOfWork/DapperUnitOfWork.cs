#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

#endregion

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// Dapper Unit Of Work
	/// </summary>
	/// <seealso cref="PH.DapperUtils.UnitOfWork.DapperBase" />
	/// <seealso cref="PH.DapperUtils.UnitOfWork.IDapperUnitOfWork" />
	public class DapperUnitOfWork :   DapperBase , IDapperUnitOfWork
	{
		private string _uowOp = string.Empty;

		/// <summary>
		/// Trows if inactive.
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
		/// Initializes a new instance of the <see cref="DapperUnitOfWork"/> class.
		/// </summary>
		/// <param name="dbConnection">The database connection.</param>
		/// <param name="dbTransaction">The database transaction.</param>
		/// <param name="id">The identifier.</param>
		/// <param name="cancellationToken">Cancellation token</param>
		/// <exception cref="System.ArgumentNullException">
		/// dbConnection
		/// or
		/// dbTransaction
		/// or
		/// id
		/// </exception>
		public DapperUnitOfWork(IDbConnection dbConnection, IDbTransaction dbTransaction, string id,
		                        System.Threading.CancellationToken cancellationToken = default(CancellationToken)) : base(dbConnection, dbTransaction, id, cancellationToken)
		{
			
		}
		

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public new void Dispose()
		{
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		public void Commit()
		{
			Active = false;
			_uowOp = nameof(Commit);
			DbTransaction.Commit();
			
		}

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		public void RollBack()
		{
			Active = false;
			_uowOp = nameof(RollBack);
			 DbTransaction.Rollback();
			 
		}
	}
}