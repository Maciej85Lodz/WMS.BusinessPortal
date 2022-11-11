using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WMS.Data;
using WMS.Models;
using WMS.Models.Invent;
using WMS.MVC;

namespace WMS.Services
{
    public class NetcoreService : INetcoreService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public NetcoreService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IRoles roles,
            IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }

        public async Task SendEmailBySendGridAsync(string apiKey, string fromEmail, string fromFullName, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var smtp = new SmtpClient();
            var credential = new NetworkCredential
            {
                UserName = smtpUser,
                Password = smtpPassword
            };
            smtp.Credentials = credential;
            smtp.Host = smtpHost;
            smtp.Port = smtpPort;
            smtp.EnableSsl = smtpSSL;
            await smtp.SendMailAsync(message);

        }

        public async Task<bool> IsAccountActivatedAsync(string email, UserManager<ApplicationUser> userManager)
        {
            bool result = false;
            try
            {
                var user = await userManager.FindByNameAsync(email);
                if (user != null)
                {
                    //Add this to check if the email was confirmed.
                    if (await userManager.IsEmailConfirmedAsync(user))
                    {
                        result = true;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }


        public async Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = System.IO.Path.Combine(webRoot, "uploads");
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = System.IO.Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = System.IO.Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }

        public async Task UpdateRoles(ApplicationUser appUser,
            ApplicationUser currentLoginUser)
        {
            try
            {
                await _roles.UpdateRoles(appUser, currentLoginUser);

                //so no need to manually re-signIn to make roles changes effective
                if (currentLoginUser.Id == appUser.Id)
                {
                    await _signInManager.SignInAsync(appUser, false);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                ApplicationUser superAdmin = new ApplicationUser
                {
                    Email = _superAdminDefaultOptions.Email
                };
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;
                superAdmin.IsSuperAdmin = true;

                Type t = superAdmin.GetType();
                foreach (System.Reflection.PropertyInfo item in t.GetProperties())
                {
                    if (item.Name.Contains("Role"))
                    {
                        item.SetValue(superAdmin, true);
                    }
                }

                await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                //loop all the roles and then fill to SuperAdmin so he become powerfull
                foreach (var item in typeof(Pages).GetNestedTypes())
                {
                    var roleName = item.Name;
                    if (!await _roleManager.RoleExistsAsync(roleName)) { await _roleManager.CreateAsync(new IdentityRole(roleName)); }

                    await _userManager.AddToRoleAsync(superAdmin, roleName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VMStock GetStockByItemTypeAndWarehouse(string ItemTypeId, string WarehouseId)
        {
            VMStock result = new VMStock();

            try
            {
                ItemType itemType = _context.ItemTypes.FirstOrDefault(x => x.ItemTypeId.Equals(ItemTypeId));
                Warehouse warehouse = _context.Warehouse.FirstOrDefault(x => x.WarehouseId.Equals(WarehouseId));

                if (itemType != null && warehouse != null)
                {
                    VMStock stock = new VMStock
                    {
                        ItemType = itemType.ItemCode,
                        Warehouse = warehouse.WarehouseName,
                        QtyReceiving = _context.ReceivingLine.Where(x => x.ItemTypeId.Equals(itemType.ItemTypeId) && x.WarehouseId.Equals(warehouse.WarehouseId)).Sum(x => x.QtyReceive),
                        QtyShipment = _context.ShipmentLine.Where(x => x.ItemTypeId.Equals(itemType.ItemTypeId) && x.WarehouseId.Equals(warehouse.WarehouseId)).Sum(x => x.QtyShipment),
                        QtyTransferIn = _context.TransferInLine.Where(x => x.ItemTypeId.Equals(itemType.ItemTypeId) && x.TransferIn.WarehouseIdTo.Equals(warehouse.WarehouseId)).Sum(x => x.Qty),
                        QtyTransferOut = _context.TransferOutLine.Where(x => x.ItemTypeId.Equals(itemType.ItemTypeId) && x.TransferOut.WarehouseIdFrom.Equals(warehouse.WarehouseId)).Sum(x => x.Qty)
                    };
                    stock.QtyOnhand = stock.QtyReceiving + stock.QtyTransferIn - stock.QtyShipment - stock.QtyTransferOut;

                    result = stock;
                }

                
            }
            catch (Exception)
            {

                throw;
            }

            return result;

        }

        public List<VMStock> GetStockPerWarehouse()
        {
            List<VMStock> result = new List<VMStock>();

            try
            {
                List<VMStock> stocks = new List<VMStock>();
                List<ItemType> itemTypes = new List<ItemType>();
                List<Warehouse> warehouses = new List<Warehouse>();
                warehouses = _context.Warehouse.ToList();
                itemTypes = _context.ItemTypes.ToList();
                foreach (var item in itemTypes)
                {
                    foreach (var wh in warehouses)
                    {
                        VMStock stock = stock = GetStockByItemTypeAndWarehouse(item.ItemTypeId, wh.WarehouseId);
                        
                        if (stock != null) stocks.Add(stock);

                    }


                }

                result = stocks;
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }

        public List<TimelineViewModel> GetTimelinesByItemTypeId(string ItemTypeId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<PurchaseOrderLine> polist = _context.PurchaseOrderLine.Include(x => x.PurchaseOrder).Where(x => x.ItemTypeId.Equals(ItemTypeId)).OrderByDescending(x => x.PurchaseOrder.PurchaseOrderDate).Take(3).ToList();
                List<SalesOrderLine> solist = _context.SalesOrderLine.Include(x => x.SalesOrder).Where(x => x.ItemTypeId.Equals(ItemTypeId)).OrderByDescending(x => x.SalesOrder.SalesOrderDate).Take(3).ToList();

                foreach (var item in polist)
                {
                    times.Add(new TimelineViewModel { Header = item.PurchaseOrder.PurchaseOrderDate.ToString("yyyy-MMM-dd"), Body = "New Purchase Order Created: " + item.PurchaseOrder.PurchaseOrderNumber + " ", Icon = "fa-file-text", CretedDate = item.PurchaseOrder.PurchaseOrderDate });
                }

                foreach (var item in solist)
                {
                    times.Add(new TimelineViewModel { Header = item.SalesOrder.SalesOrderDate.ToString("yyyy-MMM-dd"), Body = "New Sales Order Created: " + item.SalesOrder.SalesOrderNumber + " ", Icon = "fa-cart-plus", CretedDate = item.SalesOrder.SalesOrderDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesByBranchId(string BranchId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<PurchaseOrder> polist = _context.PurchaseOrder.Where(x => x.BranchId.Equals(BranchId)).OrderByDescending(x => x.PurchaseOrderDate).Take(3).ToList();
                List<SalesOrder> solist = _context.SalesOrder.Where(x => x.BranchId.Equals(BranchId)).OrderByDescending(x => x.SalesOrderDate).Take(3).ToList();

                foreach (var item in polist)
                {
                    times.Add(new TimelineViewModel { Header = item.PurchaseOrderDate.ToString("yyyy-MMM-dd"), Body = "New Purchase Order Created: " + item.PurchaseOrderNumber + " ", Icon = "fa-file-text", CretedDate = item.PurchaseOrderDate });
                }

                foreach (var item in solist)
                {
                    times.Add(new TimelineViewModel { Header = item.SalesOrderDate.ToString("yyyy-MMM-dd"), Body = "New Sales Order Created: " + item.SalesOrderNumber + " ", Icon = "fa-cart-plus", CretedDate = item.SalesOrderDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesByVendorId(string vendorId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<PurchaseOrder> polist = _context.PurchaseOrder.Where(x => x.VendorId.Equals(vendorId)).OrderByDescending(x => x.PurchaseOrderDate).Take(6).ToList();

                foreach (var item in polist)
                {
                    times.Add(new TimelineViewModel { Header = item.PurchaseOrderDate.ToString("yyyy-MMM-dd"), Body = "New Purchase Order Created: " + item.PurchaseOrderNumber + " ", Icon = "fa-file-text", CretedDate = item.PurchaseOrderDate });
                }
                

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesByCustomerId(string customerId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<SalesOrder> solist = _context.SalesOrder.Where(x => x.CustomerId.Equals(customerId)).OrderByDescending(x => x.SalesOrderDate).Take(3).ToList();
                
                foreach (var item in solist)
                {
                    times.Add(new TimelineViewModel { Header = item.SalesOrderDate.ToString("yyyy-MMM-dd"), Body = "New Sales Order Created: " + item.SalesOrderNumber + " ", Icon = "fa-cart-plus", CretedDate = item.SalesOrderDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesByPurchaseId(string purchaseId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<Receiving> list = _context.Receiving.Where(x => x.PurchaseOrderId.Equals(purchaseId)).ToList();

                foreach (var item in list)
                {
                    times.Add(new TimelineViewModel { Header = item.ReceivingDate.ToString("yyyy-MMM-dd"), Body = "New Receiving Created: " + item.ReceivingNumber + " ", Icon = "fa-truck", CretedDate = item.ReceivingDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesBySalesId(string salesId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<Shipment> list = _context.Shipment.Where(x => x.SalesOrderId.Equals(salesId)).ToList();

                foreach (var item in list)
                {
                    times.Add(new TimelineViewModel { Header = item.ShipmentDate.ToString("yyyy-MMM-dd"), Body = "New Shipment Created: " + item.ShipmentNumber + " ", Icon = "fa-plane", CretedDate = item.ShipmentDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public List<TimelineViewModel> GetTimelinesByTransferId(string transferId)
        {
            List<TimelineViewModel> results = new List<TimelineViewModel>();

            try
            {
                List<TimelineViewModel> times = new List<TimelineViewModel>();
                List<TransferOut> outlist = _context.TransferOut.Where(x => x.TransferOrderId.Equals(transferId)).OrderByDescending(x => x.TransferOutDate).Take(3).ToList();
                List<TransferIn> inlist = _context.TransferIn.Where(x => x.TransferOrderId.Equals(transferId)).OrderByDescending(x => x.TransferInDate).Take(3).ToList();

                foreach (var item in outlist)
                {
                    times.Add(new TimelineViewModel { Header = item.TransferOutDate.ToString("yyyy-MMM-dd"), Body = "New Goods Issue Created: " + item.TransferOutNumber + " ", Icon = "fa-upload", CretedDate = item.TransferOutDate });
                }

                foreach (var item in inlist)
                {
                    times.Add(new TimelineViewModel { Header = item.TransferInDate.ToString("yyyy-MMM-dd"), Body = "New Goods Receive Created: " + item.TransferInNumber + " ", Icon = "fa-download", CretedDate = item.TransferInDate });
                }

                results = times.OrderByDescending(x => x.CretedDate).ToList();
            }
            catch (Exception)
            {

                throw;
            }

            return results;
        }

        public async Task InitDemo()
        {
            try
            {
              

                Branch Branch = new Branch() { BranchName = "HQ", IsDefaultBranch = true, Street1 = "Rua Orós, 92" };
                _context.Branch.Add(Branch);

                List<Warehouse> whs = new List<Warehouse>() {
                    new Warehouse{WarehouseName = "WH1", Branch = Branch, Street1 = "Rua Orós, 92"},
                    new Warehouse{WarehouseName = "WH2", Branch = Branch, Street1 = "C/ Moralzarzal, 86"},
                    new Warehouse{WarehouseName = "WH3", Branch = Branch, Street1 = "184, chaussée de Tournai"},
                    new Warehouse{WarehouseName = "WH4", Branch = Branch, Street1 = "Åkergatan 24"},
                    new Warehouse{WarehouseName = "WH5", Branch = Branch, Street1 = "Berliner Platz 43"}
                };

                _context.Warehouse.AddRange(whs);

                List<ItemType> itemTypes = new List<ItemType>() {
                    new ItemType{ItemCode = "Chai", ItemTypeName = "Chai", ItemTypeType = ItemTypeType.Food, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Chang", ItemTypeName = "Chang", ItemTypeType = ItemTypeType.Food, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Aniseed Syrup", ItemTypeName = "Aniseed Syrup", ItemTypeType = ItemTypeType.Food, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Chef Anton's Cajun Seasoning", ItemTypeName = "Chef Anton's Cajun Seasoning", ItemTypeType = ItemTypeType.Food, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Chef Anton's Gumbo Mix", ItemTypeName = "Chef Anton's Gumbo Mix", ItemTypeType = ItemTypeType.Food, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Grandma's Boysenberry Spread", ItemTypeName = "Grandma's Boysenberry Spread", ItemTypeType = ItemTypeType.Electronic, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Uncle Bob's Organic Dried Pears", ItemTypeName = "Uncle Bob's Organic Dried Pears", ItemTypeType = ItemTypeType.Electronic, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Northwoods Cranberry Sauce", ItemTypeName = "Northwoods Cranberry Sauce", ItemTypeType = ItemTypeType.Electronic, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Mishi Kobe Niku", ItemTypeName = "Mishi Kobe Niku", ItemTypeType = ItemTypeType.Electronic, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Ikura", ItemTypeName = "Ikura", ItemTypeType = ItemTypeType.Electronic, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Queso Cabrales", ItemTypeName = "Queso Cabrales", ItemTypeType = ItemTypeType.FMCG, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Queso Manchego La Pastora", ItemTypeName = "Queso Manchego La Pastora", ItemTypeType = ItemTypeType.FMCG, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Konbu", ItemTypeName = "Konbu", ItemTypeType = ItemTypeType.FMCG, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Tofu", ItemTypeName = "Tofu", ItemTypeType = ItemTypeType.FMCG, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Genen Shouyu", ItemTypeName = "Genen Shouyu", ItemTypeType = ItemTypeType.FMCG, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Pavlova", ItemTypeName = "Pavlova", ItemTypeType = ItemTypeType.Fashion, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Alice Mutton", ItemTypeName = "Alice Mutton", ItemTypeType = ItemTypeType.Fashion, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Carnarvon Tigers", ItemTypeName = "Carnarvon Tigers", ItemTypeType = ItemTypeType.Fashion, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Teatime Chocolate Biscuits", ItemTypeName = "Teatime Chocolate Biscuits", ItemTypeType = ItemTypeType.Fashion, UOM = UOM.Pcs},
                    new ItemType{ItemCode = "Sir Rodney's Marmalade", ItemTypeName = "Sir Rodney's Marmalade", ItemTypeType = ItemTypeType.Fashion, UOM = UOM.Pcs}

                };
                _context.ItemTypes.AddRange(itemTypes);

                List<Vendor> vendors = new List<Vendor>() {
                    new Vendor{VendorName = "Exotic Liquids", Street1 = "49 Gilbert St.", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "New Orleans Cajun Delights", Street1 = "P.O. Box 78934", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Grandma Kelly's Homestead", Street1 = "707 Oxford Rd.", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Tokyo Traders", Street1 = "9-8 Sekimai Musashino-shi", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Cooperativa de Quesos 'Las Cabras'", Street1 = "Calle del Rosal 4", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Mayumi's", Street1 = "92 Setsuko Chuo-ku", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Pavlova, Ltd.", Street1 = "74 Rose St. Moonie Ponds", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Specialty Biscuits, Ltd.", Street1 = "29 King's Way", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "PB Knäckebröd AB", Street1 = "Kaloadagatan 13", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Refrescos Americanas LTDA", Street1 = "Av. das Americanas 12.890", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Heli Süßwaren GmbH & Co. KG", Street1 = "Tiergartenstraße 5", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Plutzer Lebensmittelgroßmärkte AG", Street1 = "Bogenallee 51", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Nord-Ost-Fisch Handelsgesellschaft mbH", Street1 = "Frahmredder 112a", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Formaggi Fortini s.r.l.", Street1 = "Viale Dante, 75", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Norske Meierier", Street1 = "Hatlevegen 5", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Bigfoot Breweries", Street1 = "3400 - 8th Avenue Suite 210", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Svensk Sjöföda AB", Street1 = "Brovallavägen 231", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "Aux joyeux ecclésiastiques", Street1 = "203, Rue des Francs-Bourgeois", Size = BusinessSize.Enterprise},
                    new Vendor{VendorName = "New England Seafood Cannery", Street1 = "Order Processing Dept. 2100 Paul Revere Blvd.", Size = BusinessSize.Enterprise}
                };
                _context.Vendor.AddRange(vendors);

                List<Customer> customers = new List<Customer>() {
                    new Customer{CustomerName = "Hanari Carnes", Street1 = "Rua do Paço, 67", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "HILARION-Abastos", Street1 = "Carrera 22 con Ave. Carlos Soublette #8-35", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Hungry Coyote Import Store", Street1 = "City Center Plaza 516 Main St.", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Hungry Owl All-Night Grocers", Street1 = "8 Johnstown Road", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Island Trading", Street1 = "Garden House Crowther Way", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Königlich Essen", Street1 = "Maubelstr. 90", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "La corne d'abondance", Street1 = "67, avenue de l'Europe", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "La maison d'Asie", Street1 = "1 rue Alsace-Lorraine", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Laughing Bacchus Wine Cellars", Street1 = "1900 Oak St.", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Lazy K Kountry Store", Street1 = "12 Orchestra Terrace", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Lehmanns Marktstand", Street1 = "Magazinweg 7", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Let's Stop N Shop", Street1 = "87 Polk St. Suite 5", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "LILA-Supermercado", Street1 = "Carrera 52 con Ave. Bolívar #65-98 Llano Largo", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "LINO-Delicateses", Street1 = "Ave. 5 de Mayo Porlamar", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Lonesome Pine Restaurant", Street1 = "89 Chiaroscuro Rd.", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Magazzini Alimentari Riuniti", Street1 = "Via Ludovico il Moro 22", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Maison Dewey", Street1 = "Rue Joseph-Bens 532", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Mère Paillarde", Street1 = "43 rue St. Laurent", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Morgenstern Gesundkost", Street1 = "Heerstr. 22", Size = BusinessSize.Enterprise},
                    new Customer{CustomerName = "Old World Delicatessen", Street1 = "2743 Bering St.", Size = BusinessSize.Enterprise}
                };
                _context.Customer.AddRange(customers);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
