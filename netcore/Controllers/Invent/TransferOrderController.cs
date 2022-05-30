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


    [Authorize(Roles = "TransferOrder")]
    public class TransferOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowTransferOrder(string id)
        {
            TransferOrder obj = await _context.TransferOrder
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferOrderLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TransferOrderId.Equals(id));
            

            return View(obj);
        }

        public async Task<IActionResult> PrintTransferOrder(string id)
        {
            TransferOrder obj = await _context.TransferOrder
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .Include(x => x.TransferOrderLine).ThenInclude(x => x.ItemType)
                .SingleOrDefaultAsync(x => x.TransferOrderId.Equals(id));
            return View(obj);
        }

        // GET: TransferOrder
        public async Task<IActionResult> Index()
        {
            List<TransferOrder> lists = new List<TransferOrder>();
            lists = await _context.TransferOrder.OrderByDescending(x => x.CreatedAt)
                .Include(x => x.BranchFrom)
                .Include(x => x.BranchTo)
                .Include(x => x.WarehouseFrom)
                .Include(x => x.WarehouseTo)
                .ToListAsync();
            return View(lists);
        }

        // GET: TransferOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrder = await _context.TransferOrder
                        .Include(x => x.BranchFrom)
                        .Include(x => x.BranchTo)
                        .Include(x => x.WarehouseFrom)
                        .Include(x => x.WarehouseTo)
                        .SingleOrDefaultAsync(m => m.TransferOrderId == id);
            if (transferOrder == null)
            {
                return NotFound();
            }
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOrder);
        }


        // GET: TransferOrder/Create
        public IActionResult Create()
        {
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            TransferOrder obj = new TransferOrder();
            return View(obj);
        }




        // POST: TransferOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("transferOrderId,transferOrderStatus,transferOrderNumber,transferOrderDate,Description,picName,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferOrder transferOrder)
        {
            if (transferOrder.WarehouseIdFrom == transferOrder.WarehouseIdTo)
            {
                TempData["StatusMessage"] = "Error. Warehouse from and to are the same. Transfer order only working if from and to Warehouse are different";
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                transferOrder.WarehouseFrom = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOrder.WarehouseIdFrom));
                transferOrder.BranchFrom = transferOrder.WarehouseFrom.branch;

                transferOrder.WarehouseTo = await _context.Warehouse.SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOrder.WarehouseIdTo));
                transferOrder.BranchTo = transferOrder.WarehouseTo.branch;
                

                _context.Add(transferOrder);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create Transfer " + transferOrder.TransferOrderNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = transferOrder.TransferOrderId });
            }
            return View(transferOrder);
        }

        // GET: TransferOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrder = await _context.TransferOrder.SingleOrDefaultAsync(m => m.TransferOrderId == id);
            if (transferOrder == null)
            {
                return NotFound();
            }
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            TempData["TransferOrderStatus"] = transferOrder.TransferOrderStatus;
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            return View(transferOrder);
        }

        // POST: TransferOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("transferOrderId,isIssued,isReceived,transferOrderStatus,transferOrderNumber,transferOrderDate,Description,picName,branchIdFrom,warehouseIdFrom,branchIdTo,warehouseIdTo,HasChild,createdAt")] TransferOrder transferOrder)
        {
            if (id != transferOrder.TransferOrderId)
            {
                return NotFound();
            }

            if ((TransferOrderStatus)TempData["TransferOrderStatus"] == TransferOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit [Completed] order.";
                return RedirectToAction(nameof(Edit), new { id = transferOrder.TransferOrderId });
            }

            if (transferOrder.TransferOrderStatus == TransferOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit status to [Completed].";
                return RedirectToAction(nameof(Edit), new { id = transferOrder.TransferOrderId });
            }

            if (transferOrder.IsIssued == true
                || transferOrder.IsReceived == true)
            {
                TempData["StatusMessage"] = "Error. Can not edit [Open] order that already process the goods issue or goods receive.";
                return RedirectToAction(nameof(Edit), new { id = transferOrder.TransferOrderId });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transferOrder.WarehouseFrom = await _context.Warehouse.Include(x => x.branch).SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOrder.WarehouseIdFrom));
                    transferOrder.BranchFrom = transferOrder.WarehouseFrom.branch;

                    transferOrder.WarehouseTo = await _context.Warehouse.SingleOrDefaultAsync(x => x.warehouseId.Equals(transferOrder.WarehouseIdTo));
                    transferOrder.BranchTo = transferOrder.WarehouseTo.branch;

                    _context.Update(transferOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferOrderExists(transferOrder.TransferOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["TransMessage"] = "Edit Transfer " + transferOrder.TransferOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(transferOrder);
        }

        // GET: TransferOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrder = await _context.TransferOrder
                    .Include(x => x.BranchFrom)
                    .Include(x => x.BranchTo)
                    .Include(x => x.WarehouseFrom)
                    .Include(x => x.WarehouseTo)
                    .SingleOrDefaultAsync(m => m.TransferOrderId == id);
            if (transferOrder == null)
            {
                return NotFound();
            }
            ViewData["branchIdFrom"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdFrom"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            ViewData["branchIdTo"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["warehouseIdTo"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            return View(transferOrder);
        }




        // POST: TransferOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transferOrder = await _context.TransferOrder
                .Include(x => x.TransferOrderLine)
                .SingleOrDefaultAsync(m => m.TransferOrderId == id);
            try
            {
                _context.TransferOrderLine.RemoveRange(transferOrder.TransferOrderLine);
                _context.TransferOrder.Remove(transferOrder);
                await _context.SaveChangesAsync();

                TempData["TransMessage"] = "Delete Transfer " + transferOrder.TransferOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(transferOrder);
            }
            
        }

        private bool TransferOrderExists(string id)
        {
            return _context.TransferOrder.Any(e => e.TransferOrderId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class TransferOrder
        {
            public const string Controller = "TransferOrder";
            public const string Action = "Index";
            public const string Role = "TransferOrder";
            public const string Url = "/TransferOrder/Index";
            public const string Name = "TransferOrder";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "TransferOrder")]
        public bool TransferOrderRole { get; set; } = false;
    }
}



