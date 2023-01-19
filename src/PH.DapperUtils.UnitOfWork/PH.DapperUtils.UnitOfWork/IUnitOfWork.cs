using System;
using System.Transactions;

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// Unit Of Work
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Commits the database transaction.
		/// </summary>
		void Commit();

		/// <summary>
		/// Rolls back a transaction from a pending state.
		/// </summary>
		void RollBack();
	}


}
