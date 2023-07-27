using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PK.Result
{
	public class ResultAggregateException : AggregateException
	{

		/// <inheritdoc />
		public ResultAggregateException(IEnumerable<Exception> innerExceptions) : base(innerExceptions)
		{
		}

		/// <inheritdoc />
		public ResultAggregateException(params Exception[] innerExceptions) : base(innerExceptions)
		{
		}

		/// <inheritdoc />
		protected ResultAggregateException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <inheritdoc />
		public ResultAggregateException(string message) : base(message)
		{
		}

		/// <inheritdoc />
		public ResultAggregateException(string message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions)
		{
		}

		/// <inheritdoc />
		public ResultAggregateException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <inheritdoc />
		public ResultAggregateException(string message, params Exception[] innerExceptions) : base(message, innerExceptions)
		{
		}
	}
}
