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
	public static partial class CrudSqlMapper
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
		public static int Execute(this Crud uow, string sql, object param = null, int? commandTimeout = null,
		                          CommandType? commandType = null)
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.Execute(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);
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
		public static object ExecuteScalar(this Crud uow, string sql, object param = null,
		                                   int? commandTimeout = null,
		                                   CommandType? commandType = null)
			=> uow.Uow.DbConnection.ExecuteScalar(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static T ExecuteScalar<T>(this Crud uow, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null)
			=> uow.Uow.DbConnection.ExecuteScalar<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static IEnumerable<dynamic> Query(this Crud uow, string sql, object param = null,
		                                         bool buffered = true,
		                                         int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.Query(sql, param, uow.Uow.DbTransaction, buffered, commandTimeout, commandType);


		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QueryFirst(this Crud uow, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryFirst(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QueryFirstOrDefault(this Crud uow, string sql, object param = null,
		                                          int? commandTimeout = null,
		                                          CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryFirstOrDefault(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QuerySingle(this Crud uow, string sql, object param = null,
		                                  int? commandTimeout = null,
		                                  CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingle(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Return a dynamic object with properties matching the columns.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		/// <remarks>Note: the row can be accessed via "dynamic", or by casting to an IDictionary&lt;string,object&gt;</remarks>
		public static dynamic QuerySingleOrDefault(this Crud uow, string sql, object param = null,
		                                           int? commandTimeout = null,
		                                           CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingleOrDefault(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static IEnumerable<T> Query<T>(this Crud uow, string sql, object param = null,
		                                      bool buffered = true,
		                                      int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query<T>(sql, param, uow.Uow.DbTransaction, buffered, commandTimeout, commandType);


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
		public static T QueryFirst<T>(this Crud uow, string sql, object param = null, int? commandTimeout = null,
		                              CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirst<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static T QueryFirstOrDefault<T>(this Crud uow, string sql, object param = null,
		                                       int? commandTimeout = null,
		                                       CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstOrDefault<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static T QuerySingle<T>(this Crud uow, string sql, object param = null, int? commandTimeout = null,
		                               CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingle<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static T QuerySingleOrDefault<T>(this Crud uow, string sql, object param = null,
		                                        int? commandTimeout = null,
		                                        CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingleOrDefault<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static IEnumerable<object> Query(this Crud uow, Type type, string sql, object param = null,
		                                        bool buffered = true,
		                                        int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(type, sql, param, uow.Uow.DbTransaction, buffered, commandTimeout, commandType);


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
		public static object QueryFirst(this Crud uow, Type type, string sql, object param = null,
		                                int? commandTimeout = null,
		                                CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirst(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static object QueryFirstOrDefault(this Crud uow, Type type, string sql, object param = null,
		                                         int? commandTimeout = null,
		                                         CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstOrDefault(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static object QuerySingle(this Crud uow, Type type, string sql, object param = null,
		                                 int? commandTimeout = null,
		                                 CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingle(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static object QuerySingleOrDefault(this Crud uow, Type type, string sql, object param = null,
		                                          int? commandTimeout = null,
		                                          CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingleOrDefault(type, sql, param, uow.Uow.DbTransaction, commandTimeout,
			                                         commandType);


		/// <summary>
		///     Execute a command that returns multiple result sets, and access each in turn.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for this query.</param>
		/// <param name="param">The parameters to use for this query.</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout.</param>
		/// <param name="commandType">Is it a stored proc or a batch?</param>
		public static SqlMapper.GridReader QueryMultiple(this Crud uow, string sql, object param = null,
		                                                 int? commandTimeout = null,
		                                                 CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryMultiple(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
			this Crud uow, string sql, Func<TFirst, TSecond, TReturn> map,
			object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null,
			CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param, uow.Uow.DbTransaction, buffered, splitOn,
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
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param, uow.Uow.DbTransaction, buffered, splitOn,
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
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param, uow.Uow.DbTransaction, buffered,
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
		public static IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param, uow.Uow.DbTransaction,
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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param,
			                          uow.Uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
			object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
			CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, map, param,
			                          uow.Uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


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
		public static IEnumerable<TReturn> Query<TReturn>(this Crud uow, string sql, Type[] types,
		                                                  Func<object[], TReturn> map,
		                                                  object param = null, bool buffered = true,
		                                                  string splitOn = "Id",
		                                                  int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.Query(sql, types, map, param, uow.Uow.DbTransaction, buffered, splitOn, commandTimeout,
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
		public static T Get<T>(this Crud uow, dynamic id, int? commandTimeout = null) where T : class =>
			SqlMapperExtensions.Get<T>(uow.Uow.DbConnection, id, uow.Uow.DbTransaction,
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
		public static IEnumerable<T> GetAll<T>(this Crud uow, int? commandTimeout = null) where T : class =>
			uow.Uow.DbConnection.GetAll<T>(uow.Uow.DbTransaction,
			                           commandTimeout);


		/// <summary>
		///     Inserts an entity into table "Ts" and returns identity id or number of inserted rows if inserting a list.
		/// </summary>
		/// <typeparam name="T">The type to insert.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToInsert">Entity to insert, can be list of entities</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>Identity of inserted entity, or number of inserted rows if inserting a list</returns>
		public static long Insert<T>(this Crud uow, T entityToInsert, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.Insert(entityToInsert, uow.Uow.DbTransaction,
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
		public static bool Update<T>(this Crud uow, T entityToUpdate, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.Update(entityToUpdate,
			                               uow.Uow.DbTransaction, commandTimeout);
		}

		/// <summary>
		///     Delete entity in table "Ts".
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToDelete">Entity to delete</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if not found</returns>
		public static bool Delete<T>(this Crud uow, T entityToDelete, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.Delete(entityToDelete, uow.Uow.DbTransaction,
			                               commandTimeout);
		}

		/// <summary>
		///     Delete all entities in the table related to the type T.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if none found</returns>
		public static bool DeleteAll<T>(this Crud uow, int? commandTimeout = null) where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.DeleteAll<T>(uow.Uow.DbTransaction,
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
		public static Task<IEnumerable<dynamic>> QueryAsync(this Crud uow, string sql, object param = null,
		                                                    int? commandTimeout = null,
		                                                    CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static Task<IEnumerable<T>> QueryAsync<T>(this Crud uow, string sql, object param = null,
		                                                 int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type of result to return.</typeparam>
		/// <param name="uow"></param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QueryFirstAsync<T>(this Crud uow, string sql, object param = null,
		                                         int? commandTimeout = null,
		                                         CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryFirstAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static Task<T> QueryFirstOrDefaultAsync<T>(this Crud uow, string sql, object param = null,
		                                                  int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstOrDefaultAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout,
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
		public static Task<T> QuerySingleAsync<T>(this Crud uow, string sql, object param = null,
		                                          int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingleAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<T> QuerySingleOrDefaultAsync<T>(this Crud uow, string sql, object param = null,
		                                                   int? commandTimeout = null,
		                                                   CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingleOrDefaultAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QueryFirstAsync(this Crud uow, string sql, object param = null,
		                                            int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QueryFirstOrDefaultAsync(this Crud uow, string sql, object param = null,
		                                                     int? commandTimeout = null,
		                                                     CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstOrDefaultAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QuerySingleAsync(this Crud uow, string sql, object param = null,
		                                             int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingleAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


		/// <summary>
		///     Execute a single-row query asynchronously using Task.
		/// </summary>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="sql">The SQL to execute for the query.</param>
		/// <param name="param">The parameters to pass, if any.</param>
		/// <param name="commandTimeout">The command timeout (in seconds).</param>
		/// <param name="commandType">The type of command to execute.</param>
		public static Task<dynamic> QuerySingleOrDefaultAsync(this Crud uow, string sql, object param = null,
		                                                      int? commandTimeout = null,
		                                                      CommandType? commandType = null)
			=> uow.Uow.DbConnection.QuerySingleOrDefaultAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static Task<IEnumerable<object>> QueryAsync(this Crud uow, Type type, string sql,
		                                                   object param = null, int? commandTimeout = null,
		                                                   CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryAsync(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static Task<object> QueryFirstAsync(this Crud uow, Type type, string sql, object param = null,
		                                           int? commandTimeout = null, CommandType? commandType = null)
			=> uow.Uow.DbConnection.QueryFirstAsync(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static Task<object> QueryFirstOrDefaultAsync(this Crud uow, Type type, string sql,
		                                                    object param = null, int? commandTimeout = null,
		                                                    CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryFirstOrDefaultAsync(type, sql, param, uow.Uow.DbTransaction, commandTimeout,
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
		public static Task<object> QuerySingleAsync(this Crud uow, Type type, string sql, object param = null,
		                                            int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingleAsync(type, sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);

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
		public static Task<object> QuerySingleOrDefaultAsync(this Crud uow, Type type, string sql,
		                                                     object param = null, int? commandTimeout = null,
		                                                     CommandType? commandType = null) =>
			uow.Uow.DbConnection.QuerySingleOrDefaultAsync(type, sql, param, uow.Uow.DbTransaction, commandTimeout,
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
		public static Task<int> ExecuteAsync(this Crud uow, string sql, object param = null,
		                                     int? commandTimeout = null,
		                                     CommandType? commandType = null)
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.ExecuteAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);
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
		public static Task<IEnumerable<TReturn>> QueryAsync<TFirst, TSecond, TReturn>(this Crud uow,
		                                                                              string sql, Func<TFirst, TSecond, TReturn> map, object param = null, bool buffered = true,
		                                                                              string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, map, param, uow.Uow.DbTransaction, buffered,
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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, map, param, uow.Uow.DbTransaction,
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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param = null, bool buffered = true,
			string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, map, param, uow.Uow.DbTransaction,
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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, object param = null,
			bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, map, param,
			                            uow.Uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


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
			this Crud uow,
			string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TReturn> map,
			object param = null, bool buffered = true, string splitOn = "Id",
			int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, map, param,
			                            uow.Uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


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
			QueryAsync<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn>(this Crud uow,
				string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TSeventh, TReturn> map,
				object param = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null,
				CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql,
			                            map,
			                            param,
			                            uow.Uow.DbTransaction, buffered, splitOn, commandTimeout, commandType);


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
		public static Task<IEnumerable<TReturn>> QueryAsync<TReturn>(this Crud uow, string sql, Type[] types,
		                                                             Func<object[], TReturn> map,
		                                                             object param = null, bool buffered = true,
		                                                             string splitOn = "Id", int? commandTimeout = null,
		                                                             CommandType? commandType = null) =>
			uow.Uow.DbConnection.QueryAsync(sql, types, map, param, uow.Uow.DbTransaction, buffered, splitOn,
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
		public static Task<object> ExecuteScalarAsync(this Crud uow, string sql, object param = null,
		                                              int? commandTimeout = null, CommandType? commandType = null) =>
			uow.Uow.DbConnection.ExecuteScalarAsync(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static Task<T> ExecuteScalarAsync<T>(this Crud uow, string sql, object param = null,
		                                            int? commandTimeout = null,
		                                            CommandType? commandType = null) =>
			uow.Uow.DbConnection.ExecuteScalarAsync<T>(sql, param, uow.Uow.DbTransaction, commandTimeout, commandType);


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
		public static Task<T> GetAsync<T>(this Crud uow, dynamic id, int? commandTimeout = null)
			where T : class =>
			SqlMapperExtensions.GetAsync<T>(uow.Uow.DbConnection, id, uow.Uow.DbTransaction,
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
		public static async Task<IEnumerable<T>> GetAllAsync<T>(this Crud uow, int? commandTimeout = null)
			where T : class => await uow.Uow.DbConnection.GetAllAsync<T>(uow.Uow.DbTransaction, commandTimeout);


		/// <summary>
		///     Inserts an entity into table "Ts" asynchronously using Task and returns inserted.
		/// </summary>
		/// <typeparam name="T">The type being inserted.</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToInsert">Entity to insert</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <returns>inserted entity</returns>
		public static Task<T> InsertAsync<T>(this Crud uow, T entityToInsert, int? commandTimeout = null)
			where T : class
		{
			uow.ThrowIfReadOnly();
            return uow.InsertEntityAsync(entityToInsert, commandTimeout);
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
		public static Task<bool> UpdateAsync<T>(this Crud uow, T entityToUpdate, int? commandTimeout = null)
			where T : class
		{
			uow.ThrowIfReadOnly();
			return uow.Uow.DbConnection.UpdateAsync(entityToUpdate,
			                                    uow.Uow.DbTransaction, commandTimeout);
		}

		/// <summary>
		///     Delete entity in table "Ts" asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="entityToDelete">Entity to delete</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if not found</returns>
		public static Task<bool> DeleteAsync<T>(this Crud uow, T entityToDelete, int? commandTimeout = null)
			where T : class =>
			uow.Uow.DbConnection.DeleteAsync(entityToDelete,
			                             uow.Uow.DbTransaction, commandTimeout);

		/// <summary>
		///     Delete all entities in the table related to the type T asynchronously using Task.
		/// </summary>
		/// <typeparam name="T">Type of entity</typeparam>
		/// <param name="uow">Unit Of Work</param>
		/// <param name="commandTimeout">Number of seconds before command execution timeout</param>
		/// <returns>true if deleted, false if none found</returns>
		public static Task<bool> DeleteAllAsync<T>(this Crud uow, int? commandTimeout = null) where T : class =>
			uow.Uow.DbConnection.DeleteAllAsync<T>(uow.Uow.DbTransaction,
			                                   commandTimeout);

		#endregion
	}
}