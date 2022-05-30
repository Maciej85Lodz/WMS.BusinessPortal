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


    [Authorize(Roles = "TransferOut")]
    public class TransferOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INetcoreService _netcoreService;

        public TransferOutController(ApplicationDbContext context, INetcoreService netcoreService)
        {
            _context = context;
            _netcoreService = netcoreService;
        }

        public async Task<IActionResult> ShowTransferOut(string id)
        {
            TransferOut obj = await _context.TransferOut
                .Include(x => x.TransferOrder)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferOutLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TransferOutId.Equals(id));
           

            return View(obj);
        }

        public async Task<IActionResult> PrintTransferOut(string id)
        {
            TransferOut obj = await _context.TransferOut
                .Include(x => x.TransferOrder)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferOutLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TransferOutId.Equals(id));

            return View(obj);
        }

        // GET: TransferOut
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransferOut.OrderByDescending(x => x.CreatedAt).Include(t => t.TransferOrder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransferOut/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOut = await _context.TransferOut
                    .Include(x => x.BranchFrom)
                    .Include(x => x.BranchTo)
                    .Include(x => x.WarehouseFrom)
                    .Include(x => x.WarehouseTo)
                    .Include(t => t.TransferOrder)
                        .SingleOrDefaultAsync(m => m.TransferOutId == id);
            if (transferOut == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOut.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOut);
        }


        // GET: TransferOut/Create
        public IActionResult Create()
        {
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder.Where(x => x.TransferOrderStatus == TransferOrderStatus.Open && x.IsIssued == false).ToList(), "transferOrderId", "transferOrderNumber");
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            TransferOut to = new TransferOut();
            return View(to);
        }




        // POST: TransferOut/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("transferOutId,transferOrderId,transferOutNumber,transferOutDate,Description,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferOut transferOut)
        {
            if (ModelState.IsValid)
            {

                //check transfer order
                TransferOut check = await _context.TransferOut
                    .Include(x => x.TransferOrder)
                    .SingleOrDefaultAsync(x => x.TransferOrderId.Equals(transferOut.TransferOrderId));
                if (check != null)
                {
                    ViewData["StatusMessage"] = "Error. Transfer order already issued. " + check.TransferOutNumber;

                    ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber");
                    ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
                    ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
                    

                    return View(transferOut);
                }

                //check stock
                bool isStockOK = true;
                string productList = "";
                List<TransferOrderLine> stocklines = new List<TransferOrderLine>();
                stocklines = _context.TransferOrderLine
                    .Include(x => x.TransferOrder)
                    .Include(x => x.ItemType)
                    .Where(x => x.TransferOrderId.Equals(transferOut.TransferOrderId)).ToList();
                foreach (var item in stocklines)
                {
                    VMStock stock = _netcoreService.GetStockByItemTypeAndWarehouse(item.ItemTypeId, item.TransferOrder.WarehouseIdFrom);
                    if (stock != null)
                    {
                        if (stock.QtyOnhand < item.Qty)
                        {
                            isStockOK = false;
                            productList = productList + " [" + item.ItemType.ItemCode + "] ";
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

                TransferOrder to = await _context.TransferOrder.Where(x => x.TransferOrderId.Equals(transferOut.TransferOrderId)).FirstOrDefaultAsync();
                transferOut.WarehouseIdFrom = to.WarehouseIdFrom;
                transferOut.WarehouseIdTo = to.WarehouseIdTo;


                transferOut.WarehouseFrom = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOut.WarehouseIdFrom));
                transferOut.BranchFrom = transferOut.WarehouseFrom.branch;
                transferOut.WarehouseTo = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOut.WarehouseIdTo));
                transferOut.BranchTo = transferOut.WarehouseTo.branch;

                to.IsIssued = true;

                _context.Add(transferOut);
                await _context.SaveChangesAsync();


                //auto create transfer out line, full shipment
                List<TransferOrderLine> lines = new List<TransferOrderLine>();
                lines = _context.TransferOrderLine.Include(x => x.ItemType).Where(x => x.TransferOrderId.Equals(transferOut.TransferOrderId)).ToList();
                foreach (var item in lines)
                {
                    TransferOutLine line = new TransferOutLine();
                    line.TransferOut = transferOut;
                    line.ItemType = item.ItemType;
                    line.Qty = item.Qty;
                    line.QtyInventory = line.Qty * -1;
                    

                    _context.TransferOutLine.Add(line);
                    await _context.SaveChangesAsync();
                }

                TempData["TransMessage"] = "Create Transfer " + transferOut.TransferOutNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = transferOut.TransferOutId });
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOut.TransferOrderId);
            return View(transferOut);
        }

        // GET: TransferOut/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOut = await _context.TransferOut
                .Include(x => x.BranchFrom)
                .Include(x => x.BranchTo)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .SingleOrDefaultAsync(m => m.TransferOutId == id);
            if (transferOut == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOut.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOut);
        }

        // POST: TransferOut/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("transferOutId,transferOrderId,transferOutNumber,transferOutDate,Description,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferOut transferOut)
        {
            if (id != transferOut.TransferOutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transferOut);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferOutExists(transferOut.TransferOutId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Transfer " + transferOut.TransferOutNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOut.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOut);
        }

        // GET: TransferOut/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOut = await _context.TransferOut
                    .Include(x => x.BranchFrom)
                    .Include(x => x.BranchTo)
                    .Include(x => x.WarehouseFrom)
                    .Include(x => x.WarehouseTo)
                    .Include(t => t.TransferOrder)
                    .SingleOrDefaultAsync(m => m.TransferOutId == id);
            if (transferOut == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOut.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOut);
        }




        // POST: TransferOut/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transferOut = await _context.TransferOut
                .Include(x => x.TransferOutLine)
                .Include(x => x.TransferOrder)
                .SingleOrDefaultAsync(m => m.TransferOutId == id);

            if (transferOut.TransferOrder.IsReceived == true)
            {
                ViewData["StatusMessage"] = "Error. Please delete Goods Receive first.";
                return View(transferOut);
            }

            try
            {
                _context.TransferOutLine.RemoveRange(transferOut.TransferOutLine);
                _context.TransferOut.Remove(transferOut);
                transferOut.TransferOrder.IsIssued = false;
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Transfer " + transferOut.TransferOutNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(transferOut);
            }
            
        }

        private bool TransferOutExists(string id)
        {
            return _context.TransferOut.Any(e => e.TransferOutId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class TransferOut
        {
            public const string Controller = "TransferOut";
            public const string Action = "Index";
            public const string Role = "TransferOut";
            public const string Url = "/TransferOut/Index";
            public const string Name = "TransferOut";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "TransferOut")]
        public bool TransferOutRole { get; set; } = false;
    }
}



