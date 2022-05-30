using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    public partial class updateVendornaingconvertion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "vendorName",
                table: "Vendor",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "size",
                table: "Vendor",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Vendor",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "vendorId",
                table: "Vendor",
                newName: "VendorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "Vendor",
                newName: "vendorName");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Vendor",
                newName: "size");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Vendor",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Vendor",
                newName: "vendorId");
        }
    }
}
