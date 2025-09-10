using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierDashboard.Migrations
{
    /// <inheritdoc />
    public partial class MarkupdiscountvoidServicestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DiscountFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markups",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MarkupFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarkupType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoidServices",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VoidFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VoidType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoidServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountAgencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DiscountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgencyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountAgencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountAgencies_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MarkupAgencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MarkupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgencyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkupAgencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkupAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MarkupAgencies_Markups_MarkupId",
                        column: x => x.MarkupId,
                        principalTable: "Markups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoidServiceAgencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VoidServiceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AgencyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoidServiceAgencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoidServiceAgencies_Agencies_AgencyId",
                        column: x => x.AgencyId,
                        principalTable: "Agencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoidServiceAgencies_VoidServices_VoidServiceId",
                        column: x => x.VoidServiceId,
                        principalTable: "VoidServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "13asd4134",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "2148ekfja",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(427));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "ak13jfei",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kafj938ka",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kjf98932",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjf;au332",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1062));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjioweiu",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1069));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "qiukdaj233",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1067));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akdjf832",
                column: "BookingDate",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1111));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akldja933",
                column: "BookingDate",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1119));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "kja;iou29",
                column: "BookingDate",
                value: new DateTime(2025, 9, 10, 21, 37, 2, 991, DateTimeKind.Utc).AddTicks(1116));

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAgencies_AgencyId",
                table: "DiscountAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountAgencies_DiscountId",
                table: "DiscountAgencies",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupAgencies_AgencyId",
                table: "MarkupAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MarkupAgencies_MarkupId",
                table: "MarkupAgencies",
                column: "MarkupId");

            migrationBuilder.CreateIndex(
                name: "IX_VoidServiceAgencies_AgencyId",
                table: "VoidServiceAgencies",
                column: "AgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_VoidServiceAgencies_VoidServiceId",
                table: "VoidServiceAgencies",
                column: "VoidServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountAgencies");

            migrationBuilder.DropTable(
                name: "MarkupAgencies");

            migrationBuilder.DropTable(
                name: "VoidServiceAgencies");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Markups");

            migrationBuilder.DropTable(
                name: "VoidServices");

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "13asd4134",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9049));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "2148ekfja",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9052));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "ak13jfei",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9028));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kafj938ka",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9055));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kjf98932",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9057));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjf;au332",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjioweiu",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "qiukdaj233",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9448));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akdjf832",
                column: "BookingDate",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9510));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akldja933",
                column: "BookingDate",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9521));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "kja;iou29",
                column: "BookingDate",
                value: new DateTime(2025, 9, 9, 21, 39, 21, 231, DateTimeKind.Utc).AddTicks(9518));
        }
    }
}
