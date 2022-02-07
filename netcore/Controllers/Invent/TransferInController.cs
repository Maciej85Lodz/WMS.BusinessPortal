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


    [Authorize(Roles = "TransferIn")]
    public class TransferInController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferInController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowTransferIn(string id)
        {
            TransferIn obj = await _context.TransferIn
                .Include(x => x.TransferOrder)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferInLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TansferInId.Equals(id));


            return View(obj);
        }

        public async Task<IActionResult> PrintTransferIn(string id)
        {
            TransferIn obj = await _context.TransferIn
                .Include(x => x.TransferOrder)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferInLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TansferInId.Equals(id));

            return View(obj);
        }

        // GET: TransferIn
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransferIn.OrderByDescending(x => x.createdAt).Include(t => t.TransferOrder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransferIn/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferIn = await _context.TransferIn
                    .Include(x => x.BranchFrom)
                    .Include(x => x.BranchTo)
                    .Include(x => x.WarehouseFrom)
                    .Include(x => x.WarehouseTo)
                    .Include(t => t.TransferOrder)
                        .SingleOrDefaultAsync(m => m.TansferInId == id);
            if (transferIn == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferIn.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferIn);
        }


        // GET: TransferIn/Create
        public IActionResult Create()
        {
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder.Where(x => x.TransferOrderStatus == TransferOrderStatus.Open && x.IsIssued == true).ToList(), "transferOrderId", "transferOrderNumber");
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            TransferIn obj = new TransferIn();
            return View(obj);
        }




        // POST: TransferIn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("transferInId,transferOrderId,transferInNumber,transferInDate,Description,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferIn transferIn)
        {
            if (ModelState.IsValid)
            {

                //check transfer order
                TransferIn check = await _context.TransferIn.SingleOrDefaultAsync(x => x.TransferOrderId.Equals(transferIn.TransferOrderId));
                if (check != null)
                {
                    ViewData["StatusMessage"] = "Error. Transfer order already received. " + check.TransferInNumber;

                    ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber");
                    ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
                    ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
                    ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");


                    return View(transferIn);
                }

                TransferOrder to = await _context.TransferOrder.Where(x => x.TransferOrderId.Equals(transferIn.TransferOrderId)).FirstOrDefaultAsync();
                transferIn.WarehouseIdFrom = to.WarehouseIdFrom;
                transferIn.WarehouseIdTo = to.WarehouseIdTo;

                transferIn.WarehouseFrom = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferIn.WarehouseIdFrom));
                transferIn.BranchFrom = transferIn.WarehouseFrom.branch;
                transferIn.WarehouseTo = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferIn.WarehouseIdTo));
                transferIn.BranchTo = transferIn.WarehouseTo.branch;


                to.IsReceived = true;
                to.TransferOrderStatus = TransferOrderStatus.Completed;

                _context.Add(transferIn);
                await _context.SaveChangesAsync();

                //auto create transfer in line, full shipment
                List<TransferOrderLine> lines = new List<TransferOrderLine>();
                lines = _context.TransferOrderLine.Include(x => x.ItemType).Where(x => x.TransferOrderId.Equals(transferIn.TransferOrderId)).ToList();
                foreach (var item in lines)
                {
                    TransferInLine line = new TransferInLine();
                    line.TransferIn = transferIn;
                    line.ItemType = item.ItemType;
                    line.Qty = item.Qty;
                    line.QtyInventory = line.Qty * 1;

                    _context.TransferInLine.Add(line);
                    await _context.SaveChangesAsync();
                }

                TempData["TransMessage"] = "Create Transfer " + transferIn.TransferInNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = transferIn.TansferInId });
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferIn.TransferOrderId);
            return View(transferIn);
        }

        // GET: TransferIn/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferIn = await _context.TransferIn
                .Include(x => x.BranchFrom)
                .Include(x => x.BranchTo)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .SingleOrDefaultAsync(m => m.TansferInId == id);
            if (transferIn == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferIn.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferIn);
        }

        // POST: TransferIn/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("transferInId,transferOrderId,transferInNumber,transferInDate,Description,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferIn transferIn)
        {
            if (id != transferIn.TansferInId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transferIn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferInExists(transferIn.TansferInId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Transfer " + transferIn.TransferInNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferIn.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferIn);
        }

        // GET: TransferIn/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferIn = await _context.TransferIn
                    .Include(x => x.BranchFrom)
                    .Include(x => x.BranchTo)
                    .Include(x => x.WarehouseFrom)
                    .Include(x => x.WarehouseTo)
                    .Include(t => t.TransferOrder)
                    .SingleOrDefaultAsync(m => m.TansferInId == id);
            if (transferIn == null)
            {
                return NotFound();
            }
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferIn.TransferOrderId);
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferIn);
        }




        // POST: TransferIn/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transferIn = await _context.TransferIn
                .Include(x => x.TransferInLine)
                .Include(x => x.TransferOrder)
                .SingleOrDefaultAsync(m => m.TansferInId == id);
            try
            {
                _context.TransferInLine.RemoveRange(transferIn.TransferInLine);
                _context.TransferIn.Remove(transferIn);
                transferIn.TransferOrder.TransferOrderStatus = TransferOrderStatus.Open;
                transferIn.TransferOrder.IsReceived = false;
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Transfer " + transferIn.TransferInNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(transferIn);
            }
            
        }

        private bool TransferInExists(string id)
        {
            return _context.TransferIn.Any(e => e.TansferInId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class TransferIn
        {
            public const string Controller = "TransferIn";
            public const string Action = "Index";
            public const string Role = "TransferIn";
            public const string Url = "/TransferIn/Index";
            public const string Name = "TransferIn";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "TransferIn")]
        public bool TransferInRole { get; set; } = false;
    }
}



