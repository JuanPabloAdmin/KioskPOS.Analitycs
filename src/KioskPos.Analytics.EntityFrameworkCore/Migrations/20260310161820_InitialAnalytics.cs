using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KioskPos.Analytics.Migrations
{
    /// <inheritdoc />
    public partial class InitialAnalytics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppCachedInvoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosConnectionConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    GlobalId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BusinessDay = table.Column<DateOnly>(type: "date", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VatIncluded = table.Column<bool>(type: "bit", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExternalPosId = table.Column<int>(type: "int", nullable: true),
                    ExternalUserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalCustomerId = table.Column<int>(type: "int", nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LineCount = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCachedInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCachedSalesOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosConnectionConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    GlobalId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    BusinessDay = table.Column<DateOnly>(type: "date", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderType = table.Column<int>(type: "int", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExternalPosId = table.Column<int>(type: "int", nullable: true),
                    ExternalUserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalCustomerId = table.Column<int>(type: "int", nullable: true),
                    ExternalSaleCenterId = table.Column<int>(type: "int", nullable: true),
                    SaleCenterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaleLocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guests = table.Column<int>(type: "int", nullable: true),
                    PrimaryPaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LineCount = table.Column<int>(type: "int", nullable: false),
                    ItemCount = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCachedSalesOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppDashboardSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosConnectionConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusinessDay = table.Column<DateOnly>(type: "date", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNetRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(type: "int", nullable: false),
                    TotalInvoices = table.Column<int>(type: "int", nullable: false),
                    AverageTicket = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItemsSold = table.Column<int>(type: "int", nullable: false),
                    TotalGuests = table.Column<int>(type: "int", nullable: false),
                    TableOrders = table.Column<int>(type: "int", nullable: false),
                    TakeAwayOrders = table.Column<int>(type: "int", nullable: false),
                    DeliveryOrders = table.Column<int>(type: "int", nullable: false),
                    PaymentBreakdownJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopProductsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopFamiliesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HourlySalesJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDashboardSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPosConnectionConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProviderType = table.Column<int>(type: "int", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ApiBaseUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ApiToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    AcmsBaseUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    AcmsApiToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    WorkplaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PosId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultPriceListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KioskIdentifier = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastSuccessfulSync = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPosConnectionConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSyncHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PosConnectionConfigId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SyncType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessDay = table.Column<DateOnly>(type: "date", nullable: true),
                    RecordsSynced = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSyncHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCachedInvoices_TenantId_BusinessDay",
                table: "AppCachedInvoices",
                columns: new[] { "TenantId", "BusinessDay" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCachedInvoices_TenantId_GlobalId",
                table: "AppCachedInvoices",
                columns: new[] { "TenantId", "GlobalId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL AND [GlobalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppCachedSalesOrders_TenantId_BusinessDay",
                table: "AppCachedSalesOrders",
                columns: new[] { "TenantId", "BusinessDay" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCachedSalesOrders_TenantId_GlobalId",
                table: "AppCachedSalesOrders",
                columns: new[] { "TenantId", "GlobalId" },
                unique: true,
                filter: "[TenantId] IS NOT NULL AND [GlobalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppDashboardSnapshots_TenantId_PosConnectionConfigId_BusinessDay",
                table: "AppDashboardSnapshots",
                columns: new[] { "TenantId", "PosConnectionConfigId", "BusinessDay" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppPosConnectionConfigs_TenantId",
                table: "AppPosConnectionConfigs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSyncHistories_TenantId_PosConnectionConfigId_BusinessDay",
                table: "AppSyncHistories",
                columns: new[] { "TenantId", "PosConnectionConfigId", "BusinessDay" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCachedInvoices");

            migrationBuilder.DropTable(
                name: "AppCachedSalesOrders");

            migrationBuilder.DropTable(
                name: "AppDashboardSnapshots");

            migrationBuilder.DropTable(
                name: "AppPosConnectionConfigs");

            migrationBuilder.DropTable(
                name: "AppSyncHistories");
        }
    }
}
