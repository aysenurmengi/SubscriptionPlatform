using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertSentToInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLowStockAlertSent",
                table: "Inventory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLowStockAlertSent",
                table: "Inventory");
        }
    }
}
