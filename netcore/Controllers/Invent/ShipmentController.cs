using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using WMS.Data;
using WMS.Models.Invent;
using WMS.Services;

namespace WMS.Controllers.Invent
{


    [Authorize(Roles = "Shipment")]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INetcoreService _netcoreService;

        public ShipmentController(ApplicationDbContext context, INetcoreService netcoreService)
        {
            _context = context;
            _netcoreService = netcoreService;
        }

       
        public IActionResult GetWarehouseByOrder(string salesOrderId)
        {
            SalesOrder so = _context.SalesOrder
                .Include(x => x.Branch)
                .Where(x => x.SalesOrderId.Equals(salesOrderId)).FirstOrDefault();

            List<Warehouse> warehouseList = new List<Warehouse>();

            if (so != null)
            {
                warehouseList = _context.Warehouse.Where(x => x.BranchId.Equals(so.Branch.BranchId)).ToList();
            }
            
            warehouseList.Insert(0, new Warehouse { WarehouseId = "0", WarehouseName = "Select" });

            return Json(new SelectList(warehouseList, "WarehouseId", "warehouseName"));
        }

        public async Task<IActionResult> ShowDeliveryOrder(string id)
        {
            Shipment obj = await _context.Shipment
                .Include(x => x.Customer)
                .Include(x => x.SalesOrder)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.ShipmentLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.ShipmentId.Equals(id));
            return View(obj);
        }

        public async Task<IActionResult> PrintDeliveryOrder(string id)
        {
            Shipment obj = await _context.Shipment
                .Include(x => x.Customer)
                .Include(x => x.SalesOrder)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.ShipmentLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.ShipmentId.Equals(id));
            return View(obj);
        }

        // GET: Shipment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Shipment.OrderByDescending(x => x.CreatedAt).Include(s => s.Branch).Include(s => s.Customer).Include(s => s.SalesOrder).Include(s => s.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Shipment/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment
                    .Include(s => s.Branch)
                    .Include(s => s.Customer)
                    .Include(s => s.SalesOrder)
                    .Include(s => s.Warehouse)
                        .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", shipment.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", shipment.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber", shipment.SalesOrderId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", shipment.WarehouseId);
            return View(shipment);
        }


        // GET: Shipment/Create
        public IActionResult Create()
        {
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName");
            List<SalesOrder> soList = _context.SalesOrder.Where(x => x.SalesOrderStatus == SalesOrderStatus.Open).ToList();
            soList.Insert(0, new SalesOrder { SalesOrderId = "0", SalesOrderNumber = "Select" });
            ViewData["SalesOrderId"] = new SelectList(soList, "SalesOrderId", "SalesOrderNumber");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            Shipment shipment = new Shipment();
            return View(shipment);
        }




        // POST: Shipment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("shipmentId,SalesOrderId,shipmentNumber,shipmentDate,CustomerId,customerPO,invoice,BranchId,WarehouseId,expeditionType,expeditionMode,HasChild,createdAt")] Shipment shipment)
        {
            if (shipment.SalesOrderId == "0" || shipment.WarehouseId == "0")
            {
                TempData["StatusMessage"] = "Error. Sales order or Warehouse is not valid. Please select valid sales order and Warehouse";
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                //check sales order
                Shipment check = await _context.Shipment
                    .Include(x => x.SalesOrder)
                    .SingleOrDefaultAsync(x => x.SalesOrderId.Equals(shipment.SalesOrderId));
                if (check != null)
                {
                    ViewData["StatusMessage"] = "Error. Sales order already shipped. " + check.ShipmentNumber;

                    ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName");
                    ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber");
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");

                    return View(shipment);
                }

                //check stock
                bool isStockOK = true;
                string productList = "";
                List<SalesOrderLine> stocklines = new List<SalesOrderLine>();
                stocklines = _context.SalesOrderLine
                    .Include(x => x.ItemType)
                    .Where(x => x.SalesOrderId.Equals(shipment.SalesOrderId)).ToList();
                foreach (var item in stocklines)
                {
                    VMStock stock = _netcoreService.GetStockByItemTypeAndWarehouse(item.ItemTypeId, shipment.WarehouseId);
                    if (stock != null)
                    {
                        if (stock.QtyOnhand < item.Qty)
                        {
                            isStockOK = false;
                            productList = productList + " ["+item.ItemType.ItemCode+"] ";
                        }
                    }
                    else
                    {
                        isStockOK = false;
                    }
                }

                if (!isStockOK)
                {
                    TempData["StatusMessage"] = "Error. Stock quantity problem, please check your on hand stock. " + productList;
                    return RedirectToAction(nameof(Create));
                }

                shipment.Warehouse = await _context.Warehouse.Include(x => x.Branch).SingleOrDefaultAsync(x => x.WarehouseId.Equals(shipment.WarehouseId));
                shipment.Branch = shipment.Warehouse.Branch;
                shipment.SalesOrder = await _context.SalesOrder.Include(x => x.Customer).SingleOrDefaultAsync(x => x.SalesOrderId.Equals(shipment.SalesOrderId));
                shipment.Customer = shipment.SalesOrder.Customer;

                //change status of salesorder
                shipment.SalesOrder.SalesOrderStatus = SalesOrderStatus.Completed;
                _context.Update(shipment.SalesOrder);

                _context.Add(shipment);
                await _context.SaveChangesAsync();

                //auto create shipment line, full shipment
                List<SalesOrderLine> solines = new List<SalesOrderLine>();
                solines = _context.SalesOrderLine.Include(x => x.ItemType).Where(x => x.SalesOrderId.Equals(shipment.SalesOrderId)).ToList();
                foreach (var item in solines)
                {
                    ShipmentLine line = new ShipmentLine
                    {
                        Shipment = shipment,
                        ItemType = item.ItemType,
                        Qty = item.Qty,
                        QtyShipment = item.Qty
                    };
                    line.QtyInventory = line.QtyShipment * -1;
                    line.BranchId = shipment.BranchId;
                    line.WarehouseId = shipment.WarehouseId;

                    _context.ShipmentLine.Add(line);
                    await _context.SaveChangesAsync();
                }

                TempData["TransMessage"] = "Create Shipment " + shipment.ShipmentNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = shipment.ShipmentId });
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", shipment.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", shipment.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber", shipment.SalesOrderId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipment/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment.SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", shipment.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", shipment.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber", shipment.SalesOrderId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", shipment.WarehouseId);
            return View(shipment);
        }

        // POST: Shipment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("shipmentId,SalesOrderId,shipmentNumber,shipmentDate,CustomerId,customerPO,invoice,BranchId,WarehouseId,expeditionType,expeditionMode,HasChild,createdAt")] Shipment shipment)
        {
            if (id != shipment.ShipmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.ShipmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Shipment " + shipment.ShipmentNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", shipment.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", shipment.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber", shipment.SalesOrderId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", shipment.WarehouseId);
            return View(shipment);
        }

        // GET: Shipment/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipment
                    .Include(s => s.Branch)
                    .Include(s => s.Customer)
                    .Include(s => s.SalesOrder)
                    .Include(s => s.Warehouse)
                    .SingleOrDefaultAsync(m => m.ShipmentId == id);
            if (shipment == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", shipment.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", shipment.CustomerId);
            ViewData["SalesOrderId"] = new SelectList(_context.SalesOrder, "SalesOrderId", "SalesOrderNumber", shipment.SalesOrderId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", shipment.WarehouseId);
            return View(shipment);
        }




        // POST: Shipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shipment = await _context.Shipment
                .Include(x => x.SalesOrder)
                .Include(x => x.ShipmentLine)
                .SingleOrDefaultAsync(m => m.ShipmentId == id);
            try
            {
                _context.ShipmentLine.RemoveRange(shipment.ShipmentLine);
                _context.Shipment.Remove(shipment);

                //rollback status to open
                shipment.SalesOrder.SalesOrderStatus = SalesOrderStatus.Open;

                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Shipment " + shipment.ShipmentNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(shipment);
            }
            
        }

        private bool ShipmentExists(string id)
        {
            return _context.Shipment.Any(e => e.ShipmentId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Shipment
        {
            public const string Controller = "Shipment";
            public const string Action = "Index";
            public const string Role = "Shipment";
            public const string Url = "/Shipment/Index";
            public const string Name = "Shipment";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Shipment")]
        public bool ShipmentRole { get; set; } = false;
    }
}



