using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PH.DapperUtils.UnitOfWork
{
	/// <summary>
	/// Dapper Unit Of Work
	/// </summary>
	/// <seealso cref="IUnitOfWork" />
	public interface IDapperUnitOfWork : IUnitOfWork
	{
		CancellationTokenSource CancellationTokenSource { get; }
	}
}