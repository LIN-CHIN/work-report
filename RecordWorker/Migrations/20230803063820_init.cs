using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RecordWorker.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_report_record",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<string>(type: "text", nullable: false),
                    machine_number = table.Column<string>(type: "varchar(50)", nullable: false),
                    spend_time_hour = table.Column<int>(type: "integer", nullable: false),
                    spend_time_minute = table.Column<int>(type: "integer", nullable: false),
                    spend_time_second = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_report_record", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_work_report_record_event_id",
                table: "work_report_record",
                column: "event_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "work_report_record");
        }
    }
}
