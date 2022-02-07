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


    [Authorize(Roles = "PurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowPurchaseOrder(string id)
        {
            PurchaseOrder obj = await _context.PurchaseOrder
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseOrderLine).ThenInclude(x => x.ItemType)
                .Include(x => x.Branch)
                .SingleOrDefaultAsync(x => x.PurchaseOrderId.Equals(id));

            obj.TotalOrderAmount = obj.PurchaseOrderLine.Sum(x => x.TotalAmount);
            obj.TotalDiscountAmount = obj.PurchaseOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(obj);

            return View(obj);
        }

        public async Task<IActionResult> PrintPurchaseOrder(string id)
        {
            PurchaseOrder obj = await _context.PurchaseOrder
                .Include(x => x.Vendor)
                .Include(x => x.PurchaseOrderLine).ThenInclude(x => x.ItemType)
                .Include(x => x.Branch)
                .SingleOrDefaultAsync(x => x.PurchaseOrderId.Equals(id));
            return View(obj);
        }

        // GET: PurchaseOrder
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PurchaseOrder.OrderByDescending(x => x.createdAt).Include(p => p.Vendor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                    .Include(x => x.PurchaseOrderLine)
                    .Include(p => p.Vendor)
                    .Include(x => x.Branch)
                        .SingleOrDefaultAsync(m => m.PurchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", purchaseOrder.BranchId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", purchaseOrder.VendorId);

            purchaseOrder.TotalOrderAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.TotalAmount);
            purchaseOrder.TotalDiscountAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(purchaseOrder);
            await _context.SaveChangesAsync();

            return View(purchaseOrder);
        }


        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            PurchaseOrder po = new PurchaseOrder();
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName");
            Branch defaultBranch = _context.Branch.Where(x => x.isDefaultBranch.Equals(true)).FirstOrDefault();
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", defaultBranch != null ? defaultBranch.branchId : null);
            return View(po);
        }




        // POST: PurchaseOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseOrderId,PurchaseOrderNumber,Top,PurchaseOrderDate,DeliveryDate,DeliveryAddress,ReferenceNumberInternal,ReferenceNumberExternal,Description,VendorId,BranchId,PicInternal,picVendor,PurchaseOrderStatus,TotalDiscountAmount,TotalOrderAmount,PurchaseReceiveNumber,HasChild,createdAt")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create PO " + purchaseOrder.PurchaseOrderNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = purchaseOrder.PurchaseOrderId });
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", purchaseOrder.BranchId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", purchaseOrder.VendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder.Include(x => x.PurchaseOrderLine).SingleOrDefaultAsync(m => m.PurchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            purchaseOrder.TotalOrderAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.TotalAmount);
            purchaseOrder.TotalDiscountAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(purchaseOrder);
            await _context.SaveChangesAsync();
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", purchaseOrder.BranchId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", purchaseOrder.VendorId);
            TempData["PurchaseOrderStatus"] = purchaseOrder.PurchaseOrderStatus;
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PurchaseOrderId,PurchaseOrderNumber,Top,PurchaseOrderDate,DeliveryDate,DeliveryAddress,ReferenceNumberInternal,ReferenceNumberExternal,Description,VendorId,BranchId,PicInternal,picVendor,PurchaseOrderStatus,TotalDiscountAmount,TotalOrderAmount,PurchaseReceiveNumber,HasChild,createdAt")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return NotFound();
            }
            

            if ((PurchaseOrderStatus)TempData["PurchaseOrderStatus"] == PurchaseOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit [Completed] order.";
                return RedirectToAction(nameof(Edit), new { id = purchaseOrder.PurchaseOrderId });
            }

            if (purchaseOrder.PurchaseOrderStatus == PurchaseOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit status to [Completed].";
                return RedirectToAction(nameof(Edit), new { id = purchaseOrder.PurchaseOrderId });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.PurchaseOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit PO " + purchaseOrder.PurchaseOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", purchaseOrder.BranchId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", purchaseOrder.VendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                    .Include(x => x.PurchaseOrderLine)
                    .Include(p => p.Vendor)
                    .SingleOrDefaultAsync(m => m.PurchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", purchaseOrder.BranchId);
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "vendorName", purchaseOrder.VendorId);

            purchaseOrder.TotalOrderAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.TotalAmount);
            purchaseOrder.TotalDiscountAmount = purchaseOrder.PurchaseOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(purchaseOrder);
            await _context.SaveChangesAsync();

            return View(purchaseOrder);
        }




        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var purchaseOrder = await _context.PurchaseOrder
                .Include(x => x.PurchaseOrderLine)
                .SingleOrDefaultAsync(m => m.PurchaseOrderId == id);
            try
            {
                _context.PurchaseOrderLine.RemoveRange(purchaseOrder.PurchaseOrderLine);
                _context.PurchaseOrder.Remove(purchaseOrder);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete PO " + purchaseOrder.PurchaseOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(purchaseOrder);
            }
            
        }

        private bool PurchaseOrderExists(string id)
        {
            return _context.PurchaseOrder.Any(e => e.PurchaseOrderId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class PurchaseOrder
        {
            public const string Controller = "PurchaseOrder";
            public const string Action = "Index";
            public const string Role = "PurchaseOrder";
            public const string Url = "/PurchaseOrder/Index";
            public const string Name = "PurchaseOrder";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "PurchaseOrder")]
        public bool PurchaseOrderRole { get; set; } = false;
    }
}



