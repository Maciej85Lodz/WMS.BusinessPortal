using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WMS.Models;
using WMS.Models.Invent;

namespace WMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Branch> Branch { get; set; }

        public DbSet<Warehouse> Warehouse { get; set; }

        public DbSet<ItemType> ItemTypes { get; set; }

        public DbSet<Vendor> Vendor { get; set; }

        public DbSet<VendorLine> VendorLine { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }

        public DbSet<PurchaseOrderLine> PurchaseOrderLine { get; set; }

        public DbSet<Customer> Customer { get; set; }

        public DbSet<CustomerLine> CustomerLine { get; set; }

        public DbSet<SalesOrder> SalesOrder { get; set; }

        public DbSet<SalesOrderLine> SalesOrderLine { get; set; }

        public DbSet<Shipment> Shipment { get; set; }

        public DbSet<ShipmentLine> ShipmentLine { get; set; }

        public DbSet<Receiving> Receiving { get; set; }

        public DbSet<ReceivingLine> ReceivingLine { get; set; }

        public DbSet<TransferOrder> TransferOrder { get; set; }

        public DbSet<TransferOrderLine> TransferOrderLine { get; set; }

        public DbSet<TransferOut> TransferOut { get; set; }

        public DbSet<TransferOutLine> TransferOutLine { get; set; }

        public DbSet<TransferIn> TransferIn { get; set; }

        public DbSet<TransferInLine> TransferInLine { get; set; }
        
    }
}
