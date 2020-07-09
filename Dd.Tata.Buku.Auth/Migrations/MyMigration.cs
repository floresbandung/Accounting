using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dd.Tata.Buku.Auth.Migrations
{
    public abstract class MyMigration : Migration
    {
		public FluentMigrator.Builders.Create.Table.ICreateTableColumnOptionOrWithColumnSyntax CreateBaseTable(string tableName)
		{
			return Create.Table(tableName)
				.WithColumn("Id").AsGuid().PrimaryKey()
				.WithColumn("CreateBy").AsString().NotNullable().WithDefaultValue("System")
				.WithColumn("CreateDate").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
				.WithColumn("ModifiedBy").AsString().Nullable()
				.WithColumn("ModifiedDate").AsDateTime().Nullable();
		}
	}
}
