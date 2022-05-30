using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WMS.Migrations
{
    public partial class Updatecustomernamingconversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HomeRole = table.Column<bool>(type: "bit", nullable: false),
                    BranchRole = table.Column<bool>(type: "bit", nullable: false),
                    CustomerRole = table.Column<bool>(type: "bit", nullable: false),
                    CustomerLineRole = table.Column<bool>(type: "bit", nullable: false),
                    ItemTypeRole = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseOrderRole = table.Column<bool>(type: "bit", nullable: false),
                    PurchaseOrderLineRole = table.Column<bool>(type: "bit", nullable: false),
                    ReceivingRole = table.Column<bool>(type: "bit", nullable: false),
                    ReceivingLineRole = table.Column<bool>(type: "bit", nullable: false),
                    SalesOrderRole = table.Column<bool>(type: "bit", nullable: false),
                    SalesOrderLineRole = table.Column<bool>(type: "bit", nullable: false),
                    ShipmentRole = table.Column<bool>(type: "bit", nullable: false),
                    ShipmentLineRole = table.Column<bool>(type: "bit", nullable: false),
                    StockRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferInRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferInLineRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferOrderRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferOrderLineRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferOutRole = table.Column<bool>(type: "bit", nullable: false),
                    TransferOutLineRole = table.Column<bool>(type: "bit", nullable: false),
                    VendorRole = table.Column<bool>(type: "bit", nullable: false),
                    VendorLineRole = table.Column<bool>(type: "bit", nullable: false),
                    WarehouseRole = table.Column<bool>(type: "bit", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserRole = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    branchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    branchName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDefaultBranch = table.Column<bool>(type: "bit", nullable: false),
                    Street1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.branchId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Street1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ItemTypes",
                columns: table => new
                {
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ManufacturersNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ItemTypeType = table.Column<int>(type: "int", nullable: false),
                    UOM = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTypes", x => x.ItemTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    vendorId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    vendorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    size = table.Column<int>(type: "int", nullable: false),
                    Street1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.vendorId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Warehouse",
                columns: table => new
                {
                    warehouseId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    warehouseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    branchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Street1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Province = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouse", x => x.warehouseId);
                    table.ForeignKey(
                        name: "FK_Warehouse_Branch_branchId",
                        column: x => x.branchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerLine",
                columns: table => new
                {
                    CustomerLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Salutation = table.Column<int>(type: "int", nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OfficePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLine", x => x.CustomerLineId);
                    table.ForeignKey(
                        name: "FK_CustomerLine_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrder",
                columns: table => new
                {
                    SalesOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    SalesOrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Top = table.Column<int>(type: "int", nullable: false),
                    SalesOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNumberInternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReferenceNumberExternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    PicInternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PicCustomer = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SalesOrderStatus = table.Column<int>(type: "int", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalesShipmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrder", x => x.SalesOrderId);
                    table.ForeignKey(
                        name: "FK_SalesOrder_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrder_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    top = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNumberInternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ReferenceNumberExternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    VendorId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    PicInternal = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PicVendor = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PurchaseOrderStatus = table.Column<int>(type: "int", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseReceiveNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "vendorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorLine",
                columns: table => new
                {
                    vendorLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    jobTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    vendorId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Salutation = table.Column<int>(type: "int", nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OfficePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WorkEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorLine", x => x.vendorLineId);
                    table.ForeignKey(
                        name: "FK_VendorLine_Vendor_vendorId",
                        column: x => x.vendorId,
                        principalTable: "Vendor",
                        principalColumn: "vendorId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrder",
                columns: table => new
                {
                    TransferOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransferOrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PicName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchFrombranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseFromwarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    BranchIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchTobranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseTowarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    TransferOrderStatus = table.Column<int>(type: "int", nullable: false),
                    IsIssued = table.Column<bool>(type: "bit", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrder", x => x.TransferOrderId);
                    table.ForeignKey(
                        name: "FK_TransferOrder_Branch_BranchFrombranchId",
                        column: x => x.BranchFrombranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrder_Branch_BranchTobranchId",
                        column: x => x.BranchTobranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrder_Warehouse_WarehouseFromwarehouseId",
                        column: x => x.WarehouseFromwarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrder_Warehouse_WarehouseTowarehouseId",
                        column: x => x.WarehouseTowarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderLine",
                columns: table => new
                {
                    SalesOrderLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    SalesOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Qty = table.Column<float>(type: "real", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrderLine", x => x.SalesOrderLineId);
                    table.ForeignKey(
                        name: "FK_SalesOrderLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesOrderLine_SalesOrder_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrder",
                        principalColumn: "SalesOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    ShipmentId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    SalesOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ShipmentNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    CustomerPurchaseOrder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ExpeditionType = table.Column<int>(type: "int", nullable: false),
                    ExpeditionMode = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK_Shipment_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shipment_SalesOrder_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "SalesOrder",
                        principalColumn: "SalesOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipment_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLine",
                columns: table => new
                {
                    purchaseOrderLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    PurchaseOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderLine", x => x.purchaseOrderLineId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLine_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receiving",
                columns: table => new
                {
                    ReceivingId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    PurchaseOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ReceivingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    VendorDONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VendorInvoice = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receiving", x => x.ReceivingId);
                    table.ForeignKey(
                        name: "FK_Receiving_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receiving_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receiving_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "vendorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Receiving_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferIn",
                columns: table => new
                {
                    TansferInId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferInNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransferInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchFrombranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseFromwarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    BranchIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchTobranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseTowarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferIn", x => x.TansferInId);
                    table.ForeignKey(
                        name: "FK_TransferIn_Branch_BranchFrombranchId",
                        column: x => x.BranchFrombranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferIn_Branch_BranchTobranchId",
                        column: x => x.BranchTobranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferIn_TransferOrder_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrder",
                        principalColumn: "TransferOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferIn_Warehouse_WarehouseFromwarehouseId",
                        column: x => x.WarehouseFromwarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferIn_Warehouse_WarehouseTowarehouseId",
                        column: x => x.WarehouseTowarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderLine",
                columns: table => new
                {
                    TransferOrderLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Qty = table.Column<float>(type: "real", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderLine", x => x.TransferOrderLineId);
                    table.ForeignKey(
                        name: "FK_TransferOrderLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrderLine_TransferOrder_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrder",
                        principalColumn: "TransferOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferOut",
                columns: table => new
                {
                    TransferOutId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOrderId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOutNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransferOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BranchIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchFrombranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdFrom = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseFromwarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    BranchIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchTobranchId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    WarehouseIdTo = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseTowarehouseId = table.Column<string>(type: "nvarchar(38)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasChild = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOut", x => x.TransferOutId);
                    table.ForeignKey(
                        name: "FK_TransferOut_Branch_BranchFrombranchId",
                        column: x => x.BranchFrombranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOut_Branch_BranchTobranchId",
                        column: x => x.BranchTobranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOut_TransferOrder_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrder",
                        principalColumn: "TransferOrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferOut_Warehouse_WarehouseFromwarehouseId",
                        column: x => x.WarehouseFromwarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOut_Warehouse_WarehouseTowarehouseId",
                        column: x => x.WarehouseTowarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentLine",
                columns: table => new
                {
                    ShipmentLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ShipmentId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Qty = table.Column<float>(type: "real", nullable: false),
                    QtyShipment = table.Column<float>(type: "real", nullable: false),
                    QtyInventory = table.Column<float>(type: "real", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentLine", x => x.ShipmentLineId);
                    table.ForeignKey(
                        name: "FK_ShipmentLine_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentLine_Shipment_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipment",
                        principalColumn: "ShipmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShipmentLine_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingLine",
                columns: table => new
                {
                    ReceivingLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    ReceivingId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    BranchId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    WarehouseId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    QtyOrder = table.Column<float>(type: "real", nullable: false),
                    QtyReceive = table.Column<float>(type: "real", nullable: false),
                    QtyInventory = table.Column<float>(type: "real", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingLine", x => x.ReceivingLineId);
                    table.ForeignKey(
                        name: "FK_ReceivingLine_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "branchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLine_Receiving_ReceivingId",
                        column: x => x.ReceivingId,
                        principalTable: "Receiving",
                        principalColumn: "ReceivingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceivingLine_Warehouse_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "warehouseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferInLine",
                columns: table => new
                {
                    TransferInLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferInId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Qty = table.Column<float>(type: "real", nullable: false),
                    QtyInventory = table.Column<float>(type: "real", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferInLine", x => x.TransferInLineId);
                    table.ForeignKey(
                        name: "FK_TransferInLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferInLine_TransferIn_TransferInId",
                        column: x => x.TransferInId,
                        principalTable: "TransferIn",
                        principalColumn: "TansferInId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferOutLine",
                columns: table => new
                {
                    TransferOutLineId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false),
                    TransferOutId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    ItemTypeId = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: true),
                    Qty = table.Column<float>(type: "real", nullable: false),
                    QtyInventory = table.Column<float>(type: "real", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOutLine", x => x.TransferOutLineId);
                    table.ForeignKey(
                        name: "FK_TransferOutLine_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "ItemTypes",
                        principalColumn: "ItemTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOutLine_TransferOut_TransferOutId",
                        column: x => x.TransferOutId,
                        principalTable: "TransferOut",
                        principalColumn: "TransferOutId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLine_CustomerId",
                table: "CustomerLine",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_BranchId",
                table: "PurchaseOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_VendorId",
                table: "PurchaseOrder",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_ItemTypeId",
                table: "PurchaseOrderLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLine_PurchaseOrderId",
                table: "PurchaseOrderLine",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_BranchId",
                table: "Receiving",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_PurchaseOrderId",
                table: "Receiving",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_VendorId",
                table: "Receiving",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Receiving_WarehouseId",
                table: "Receiving",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_BranchId",
                table: "ReceivingLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_ItemTypeId",
                table: "ReceivingLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_ReceivingId",
                table: "ReceivingLine",
                column: "ReceivingId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingLine_WarehouseId",
                table: "ReceivingLine",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_BranchId",
                table: "SalesOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrder_CustomerId",
                table: "SalesOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLine_ItemTypeId",
                table: "SalesOrderLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLine_SalesOrderId",
                table: "SalesOrderLine",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_BranchId",
                table: "Shipment",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_CustomerId",
                table: "Shipment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_SalesOrderId",
                table: "Shipment",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_WarehouseId",
                table: "Shipment",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_BranchId",
                table: "ShipmentLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_ItemTypeId",
                table: "ShipmentLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_ShipmentId",
                table: "ShipmentLine",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentLine_WarehouseId",
                table: "ShipmentLine",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferIn_BranchFrombranchId",
                table: "TransferIn",
                column: "BranchFrombranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferIn_BranchTobranchId",
                table: "TransferIn",
                column: "BranchTobranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferIn_TransferOrderId",
                table: "TransferIn",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferIn_WarehouseFromwarehouseId",
                table: "TransferIn",
                column: "WarehouseFromwarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferIn_WarehouseTowarehouseId",
                table: "TransferIn",
                column: "WarehouseTowarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferInLine_ItemTypeId",
                table: "TransferInLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferInLine_TransferInId",
                table: "TransferInLine",
                column: "TransferInId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrder_BranchFrombranchId",
                table: "TransferOrder",
                column: "BranchFrombranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrder_BranchTobranchId",
                table: "TransferOrder",
                column: "BranchTobranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrder_WarehouseFromwarehouseId",
                table: "TransferOrder",
                column: "WarehouseFromwarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrder_WarehouseTowarehouseId",
                table: "TransferOrder",
                column: "WarehouseTowarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLine_ItemTypeId",
                table: "TransferOrderLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLine_TransferOrderId",
                table: "TransferOrderLine",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOut_BranchFrombranchId",
                table: "TransferOut",
                column: "BranchFrombranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOut_BranchTobranchId",
                table: "TransferOut",
                column: "BranchTobranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOut_TransferOrderId",
                table: "TransferOut",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOut_WarehouseFromwarehouseId",
                table: "TransferOut",
                column: "WarehouseFromwarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOut_WarehouseTowarehouseId",
                table: "TransferOut",
                column: "WarehouseTowarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOutLine_ItemTypeId",
                table: "TransferOutLine",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOutLine_TransferOutId",
                table: "TransferOutLine",
                column: "TransferOutId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorLine_vendorId",
                table: "VendorLine",
                column: "vendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouse_branchId",
                table: "Warehouse",
                column: "branchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CustomerLine");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLine");

            migrationBuilder.DropTable(
                name: "ReceivingLine");

            migrationBuilder.DropTable(
                name: "SalesOrderLine");

            migrationBuilder.DropTable(
                name: "ShipmentLine");

            migrationBuilder.DropTable(
                name: "TransferInLine");

            migrationBuilder.DropTable(
                name: "TransferOrderLine");

            migrationBuilder.DropTable(
                name: "TransferOutLine");

            migrationBuilder.DropTable(
                name: "VendorLine");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Receiving");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "TransferIn");

            migrationBuilder.DropTable(
                name: "ItemTypes");

            migrationBuilder.DropTable(
                name: "TransferOut");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropTable(
                name: "SalesOrder");

            migrationBuilder.DropTable(
                name: "TransferOrder");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Warehouse");

            migrationBuilder.DropTable(
                name: "Branch");
        }
    }
}
