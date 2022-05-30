using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    public partial class gethelp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferIn_Branch_BranchFrombranchId",
                table: "TransferIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferIn_Branch_BranchTobranchId",
                table: "TransferIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrder_Branch_BranchFrombranchId",
                table: "TransferOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrder_Branch_BranchTobranchId",
                table: "TransferOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOut_Branch_BranchFrombranchId",
                table: "TransferOut");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOut_Branch_BranchTobranchId",
                table: "TransferOut");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Warehouse",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "VendorLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Vendor",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferOutLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferOut",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "BranchTobranchId",
                table: "TransferOut",
                newName: "BranchToBranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFrombranchId",
                table: "TransferOut",
                newName: "BranchFromBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOut_BranchTobranchId",
                table: "TransferOut",
                newName: "IX_TransferOut_BranchToBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOut_BranchFrombranchId",
                table: "TransferOut",
                newName: "IX_TransferOut_BranchFromBranchId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferOrderLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferOrder",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "BranchTobranchId",
                table: "TransferOrder",
                newName: "BranchToBranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFrombranchId",
                table: "TransferOrder",
                newName: "BranchFromBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrder_BranchTobranchId",
                table: "TransferOrder",
                newName: "IX_TransferOrder_BranchToBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrder_BranchFrombranchId",
                table: "TransferOrder",
                newName: "IX_TransferOrder_BranchFromBranchId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferInLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "TransferIn",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "BranchTobranchId",
                table: "TransferIn",
                newName: "BranchToBranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFrombranchId",
                table: "TransferIn",
                newName: "BranchFromBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferIn_BranchTobranchId",
                table: "TransferIn",
                newName: "IX_TransferIn_BranchToBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferIn_BranchFrombranchId",
                table: "TransferIn",
                newName: "IX_TransferIn_BranchFromBranchId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "ShipmentLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Shipment",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "SalesOrderLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "SalesOrder",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "ReceivingLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Receiving",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "PurchaseOrderLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "PurchaseOrder",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "ItemTypes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "CustomerLine",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Customer",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "isDefaultBranch",
                table: "Branch",
                newName: "IsDefaultBranch");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Branch",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Branch",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "branchName",
                table: "Branch",
                newName: "BranchName");

            migrationBuilder.RenameColumn(
                name: "branchId",
                table: "Branch",
                newName: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferIn_Branch_BranchFromBranchId",
                table: "TransferIn",
                column: "BranchFromBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferIn_Branch_BranchToBranchId",
                table: "TransferIn",
                column: "BranchToBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrder_Branch_BranchFromBranchId",
                table: "TransferOrder",
                column: "BranchFromBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrder_Branch_BranchToBranchId",
                table: "TransferOrder",
                column: "BranchToBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOut_Branch_BranchFromBranchId",
                table: "TransferOut",
                column: "BranchFromBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOut_Branch_BranchToBranchId",
                table: "TransferOut",
                column: "BranchToBranchId",
                principalTable: "Branch",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferIn_Branch_BranchFromBranchId",
                table: "TransferIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferIn_Branch_BranchToBranchId",
                table: "TransferIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrder_Branch_BranchFromBranchId",
                table: "TransferOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOrder_Branch_BranchToBranchId",
                table: "TransferOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOut_Branch_BranchFromBranchId",
                table: "TransferOut");

            migrationBuilder.DropForeignKey(
                name: "FK_TransferOut_Branch_BranchToBranchId",
                table: "TransferOut");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Warehouse",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "VendorLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Vendor",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferOutLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferOut",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "BranchToBranchId",
                table: "TransferOut",
                newName: "BranchTobranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFromBranchId",
                table: "TransferOut",
                newName: "BranchFrombranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOut_BranchToBranchId",
                table: "TransferOut",
                newName: "IX_TransferOut_BranchTobranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOut_BranchFromBranchId",
                table: "TransferOut",
                newName: "IX_TransferOut_BranchFrombranchId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferOrderLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferOrder",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "BranchToBranchId",
                table: "TransferOrder",
                newName: "BranchTobranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFromBranchId",
                table: "TransferOrder",
                newName: "BranchFrombranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrder_BranchToBranchId",
                table: "TransferOrder",
                newName: "IX_TransferOrder_BranchTobranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferOrder_BranchFromBranchId",
                table: "TransferOrder",
                newName: "IX_TransferOrder_BranchFrombranchId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferInLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "TransferIn",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "BranchToBranchId",
                table: "TransferIn",
                newName: "BranchTobranchId");

            migrationBuilder.RenameColumn(
                name: "BranchFromBranchId",
                table: "TransferIn",
                newName: "BranchFrombranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferIn_BranchToBranchId",
                table: "TransferIn",
                newName: "IX_TransferIn_BranchTobranchId");

            migrationBuilder.RenameIndex(
                name: "IX_TransferIn_BranchFromBranchId",
                table: "TransferIn",
                newName: "IX_TransferIn_BranchFrombranchId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ShipmentLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Shipment",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SalesOrderLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "SalesOrder",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ReceivingLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Receiving",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseOrderLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseOrder",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ItemTypes",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "CustomerLine",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Customer",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "IsDefaultBranch",
                table: "Branch",
                newName: "isDefaultBranch");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Branch",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Branch",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "BranchName",
                table: "Branch",
                newName: "branchName");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Branch",
                newName: "branchId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferIn_Branch_BranchFrombranchId",
                table: "TransferIn",
                column: "BranchFrombranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferIn_Branch_BranchTobranchId",
                table: "TransferIn",
                column: "BranchTobranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrder_Branch_BranchFrombranchId",
                table: "TransferOrder",
                column: "BranchFrombranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOrder_Branch_BranchTobranchId",
                table: "TransferOrder",
                column: "BranchTobranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOut_Branch_BranchFrombranchId",
                table: "TransferOut",
                column: "BranchFrombranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransferOut_Branch_BranchTobranchId",
                table: "TransferOut",
                column: "BranchTobranchId",
                principalTable: "Branch",
                principalColumn: "branchId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
