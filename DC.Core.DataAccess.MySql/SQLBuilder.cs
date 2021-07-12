using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DC.Core.DataAccess.MySql.SQLStatements;

namespace DC.Core.DataAccess.MySql
{
	public class SQLBuilder
	{
		public const string LEFT_DELIMITER = "{";
		public const string RIGHT_DELIMITER = "}";
		public const string PARAMETER_PREFIX = ":";

		private const string INNER_QUERY = "##INNER_QUERY##";
		private const string ORDER_BY_CLAUSE = "ORDER BY";
		private const string ORDER_ASCENDING = "ASC";
		private const string ORDER_DESCENDING = "DESC";

		private readonly StringBuilder sbStatement;

		private readonly List<OrderByOption> OrderByColumns = new List<OrderByOption>();

		public SQLBuilder(string sqlStatement)
		{
			sbStatement = new StringBuilder(sqlStatement);
		}

		public void KeepParameters(IEnumerable<string> parameters)
		{
			KeepParameters(parameters.ToArray());
		}

		public void KeepParameters(params string[] parameters)
		{
			string anyParamPat = $@"\{LEFT_DELIMITER}([^\{RIGHT_DELIMITER}]+{PARAMETER_PREFIX}\w+[^\{RIGHT_DELIMITER}]*)\{RIGHT_DELIMITER}";
			Regex anyParameterRegex = new Regex(anyParamPat, RegexOptions.Singleline | RegexOptions.IgnoreCase);
			MatchCollection allParams = anyParameterRegex.Matches(sbStatement.ToString());

			foreach (Match match in allParams)
			{
				bool isProtected = false;
				if (parameters != null)
					foreach (string par in parameters.Where(p => p != null))
					{
						string pat = $@"\{LEFT_DELIMITER}([^\{RIGHT_DELIMITER}]+{PARAMETER_PREFIX}{par}+[^\{RIGHT_DELIMITER}]*)\{RIGHT_DELIMITER}";
						Regex r = new Regex(pat, RegexOptions.Singleline | RegexOptions.IgnoreCase);
						isProtected = r.IsMatch(match.Value);
						if (isProtected)
							break;
					}

				if (!isProtected)
				{
					string s2Remove = match.Value;
					sbStatement.Replace(s2Remove, string.Empty);
				}
			}
		}

		public void AddOrderBy(string column, bool descending)
		{
			OrderByColumns.Add(new OrderByOption(column, descending));
		}

		/// <summary>
		/// Returns a clean SQL statement from the statement builder
		/// Adds 2 new parameter that need to be filled. "PageNumber" and "RowspPage".
		/// PageNumber indicates the page of data that is to be returned (starts at 0 ),
		/// RowspPage indicates the maximum number of records to return.
		/// To show the first 100 records, use: PageNumber:0, RowspPage: 100
		/// To show the second 100 records, use: PageNumber:1, RowspPage: 200
		/// NOTE: It's mandatory the Order By clause in the query
		/// </summary>
		/// <returns>The statement with respective parameters</returns>
		public string ToPaginated()
		{
			string resultText = this.ToStatement();

			if (!OrderByColumns.Any())
				throw new ArgumentOutOfRangeException(nameof(OrderByColumns));

			resultText = Common.QueryPaginator.Replace(INNER_QUERY, resultText);

			return resultText;
		}

		public string ToStatement()
		{
			string resultText = sbStatement.ToString();

			resultText = resultText.Replace(LEFT_DELIMITER, string.Empty);
			resultText = resultText.Replace(RIGHT_DELIMITER, string.Empty);

			resultText = string.Concat(resultText, " ", GetOrderByClause());

			return resultText;
		}

		private string GetOrderByClause()
		{
			if (!OrderByColumns.Any())
				return string.Empty;

			string orderConditions = string.Join(", ", OrderByColumns.Select(o => o.ToString()));

			return $"{ORDER_BY_CLAUSE} {orderConditions}";
		}

		private class OrderByOption
		{
			public OrderByOption(string columnName, bool descending)
			{
				ColumnName = columnName ?? throw new ArgumentNullException(nameof(columnName));
				Descending = descending;
			}

			public string ColumnName { get; set; }
			public bool Descending { get; set; }

			public override string ToString()
			{
				string order = Descending ? ORDER_DESCENDING : ORDER_ASCENDING;
				return $"{ColumnName} {order}";
			}
		}
	}
}
