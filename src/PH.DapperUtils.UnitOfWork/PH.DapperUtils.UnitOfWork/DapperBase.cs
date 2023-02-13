using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// Dapper Base class
	///
	/// <para>Wrapper for Hide <see cref="IDbConnection"/> and <see cref="IDbTransaction"/> exposing only dapper method for data manipulation</para>
	///
	/// <para><see cref="DapperBase"/> provide internal access for <see cref="IDbConnection">connection</see> and related <see cref="IDbTransaction">transaction</see> to <see cref="CrudSqlMapper"/> </para>
	/// </summary>
	public abstract class DapperBase : IDisposable
	{
		private IDbConnection _conn;

		internal IDbConnection  DbConnection => GetConn();

		private IDbConnection GetConn()
		{
			if (CancellationToken.IsCancellationRequested)
			{
				Dispose(true);
				CancellationToken.ThrowIfCancellationRequested();
			}

			return _conn;
		}

		internal IDbTransaction DbTransaction;

		/// <summary>
		/// Gets or sets the cancellation token.
		/// </summary>
		/// <value>
		/// The cancellation token.
		/// </value>
		internal CancellationToken CancellationToken { get; set; }

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
		/// Gets the cancellation token source.
		/// </summary>
		/// <value>
		/// The cancellation token source.
		/// </value>
		public CancellationTokenSource CancellationTokenSource { get; }

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

			CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new []{ cancellationToken });
			CancellationToken       = CancellationTokenSource.Token;

			Active        = true;
            DictionaryMap = new MapConfig();

        }

        public void SetMapConfig(MapConfig cfg)
        {
            DictionaryMap = cfg;
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
				Active   = false;
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



        #region Map / Fields configuration

        internal MapConfig DictionaryMap;


        internal TableConfig GetTableConfigConfig<T>(T entity) where T : class
        {
            var eType = entity.GetType();
            if (!DictionaryMap.Tables.ContainsKey(eType))
            {
                var props = eType.GetProperties();
                var l     = new List<FieldConfig>();
                foreach (var propertyInfo in props)
                {
                    var parameter   = Expression.Parameter(eType);
                    var property    = Expression.Property(parameter, propertyInfo);
                    var conversion  = Expression.Convert(property, typeof(object));
                    var fnc         = (Expression.Lambda<Func<T, object>>(conversion, parameter)).Compile();

                    bool isKey = false;
                    bool isAssigned = false;
                    var keyAttr = propertyInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>();
                    if (keyAttr != null)
                    {
                        isKey = true;
                    }

                    if (!isKey)
                    {
                        var k2 = propertyInfo.GetCustomAttribute<Dapper.Contrib.Extensions.KeyAttribute>();
                        if (k2 != null)
                        {
                            isKey = true;
                        }
                    }

                    if (isKey)
                    {
                        var dbGen = propertyInfo.GetCustomAttribute<DatabaseGeneratedAttribute>();
                        if (null != dbGen)
                        {
                            if (dbGen.DatabaseGeneratedOption == DatabaseGeneratedOption.None)
                            {
                                isAssigned = true;
                            }
                        }


                        var fieldConfig = new KeyConfig<T>(fnc, propertyInfo.Name,isAssigned);

                        l.Add(fieldConfig);

                    }
                    else
                    {
                        var fieldConfig = new FieldConfig<T>(fnc, propertyInfo.Name);

                        l.Add(fieldConfig);    
                    }


                    
                }

                var tbl = $"{eType.Name.ToLowerInvariant()}";
                if (!tbl.EndsWith("s", StringComparison.InvariantCultureIgnoreCase))
                {
                    tbl = $"{tbl}s";
                }

                DictionaryMap.Tables.Add(eType, new TableConfig() { TableName = tbl, Fields = l.ToArray() });
            }

            return DictionaryMap.Tables[eType];
        }

       


        #endregion
    }
}