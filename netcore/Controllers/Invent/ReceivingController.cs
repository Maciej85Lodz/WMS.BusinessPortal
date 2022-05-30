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

namespace WMS.Controllers.Invent
{


    [Authorize(Roles = "Receiving")]
    public class ReceivingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceivingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetWarehouseByOrder(string purchaseOrderId)
        {
            PurchaseOrder po = _context.PurchaseOrder
                .Include(x => x.Branch)
                .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId)).FirstOrDefault();

            List<Warehouse> warehouseList = new List<Warehouse>();

            if (po != null)
            {
                warehouseList = _context.Warehouse.Where(x => x.branchId.Equals(po.Branch.BranchId)).ToList();
            }
            
            warehouseList.Insert(0, new Warehouse { warehouseId = "0", warehouseName = "Select" });

            return Json(new SelectList(warehouseList, "WarehouseId", "warehouseName"));
        }

        public async Task<IActionResult> ShowGSRN(string id)
        {
            Receiving obj = await _context.Receiving
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseOrder)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.ReceivingLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.ReceivingId.Equals(id));
            return View(obj);
        }

        public async Task<IActionResult> PrintGSRN(string id)
        {
            Receiving obj = await _context.Receiving
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseOrder)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.ReceivingLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.ReceivingId.Equals(id));
            return View(obj);
        }

        // GET: Receiving
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Receiving.OrderByDescending(x => x.CreatedAt).Include(r => r.Branch).Include(r => r.PurchaseOrder).Include(r => r.Vendor).Include(r => r.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Receiving/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiving = await _context.Receiving
                    .Include(r => r.Branch)
                    .Include(r => r.PurchaseOrder)
                    .Include(r => r.Vendor)
                    .Include(r => r.Warehouse)
                        .SingleOrDefaultAsync(m => m.ReceivingId == id);
            if (receiving == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receiving.BranchId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber", receiving.PurchaseOrderId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", receiving.VendorId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receiving.WarehouseId);

            return View(receiving);
        }


        // GET: Receiving/Create
        public IActionResult Create()
        {
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName");
            List<PurchaseOrder> poList = _context.PurchaseOrder.Where(x => x.PurchaseOrderStatus == PurchaseOrderStatus.Open).ToList();
            poList.Insert(0, new PurchaseOrder { PurchaseOrderId = "0", PurchaseOrderNumber = "Select" });
            ViewData["PurchaseOrderId"] = new SelectList(poList, "PurchaseOrderId", "PurchaseOrderNumber");
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            Receiving rcv = new Receiving();
            return View(rcv);
        }




        // POST: Receiving/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceivingId,PurchaseOrderId,ReceivingNumber,receivingDate,VendorId,VendorDONumber,VendorInvoice,BranchId,WarehouseId,HasChild,createdAt")] Receiving receiving)
        {
            if (receiving.PurchaseOrderId == "0" || receiving.WarehouseId == "0")
            {
                TempData["StatusMessage"] = "Error. Purchase order or Warehouse is not valid. Please select valid purchase order and Warehouse";
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                //check Receiving
                Receiving check = await _context.Receiving.SingleOrDefaultAsync(x => x.PurchaseOrderId.Equals(receiving.PurchaseOrderId));
                if (check != null)
                {
                    ViewData["StatusMessage"] = "Error. Purchase order already received. " + check.ReceivingNumber;

                    ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber");
                    ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName");
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");

                    return View(receiving);
                }
                receiving.Warehouse = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(receiving.WarehouseId));
                receiving.Branch = receiving.Warehouse.branch;
                receiving.PurchaseOrder = await _context.PurchaseOrder.Include(x => x.Vendor).SingleOrDefaultAsync(x => x.PurchaseOrderId.Equals(receiving.PurchaseOrderId));
                receiving.Vendor = receiving.PurchaseOrder.Vendor;

                _context.Add(receiving);

                //change status of purchase order to completed
                receiving.PurchaseOrder.PurchaseOrderStatus = PurchaseOrderStatus.Completed;
                _context.PurchaseOrder.Update(receiving.PurchaseOrder);

                await _context.SaveChangesAsync();

                //auto create Receiving line, full receive
                List<PurchaseOrderLine> polines = new List<PurchaseOrderLine>();
                polines = _context.PurchaseOrderLine.Include(x => x.ItemType).Where(x => x.PurchaseOrderId.Equals(receiving.PurchaseOrderId)).ToList();
                foreach (var item in polines)
                {
                    ReceivingLine line = new ReceivingLine();
                    line.Receiving = receiving;
                    line.ItemType = item.ItemType;
                    line.QtyOrder = item.Quantity;
                    line.QtyReceive = item.Quantity;
                    line.QtyInventory = line.QtyReceive * 1;
                    line.BranchId = receiving.BranchId;
                    line.WarehouseId = receiving.WarehouseId;

                    _context.ReceivingLine.Add(line);
                    await _context.SaveChangesAsync();
                }

                TempData["TransMessage"] = "Create GSRN " + receiving.ReceivingNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = receiving.ReceivingId });
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receiving.BranchId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber", receiving.PurchaseOrderId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", receiving.VendorId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receiving.WarehouseId);
            return View(receiving);
        }

        // GET: Receiving/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiving = await _context.Receiving.SingleOrDefaultAsync(m => m.ReceivingId == id);
            if (receiving == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receiving.BranchId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber", receiving.PurchaseOrderId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", receiving.VendorId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receiving.WarehouseId);
            return View(receiving);
        }

        // POST: Receiving/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ReceivingId,PurchaseOrderId,ReceivingNumber,receivingDate,VendorId,VendorDONumber,VendorInvoice,BranchId,WarehouseId,HasChild,createdAt")] Receiving receiving)
        {
            if (id != receiving.ReceivingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiving);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceivingExists(receiving.ReceivingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit GSRN " + receiving.ReceivingNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receiving.BranchId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber", receiving.PurchaseOrderId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", receiving.VendorId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receiving.WarehouseId);
            return View(receiving);
        }

        // GET: Receiving/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiving = await _context.Receiving
                    .Include(r => r.Branch)
                    .Include(r => r.PurchaseOrder)
                    .Include(r => r.Vendor)
                    .Include(r => r.Warehouse)
                    .SingleOrDefaultAsync(m => m.ReceivingId == id);
            if (receiving == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receiving.BranchId);
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderNumber", receiving.PurchaseOrderId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", receiving.VendorId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receiving.WarehouseId);

            return View(receiving);
        }




        // POST: Receiving/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var receiving = await _context.Receiving
                .Include(x => x.ReceivingLine)
                .Include(x => x.PurchaseOrder)
                .SingleOrDefaultAsync(m => m.ReceivingId == id);
            try
            {
                _context.ReceivingLine.RemoveRange(receiving.ReceivingLine);
                _context.Receiving.Remove(receiving);

                //rollback status to open
                receiving.PurchaseOrder.PurchaseOrderStatus = PurchaseOrderStatus.Open;

                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete GSRN " + receiving.ReceivingNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(receiving);
            }
            
        }

        private bool ReceivingExists(string id)
        {
            return _context.Receiving.Any(e => e.ReceivingId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Receiving
        {
            public const string Controller = "Receiving";
            public const string Action = "Index";
            public const string Role = "Receiving";
            public const string Url = "/Receiving/Index";
            public const string Name = "Receiving";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Receiving")]
        public bool ReceivingRole { get; set; } = false;
    }
}



