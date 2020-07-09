using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dd.Tata.Buku.Auth.Migrations
{
    [Migration(07082020)]
    public class InitMigration : MyMigration
    {
		public override void Up()
		{
			CreateBaseTable("UserProfile")
				.WithColumn("FirstName").AsString()
				.WithColumn("LastName").AsString();

			Insert.IntoTable("UserProfile")
				.Row(new
				{
					Id = Guid.NewGuid(),
					FirstName = "Maulana",
					LastName = "."
				});
		}

		public override void Down()
		{

		}
	}
}
