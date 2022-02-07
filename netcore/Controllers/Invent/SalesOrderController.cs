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


    [Authorize(Roles = "SalesOrder")]
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ShowSalesOrder(string id)
        {
            SalesOrder obj = await _context.SalesOrder
                .Include(x => x.Customer)
                .Include(x => x.SalesOrderLine).ThenInclude(x => x.ItemType)
                .Include(x => x.Branch)
                .SingleOrDefaultAsync(x => x.SalesOrderId.Equals(id));

            obj.TotalOrderAmount = obj.SalesOrderLine.Sum(x => x.TotalAmount);
            obj.TotalDiscountAmount = obj.SalesOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(obj);

            return View(obj);
        }

        public async Task<IActionResult> PrintSalesOrder(string id)
        {
            SalesOrder obj = await _context.SalesOrder
                .Include(x => x.Customer)
                .Include(x => x.SalesOrderLine).ThenInclude(x => x.ItemType)
                .Include(x => x.Branch)
                .SingleOrDefaultAsync(x => x.SalesOrderId.Equals(id));
            return View(obj);
        }

        // GET: SalesOrder
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SalesOrder.OrderByDescending(x => x.createdAt).Include(s => s.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                    .Include(x => x.SalesOrderLine)
                    .Include(s => s.Customer)
                    .Include(x => x.Branch)
                        .SingleOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", salesOrder.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", salesOrder.CustomerId);

            salesOrder.TotalOrderAmount = salesOrder.SalesOrderLine.Sum(x => x.TotalAmount);
            salesOrder.TotalDiscountAmount = salesOrder.SalesOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(salesOrder);
            await _context.SaveChangesAsync();

            return View(salesOrder);
        }


        // GET: SalesOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName");
            Branch defaultBranch = _context.Branch.Where(x => x.isDefaultBranch.Equals(true)).FirstOrDefault();
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", defaultBranch != null ? defaultBranch.branchId : null);
            SalesOrder so = new SalesOrder();
            return View(so);
        }


        // POST: SalesOrder/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderId,SalesOrderNumber,Top,SalesOrderDate,DeliveryDate,DeliveryAddress,ReferenceNumberInternal,ReferenceNumberExternal,Description,CustomerId,BranchId,PicInternal,PicCustomer,SalesOrderStatus,TotalDiscountAmount,TotalOrderAmount,salesShipmentNumber,HasChild,createdAt")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create SO " + salesOrder.SalesOrderNumber + " Success";
                return RedirectToAction(nameof(Details), new { id = salesOrder.SalesOrderId });
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", salesOrder.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder.Include(x => x.SalesOrderLine).SingleOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            salesOrder.TotalOrderAmount = salesOrder.SalesOrderLine.Sum(x => x.TotalAmount);
            salesOrder.TotalDiscountAmount = salesOrder.SalesOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(salesOrder);
            await _context.SaveChangesAsync();
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", salesOrder.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", salesOrder.CustomerId);
            TempData["SalesOrderStatus"] = salesOrder.SalesOrderStatus;
            ViewData["StatusMessage"] = TempData["StatusMessage"];
            return View(salesOrder);
        }

        // POST: SalesOrder/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SalesOrderId,SalesOrderNumber,Top,SalesOrderDate,DeliveryDate,DeliveryAddress,ReferenceNumberInternal,ReferenceNumberExternal,Description,CustomerId,BranchId,PicInternal,PicCustomer,SalesOrderStatus,TotalDiscountAmount,TotalOrderAmount,salesShipmentNumber,HasChild,createdAt")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderId)
            {
                return NotFound();
            }

            if ((SalesOrderStatus)TempData["SalesOrderStatus"] == SalesOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit [Completed] order.";
                return RedirectToAction(nameof(Edit), new { id = salesOrder.SalesOrderId});
            }

            if (salesOrder.SalesOrderStatus== SalesOrderStatus.Completed)
            {
                TempData["StatusMessage"] = "Error. Can not edit status to [Completed].";
                return RedirectToAction(nameof(Edit), new { id = salesOrder.SalesOrderId});
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.SalesOrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit SO " + salesOrder.SalesOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", salesOrder.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                    .Include(x => x.SalesOrderLine)
                    .Include(s => s.Customer)
                    .SingleOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", salesOrder.BranchId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "customerName", salesOrder.CustomerId);

            salesOrder.TotalOrderAmount = salesOrder.SalesOrderLine.Sum(x => x.TotalAmount);
            salesOrder.TotalDiscountAmount = salesOrder.SalesOrderLine.Sum(x => x.DiscountAmount);
            _context.Update(salesOrder);
            await _context.SaveChangesAsync();

            return View(salesOrder);
        }




        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var salesOrder = await _context.SalesOrder
                .Include(x => x.SalesOrderLine)
                .SingleOrDefaultAsync(m => m.SalesOrderId == id);
            try
            {
                _context.SalesOrderLine.RemoveRange(salesOrder.SalesOrderLine);
                _context.SalesOrder.Remove(salesOrder);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete SO " + salesOrder.SalesOrderNumber + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(salesOrder);
            }
            
        }

        private bool SalesOrderExists(string id)
        {
            return _context.SalesOrder.Any(e => e.SalesOrderId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class SalesOrder
        {
            public const string Controller = "SalesOrder";
            public const string Action = "Index";
            public const string Role = "SalesOrder";
            public const string Url = "/SalesOrder/Index";
            public const string Name = "SalesOrder";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "SalesOrder")]
        public bool SalesOrderRole { get; set; } = false;
    }
}



