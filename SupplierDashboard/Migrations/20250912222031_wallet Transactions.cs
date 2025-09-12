using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplierDashboard.Migrations
{
    /// <inheritdoc />
    public partial class walletTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WalletSetting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubAgencyId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionSubType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentNumbers = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletTransactions_SubAgencies_SubAgencyId",
                        column: x => x.SubAgencyId,
                        principalTable: "SubAgencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "13asd4134",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(8584));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "2148ekfja",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "ak13jfei",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(8567));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kafj938ka",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "Agencies",
                keyColumn: "Id",
                keyValue: "kjf98932",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(8593));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjf;au332",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9206));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "akdjioweiu",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9219));

            migrationBuilder.UpdateData(
                table: "Agents",
                keyColumn: "Id",
                keyValue: "qiukdaj233",
                column: "CreatedAt",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9215));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akdjf832",
                column: "BookingDate",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "akldja933",
                column: "BookingDate",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9343));

            migrationBuilder.UpdateData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: "kja;iou29",
                column: "BookingDate",
                value: new DateTime(2025, 9, 12, 22, 20, 26, 533, DateTimeKind.Utc).AddTicks(9341));

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_SubAgencyId",
                table: "WalletTransactions",
                column: "SubAgencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletSetting");

            migrationBuilder.DropTable(
                name: "WalletTransactions");

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
        }
    }
}
