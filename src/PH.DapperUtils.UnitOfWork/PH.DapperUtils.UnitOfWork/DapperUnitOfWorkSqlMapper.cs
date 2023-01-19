#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

#endregion

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// A Subset of extensions methods from <see cref="Dapper.SqlMapper"/> overriding use of <see cref="IDbConnection"/> and <see cref="IDbTransaction"/>
	///
	/// 
	/// </summary>
	public static partial class DapperUnitOfWorkSqlMapper
	{
		/// <summary>
		///     Execute parameterized SQL.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The number of rows affected.</returns>
		public static int Execute(this DapperBase uow, string sql, object param = null, int? commandTimeout = null,
		                          CommandType? commandType = null)
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.Execute(sql, param, uow.DbTransaction, commandTimeout, commandType);
		}


		/// <summary>
		///     Execute parameterized SQL that selects a single value.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute.</param>
		/// <param name="param">The parameters to use for this command.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The first cell selected as <see cref="object" />.</returns>
		public static object ExecuteScalar(this DapperBase uow, string sql, object param = null,
		                                   int? commandTimeout = null,
		                                   CommandType? commandType = null)
			=> uow.DbConnection.ExecuteScalar(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute parameterized SQL that selects a single value.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute.</param>
		/// <param name="param">The parameters to use for this command.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The first cell returned, as <typeparamref name="T" />.</returns>
		public static T ExecuteScalar<T>(this DapperBase uow, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null)
			=> uow.DbConnection.ExecuteScalar<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Return a sequence of dynamic objects with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static IEnumerable<dynamic> Query(this DapperBase uow, string sql, object param = null,
		                                         bool buffered = true,
		                                         int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.Query(sql, param, uow.DbTransaction, buffered, commandTimeout, commandType);


		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QueryFirst(this DapperBase uow, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null) =>
			uow.DbConnection.QueryFirst(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QueryFirstOrDefault(this DapperBase uow, string sql, object param = null,
		                                          int? commandTimeout = null,
		                                          CommandType? commandType = null) =>
			uow.DbConnection.QueryFirstOrDefault(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QuerySingle(this DapperBase uow, string sql, object param = null,
		                                  int? commandTimeout = null,
		                                  CommandType? commandType = null) =>
			uow.DbConnection.QuerySingle(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QuerySingleOrDefault(this DapperBase uow, string sql, object param = null,
		                                           int? commandTimeout = null,
		                                           CommandType? commandType = null) =>
			uow.DbConnection.QuerySingleOrDefault(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Executes a query, returning the data typed as <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of results to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="buffered">Whether to buffer results in memory.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static IEnumerable<T> Query<T>(this DapperBase uow, string sql, object param = null,
		                                      bool buffered = true,
		                                      int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query<T>(sql, param, uow.DbTransaction, buffered, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static T QueryFirst<T>(this DapperBase uow, string sql, object param = null, int? commandTimeout = null,
		                              CommandType? commandType = null)
			=> uow.DbConnection.QueryFirst<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static T QueryFirstOrDefault<T>(this DapperBase uow, string sql, object param = null,
		                                       int? commandTimeout = null,
		                                       CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstOrDefault<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static T QuerySingle<T>(this DapperBase uow, string sql, object param = null, int? commandTimeout = null,
		                               CommandType? commandType = null)
			=> uow.DbConnection.QuerySingle<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <typeparamref name="T" />.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static T QuerySingleOrDefault<T>(this DapperBase uow, string sql, object param = null,
		                                        int? commandTimeout = null,
		                                        CommandType? commandType = null)
			=> uow.DbConnection.QuerySingleOrDefault<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Executes a single-row query, returning the data typed as <paramref name="type" />.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="buffered">Whether to buffer results in memory.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static IEnumerable<object> Query(this DapperBase uow, Type type, string sql, object param = null,
		                                        bool buffered = true,
		                                        int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(type, sql, param, uow.DbTransaction, buffered, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <paramref name="type" />.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static object QueryFirst(this DapperBase uow, Type type, string sql, object param = null,
		                                int? commandTimeout = null,
		                                CommandType? commandType = null)
			=> uow.DbConnection.QueryFirst(type, sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <paramref name="type" />.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static object QueryFirstOrDefault(this DapperBase uow, Type type, string sql, object param = null,
		                                         int? commandTimeout = null,
		                                         CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstOrDefault(type, sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Executes a single-row query, returning the data typed as <paramref name="type" />.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static object QuerySingle(this DapperBase uow, Type type, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null)
			=> uow.DbConnection.QuerySingle(type, sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Executes a single-row query, returning the data typed as <paramref name="type" />.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		/// <returns>
		///     A sequence of data of the supplied type; if a basic type (int, string, etc) is queried then the data from the first
		///     column is assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static object QuerySingleOrDefault(this DapperBase uow, Type type, string sql, object param = null,
		                                          int? commandTimeout = null,
		                                          CommandType? commandType = null)
			=> uow.DbConnection.QuerySingleOrDefault(type, sql, param, uow.DbTransaction, commandTimeout,
			                                         commandType);


		/// <summary>
		///     Execute a command that returns multiple result sets, and access each in turn.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		public static SqlMapper.GridReader QueryMultiple(this DapperBase uow, string sql, object param = null,
		                                                 int? commandTimeout = null,
		                                                 CommandType? commandType = null) =>
			uow.DbConnection.QueryMultiple(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with 2 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(
			this DapperBase uow, string sql, Func<TFirst, TSecond, TReturn> map,
			object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null,
			CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param, uow.DbTransaction, buffered, splitOn,
			                          commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with 3 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param, uow.DbTransaction, buffered, splitOn,
			                          commandTimeout, commandType);

		/// <summary>
		///     Perform a multi-mapping query with 4 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param, uow.DbTransaction, buffered,
			                          splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with 5 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param, uow.DbTransaction,
			                          buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with 6 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param,
			                          uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with 7 input types. If you need more types -> use Query with Type[] parameter.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
		/// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
			object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
			CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, map, param,
			                          uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform a multi-mapping query with an arbitrary number of input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="types">Array of types in the recordset.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static IEnumerable<TReturn> Query<TReturn>(this DapperBase uow, string sql, Type[] types,
		                                                  Func<object[], TReturn> map,
		                                                  object param = null, bool buffered = true,
		                                                  string splitOn = "Id",
		                                                  int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.Query(sql, types, map, param, uow.DbTransaction, buffered, splitOn, commandTimeout,
			                          commandType);


		/// <summary>
		///     Returns a single entity by a single id from table "Ts".
		///     Id must be marked with [Key] attribute.
		///     Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
		///     for optimal performance.
		/// </summary>
		/// <typeparam name="T">Interface or type to create and populate</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Entity of T</returns>
		public static T Get<T>(this DapperBase uow, dynamic id, int? commandTimeout = null) where T : class =>
			SqlMapperExtensions.Get<T>(uow.DbConnection, id, uow.DbTransaction,
			                           commandTimeout);

		/// <summary>
		///     Returns a list of entities from table "Ts".
		///     Id of T must be marked with [Key] attribute.
		///     Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
		///     for optimal performance.
		/// </summary>
		/// <typeparam name="T">Interface or type to create and populate</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Entity of T</returns>
		public static IEnumerable<T> GetAll<T>(this DapperBase uow, int? commandTimeout = null) where T : class =>
			uow.DbConnection.GetAll<T>(uow.DbTransaction,
			                           commandTimeout);


		/// <summary>
		///     Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
		/// </summary>
		/// <typeparam name="T">The type to insert.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToInsert">Entity to insert, can be list of entities</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Identity of inserted entity, or number of inserted rows if inserting a list</returns>
		public static long Insert<T>(this DapperBase uow, T entityToInsert, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.Insert(entityToInsert, uow.DbTransaction,
			                               commandTimeout);
		}

		/// <summary>
		///     Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
		/// </summary>
		/// <typeparam name="T">Type to be updated</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToUpdate">Entity to be updated</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
		public static bool Update<T>(this DapperBase uow, T entityToUpdate, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.Update(entityToUpdate,
			                               uow.DbTransaction, commandTimeout);
		}

		/// <summary>
		///     Delete entity in table "Ts".
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToDelete">Entity to delete</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if not found</returns>
		public static bool Delete<T>(this DapperBase uow, T entityToDelete, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.Delete(entityToDelete, uow.DbTransaction,
			                               commandTimeout);
		}

		/// <summary>
		///     Delete all entities in the table related to the type T.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if none found</returns>
		public static bool DeleteAll<T>(this DapperBase uow, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.DeleteAll<T>(uow.DbTransaction,
			                                     commandTimeout);
		}
	

		#region Async

		/// <summary>
		///     Execute a query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: each row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static Task<IEnumerable<dynamic>> QueryAsync(this DapperBase uow, string sql, object param = null,
		                                                    int? commandTimeout = null,
		                                                    CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type of results to return.</typeparam>
		/// <param name="uow"></param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <returns>
		///     A sequence of data of <typeparamref name="T" />; if a basic type (int, string, etc) is queried then the data from
		///     the first column in assumed, otherwise an instance is
		///     created per row, and a direct column-name===member-name mapping is assumed (case insensitive).
		/// </returns>
		public static Task<IEnumerable<T>> QueryAsync<T>(this DapperBase uow, string sql, object param = null,
		                                                 int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow"></param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QueryFirstAsync<T>(this DapperBase uow, string sql, object param = null,
		                                         int? commandTimeout = null,
		                                         CommandType? commandType = null) =>
			uow.DbConnection.QueryFirstAsync<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// /
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QueryFirstOrDefaultAsync<T>(this DapperBase uow, string sql, object param = null,
		                                                  int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstOrDefaultAsync<T>(sql, param, uow.DbTransaction, commandTimeout,
			                                                commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QuerySingleAsync<T>(this DapperBase uow, string sql, object param = null,
		                                          int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QuerySingleAsync<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QuerySingleOrDefaultAsync<T>(this DapperBase uow, string sql, object param = null,
		                                                   int? commandTimeout = null,
		                                                   CommandType? commandType = null) =>
			uow.DbConnection.QuerySingleOrDefaultAsync<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QueryFirstAsync(this DapperBase uow, string sql, object param = null,
		                                            int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QueryFirstOrDefaultAsync(this DapperBase uow, string sql, object param = null,
		                                                     int? commandTimeout = null,
		                                                     CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstOrDefaultAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QuerySingleAsync(this DapperBase uow, string sql, object param = null,
		                                             int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.QuerySingleAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QuerySingleOrDefaultAsync(this DapperBase uow, string sql, object param = null,
		                                                      int? commandTimeout = null,
		                                                      CommandType? commandType = null)
			=> uow.DbConnection.QuerySingleOrDefaultAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		public static Task<IEnumerable<object>> QueryAsync(this DapperBase uow, Type type, string sql,
		                                                   object param = null, int? commandTimeout = null,
		                                                   CommandType? commandType = null)
			=> uow.DbConnection.QueryAsync(type, sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		public static Task<object> QueryFirstAsync(this DapperBase uow, Type type, string sql, object param = null,
		                                           int? commandTimeout = null, CommandType? commandType = null)
			=> uow.DbConnection.QueryFirstAsync(type, sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		public static Task<object> QueryFirstOrDefaultAsync(this DapperBase uow, Type type, string sql,
		                                                    object param = null, int? commandTimeout = null,
		                                                    CommandType? commandType = null) =>
			uow.DbConnection.QueryFirstOrDefaultAsync(type, sql, param, uow.DbTransaction, commandTimeout,
			                                          commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		public static Task<object> QuerySingleAsync(this DapperBase uow, Type type, string sql, object param = null,
		                                            int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QuerySingleAsync(type, sql, param, uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="type">The type to return.</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type" /> is <c>null</c>.</exception>
		public static Task<object> QuerySingleOrDefaultAsync(this DapperBase uow, Type type, string sql,
		                                                     object param = null, int? commandTimeout = null,
		                                                     CommandType? commandType = null) =>
			uow.DbConnection.QuerySingleOrDefaultAsync(type, sql, param, uow.DbTransaction, commandTimeout,
			                                           commandType);


		/// <summary>
		///     Execute a command asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The number of rows affected.</returns>
		public static Task<int> ExecuteAsync(this DapperBase uow, string sql, object param = null,
		                                     int? commandTimeout = null,
		                                     CommandType? commandType = null)
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.ExecuteAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);
		}


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 2 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this DapperBase uow,
		                                                                              string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true,
		                                                                              string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, map, param, uow.DbTransaction, buffered,
			                            splitOn,
			                            commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 3 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, map, param, uow.DbTransaction,
			                            buffered,
			                            splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 4 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, map, param, uow.DbTransaction,
			                            buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 5 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, map, param,
			                            uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 6 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn>(
			this DapperBase uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
			object param = null, bool buffered = true, string splitOn = "Id",
			int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, map, param,
			                            uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with 7 input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TFirst">The first type in the recordset.</typeparam>
		/// <typeparam name="TSecond">The second type in the recordset.</typeparam>
		/// <typeparam name="TThird">The third type in the recordset.</typeparam>
		/// <typeparam name="TFourth">The fourth type in the recordset.</typeparam>
		/// <typeparam name="TFifth">The fifth type in the recordset.</typeparam>
		/// <typeparam name="TSixth">The sixth type in the recordset.</typeparam>
		/// <typeparam name="TSeventh">The seventh type in the recordset.</typeparam>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>>
			QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this DapperBase uow,
				string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
				object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
				CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql,
			                            map,
			                            param,
			                            uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


		/// <summary>
		///     Perform an asynchronous multi-mapping query with an arbitrary number of input types.
		///     This returns a single type, combined from the raw types via <paramref name="map" />.
		/// </summary>
		/// <typeparam name="TReturn">The combined type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="types">Array of types in the recordset.</param>
		/// <param name="map">The function to map row types to the return type.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="buffered">Whether to buffer the results in memory.</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id").</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>An enumerable of <typeparamref name="TReturn" />.</returns>
		public static Task<IEnumerable<TReturn>> QueryAsync<TReturn>(this DapperBase uow, string sql, Type[] types,
		                                                             Func<object[], TReturn> map,
		                                                             object param = null, bool buffered = true,
		                                                             string splitOn = "Id", int? commandTimeout = null,
		                                                             CommandType? commandType = null) =>
			uow.DbConnection.QueryAsync(sql, types, map, param, uow.DbTransaction, buffered, splitOn,
			                            commandTimeout, commandType);


		/// <summary>
		///     Execute parameterized SQL that selects a single value.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute.</param>
		/// <param name="param">The parameters to use for this command.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The first cell returned, as <see cref="object" />.</returns>
		public static Task<object> ExecuteScalarAsync(this DapperBase uow, string sql, object param = null,
		                                              int? commandTimeout = null, CommandType? commandType = null) =>
			uow.DbConnection.ExecuteScalarAsync(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute parameterized SQL that selects a single value.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute.</param>
		/// <param name="param">The parameters to use for this command.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		/// <returns>The first cell returned, as <typeparamref name="T" />.</returns>
		public static Task<T> ExecuteScalarAsync<T>(this DapperBase uow, string sql, object param = null,
		                                            int? commandTimeout = null,
		                                            CommandType? commandType = null) =>
			uow.DbConnection.ExecuteScalarAsync<T>(sql, param, uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Returns a single entity by a single id from table "Ts" asynchronously using Task. T must be of interface type.
		///     Id must be marked with [Key] attribute.
		///     Created entity is tracked/intercepted for changes and used by the Update() extension.
		/// </summary>
		/// <typeparam name="T">Interface type to create and populate</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="id">Id of the entity to get, must be marked with [Key] attribute</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Entity of T</returns>
		public static Task<T> GetAsync<T>(this DapperBase uow, dynamic id, int? commandTimeout = null)
			where T : class =>
			SqlMapperExtensions.GetAsync<T>(uow.DbConnection, id, uow.DbTransaction,
			                                commandTimeout);

		/// <summary>
		///     Returns a list of entities from table "Ts".
		///     Id of T must be marked with [Key] attribute.
		///     Entities created from interfaces are tracked/intercepted for changes and used by the Update() extension
		///     for optimal performance.
		/// </summary>
		/// <typeparam name="T">Interface or type to create and populate</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Entity of T</returns>
		public static async Task<IEnumerable<T>> GetAllAsync<T>(this DapperBase uow, int? commandTimeout = null)
			where T : class => await uow.DbConnection.GetAllAsync<T>(uow.DbTransaction, commandTimeout);


		/// <summary>
		///     Inserts an entity into table "Ts" asynchronously using Task and returns identity id.
		/// </summary>
		/// <typeparam name="T">The type being inserted.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToInsert">Entity to insert</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <param name="sqlAdapter">The specific ISqlAdapter to use, auto-detected based on connection if null</param>
		/// <returns>Identity of inserted entity</returns>
		public static Task<int> InsertAsync<T>(this DapperBase uow, T entityToInsert, int? commandTimeout = null,
		                                       ISqlAdapter sqlAdapter = null)
			where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.InsertAsync(entityToInsert,
			                                    uow.DbTransaction, commandTimeout, sqlAdapter);
		}

		/// <summary>
		///     Updates entity in table "Ts" asynchronously using Task, checks if the entity is modified if the entity is tracked
		///     by the Get() extension.
		/// </summary>
		/// <typeparam name="T">Type to be updated</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToUpdate">Entity to be updated</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
		public static Task<bool> UpdateAsync<T>(this DapperBase uow, T entityToUpdate, int? commandTimeout = null)
			where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.DbConnection.UpdateAsync(entityToUpdate,
			                                    uow.DbTransaction, commandTimeout);
		}

		/// <summary>
		///     Delete entity in table "Ts" asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToDelete">Entity to delete</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if not found</returns>
		public static Task<bool> DeleteAsync<T>(this DapperBase uow, T entityToDelete, int? commandTimeout = null)
			where T : class =>
			uow.DbConnection.DeleteAsync(entityToDelete,
			                             uow.DbTransaction, commandTimeout);

		/// <summary>
		///     Delete all entities in the table related to the type T asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if none found</returns>
		public static Task<bool> DeleteAllAsync<T>(this DapperBase uow, int? commandTimeout = null) where T : class =>
			uow.DbConnection.DeleteAllAsync<T>(uow.DbTransaction,
			                                   commandTimeout);

		#endregion
	}
}