using System;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.Dialect.Schema;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.Dialect
{
	/// <summary>
	/// An SQL dialect for DB2 on iSeries OS/400.
	/// </summary>
	/// <remarks>
	/// The DB2400Dialect defaults the following configuration properties:
	/// <list type="table">
	///		<listheader>
	///			<term>Property</term>
	///			<description>Default Value</description>
	///		</listheader>
	///		<item>
	///			<term>connection.driver_class</term>
	///			<description><see cref="NHibernate.Driver.DB2400Driver" /></description>
	///		</item>
	/// </list>
	/// </remarks>
	public class DB2400Dialect : DB2Dialect
	{
		public DB2400Dialect()
		{
			DefaultProperties[Cfg.Environment.ConnectionDriver] = "NHibernate.Driver.DB2400Driver";
		}

		public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
		{
			// The DB2 implementation is not valid for DB2400.
			throw new NotSupportedException();
		}

		public override bool SupportsSequences
		{
			get { return false; }
		}

		public override string IdentitySelectString
		{
			get { return "select identity_val_local() from sysibm.sysdummy1"; }
		}

		public override bool SupportsLimit
		{
			get { return true; }
		}

		public override bool SupportsLimitOffset
		{
			get { return false; }
		}

        public override SqlString GetLimitString(SqlString queryString, SqlString offset, SqlString limit)
		{
            return new SqlString(queryString, " fetch first ", limit, " rows only ");
		}

		public override bool UseMaxForLimit
		{
			get { return true; }
		}

		public override bool SupportsVariableLimit
		{
			get { return false; }
		}

		public override string ToStringLiteral(string value, SqlType type)
		{
			if (value == null)
				throw new System.ArgumentNullException(nameof(value));
			if (type == null)
				throw new System.ArgumentNullException(nameof(value));

			// See https://www.ibm.com/docs/en/db2/11.5?topic=elements-constants#r0000731__title__7
			var literal = new StringBuilder(value);
			
			literal
				.Replace("'", "''")
				.Insert(0, '\'')
				.Append('\'');

			return literal.ToString();
		}
	}
}
