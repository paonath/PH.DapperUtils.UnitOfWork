using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// Dapper Base class
	///
	/// <para>Wrapper for Hide <see cref="IDbConnection"/> and <see cref="IDbTransaction"/> exposing only dapper method for data manipulation</para>
	///
	/// <para><see cref="DapperBase"/> provide internal access for <see cref="IDbConnection">connection</see> and related <see cref="IDbTransaction">transaction</see> to <see cref="DapperUnitOfWorkSqlMapper"/> </para>
	/// </summary>
	public abstract class DapperBase : IDisposable
	{
		private IDbConnection _conn;

		internal IDbConnection  DbConnection => GetConn();

		private IDbConnection GetConn()
		{
			CancellationToken.ThrowIfCancellationRequested();
			return _conn;
		}

		internal IDbTransaction DbTransaction;

		/// <summary>
		/// Gets or sets the cancellation token.
		/// </summary>
		/// <value>
		/// The cancellation token.
		/// </value>
		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="DapperBase"/> is disposed.
		/// </summary>
		/// <value>
		///   <c>true</c> if disposed; otherwise, <c>false</c>.
		/// </value>
		public bool              Disposed          { get; internal set; }

		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public string            Id                { get; internal set; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="DapperBase"/> is read-only.
		/// </summary>
		/// <value>
		///   <c>true</c> if read-only (<code>IsolationLevel.Chaos || IsolationLevel.ReadUncommitted ||  IsolationLevel.Unspecified</code>) ; otherwise, <c>false</c>.
		/// </value>
		public bool              ReadOnly          => IfReadOnly();

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="DapperBase"/> is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if active; otherwise, <c>false</c>.
		/// </value>
		public bool              Active            { get; protected internal set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="DapperBase"/> class.
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
		protected DapperBase(IDbConnection dbConnection, IDbTransaction dbTransaction, string id, CancellationToken cancellationToken = default(CancellationToken))
		{
			_conn         = dbConnection  ?? throw new ArgumentNullException(nameof(dbConnection));
			DbTransaction = dbTransaction ?? throw new ArgumentNullException(nameof(dbTransaction));
			Id            = id            ?? throw new ArgumentNullException(nameof(id));
			Disposed      = false;

			CancellationToken = cancellationToken;
			Active            = true;
		}

		internal bool IfReadOnly()
		{
			switch (DbTransaction.IsolationLevel)
			{
				case IsolationLevel.ReadCommitted:
				case IsolationLevel.RepeatableRead:
				case IsolationLevel.Serializable:
				case IsolationLevel.Snapshot:
					return false;


				case IsolationLevel.Chaos:
				case IsolationLevel.ReadUncommitted:
				case IsolationLevel.Unspecified:
					return true;
			}

			return true;
		}

		/// <summary>
		///     Throws if read only. (IsolationLevel.Chaos,IsolationLevel.ReadUncommitted,IsolationLevel.Unspecified)
		/// </summary>
		/// <exception cref="System.NotSupportedException">CRUD Operation not allowed: IsolationLevel</exception>
		public virtual void ThrowIfReadOnly()
		{
			if (ReadOnly)
			{
				throw new
					NotSupportedException($"CRUD Operation not allowed: IsolationLevel: '{DbTransaction.IsolationLevel}'");
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !Disposed)
			{
				Disposed = true;
				_conn?.Dispose();
				DbTransaction?.Dispose();
			}
		}

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}