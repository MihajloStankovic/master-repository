using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStore.Migrations
{
    public partial class AddRoles : Migration
    {
        private string AdministratorRoleId = 1.ToString();
        private string EmployeeRoleId = 2.ToString();
        private string UserRoleId = 3.ToString();

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            AddRolesSQL(migrationBuilder);
        }

        public void AddRolesSQL(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@$"insert into [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) " +
               @$"values ('{AdministratorRoleId}', 'Administrator', 'A', '1a')");

            migrationBuilder.Sql(@$"insert into [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) " +
               @$"values ('{EmployeeRoleId}', 'Employee', 'E', '2e')");

            migrationBuilder.Sql(@$"insert into [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) " +
               @$"values ('{UserRoleId}', 'User', 'U', '3u')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
