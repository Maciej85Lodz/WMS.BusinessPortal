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


    [Authorize(Roles = "ReceivingLine")]
    public class ReceivingLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReceivingLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReceivingLine
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReceivingLine.Include(r => r.Branch).Include(r => r.ItemType).Include(r => r.Receiving).Include(r => r.Warehouse);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ReceivingLine/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receivingLine = await _context.ReceivingLine
                    .Include(r => r.Branch)
                    .Include(r => r.ItemType)
                    .Include(r => r.Receiving)
                    .Include(r => r.Warehouse)
                        .SingleOrDefaultAsync(m => m.ReceivingLineId == id);
            if (receivingLine == null)
            {
                return NotFound();
            }

            return View(receivingLine);
        }


        // GET: ReceivingLine/Create
        public IActionResult Create(string masterid, string id)
        {
            var check = _context.ReceivingLine.SingleOrDefault(m => m.ReceivingLineId == id);
            var selected = _context.Receiving.SingleOrDefault(m => m.ReceivingId == masterid);
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode");
            ViewData["ReceivingId"] = new SelectList(_context.Receiving, "ReceivingId", "ReceivingNumber");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName");
            if (check == null)
            {
                ReceivingLine objline = new ReceivingLine();
                objline.ReceivingLineId = string.Empty;
                objline.Receiving = selected;
                objline.ReceivingId = masterid;
                return View(objline);
            }
            else
            {
                return View(check);
            }
        }




        // POST: ReceivingLine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceivingLineId,ReceivingId,BranchId,WarehouseId,ItemTypeId,Quantity,QtyReceive,QtyInventory,createdAt")] ReceivingLine receivingLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receivingLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receivingLine.BranchId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", receivingLine.ItemTypeId);
            ViewData["ReceivingId"] = new SelectList(_context.Receiving, "ReceivingId", "ReceivingNumber", receivingLine.ReceivingId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receivingLine.WarehouseId);
            return View(receivingLine);
        }

        // GET: ReceivingLine/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receivingLine = await _context.ReceivingLine.SingleOrDefaultAsync(m => m.ReceivingLineId == id);
            if (receivingLine == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receivingLine.BranchId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", receivingLine.ItemTypeId);
            ViewData["ReceivingId"] = new SelectList(_context.Receiving, "ReceivingId", "ReceivingNumber", receivingLine.ReceivingId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receivingLine.WarehouseId);
            return View(receivingLine);
        }

        // POST: ReceivingLine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ReceivingLineId,ReceivingId,BranchId,WarehouseId,ItemTypeId,Quantity,QtyReceive,QtyInventory,createdAt")] ReceivingLine receivingLine)
        {
            if (id != receivingLine.ReceivingLineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receivingLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceivingLineExists(receivingLine.ReceivingLineId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "branchName", receivingLine.BranchId);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", receivingLine.ItemTypeId);
            ViewData["ReceivingId"] = new SelectList(_context.Receiving, "ReceivingId", "ReceivingNumber", receivingLine.ReceivingId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "warehouseName", receivingLine.WarehouseId);
            return View(receivingLine);
        }

        // GET: ReceivingLine/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receivingLine = await _context.ReceivingLine
                    .Include(r => r.Branch)
                    .Include(r => r.ItemType)
                    .Include(r => r.Receiving)
                    .Include(r => r.Warehouse)
                    .SingleOrDefaultAsync(m => m.ReceivingLineId == id);
            if (receivingLine == null)
            {
                return NotFound();
            }

            return View(receivingLine);
        }




        // POST: ReceivingLine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var receivingLine = await _context.ReceivingLine.SingleOrDefaultAsync(m => m.ReceivingLineId == id);
            _context.ReceivingLine.Remove(receivingLine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceivingLineExists(string id)
        {
            return _context.ReceivingLine.Any(e => e.ReceivingLineId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class ReceivingLine
        {
            public const string Controller = "ReceivingLine";
            public const string Action = "Index";
            public const string Role = "ReceivingLine";
            public const string Url = "/ReceivingLine/Index";
            public const string Name = "ReceivingLine";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "ReceivingLine")]
        public bool ReceivingLineRole { get; set; } = false;
    }
}



