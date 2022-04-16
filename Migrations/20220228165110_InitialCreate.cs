using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CrudApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PERSONS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NAME = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    AGE = table.Column<byte>(type: "smallint", nullable: true),
                    SEX = table.Column<bool>(type: "boolean", nullable: false),
                    HAIR_COLOR = table.Column<byte>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PERSONS");
        }
    }
}
