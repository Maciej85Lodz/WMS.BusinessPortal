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


    [Authorize(Roles = "Warehouse")]
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Warehouse
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warehouse.OrderByDescending(x => x.CreatedAt).Include(x => x.Branch).ToListAsync());
        }

        // GET: Warehouse/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                        .Include(x => x.Branch)
                        .SingleOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchName", warehouse.BranchId);
            return View(warehouse);
        }


        // GET: Warehouse/Create
        public IActionResult Create()
        {
            Warehouse obj = new Warehouse();
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchName");
            return View(obj);
        }




        // POST: Warehouse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,WarehouseId,warehouseName,Description,street1,street2,city,province,country,createdAt")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create Warehouse " + warehouse.WarehouseName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouse/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse.Include(x => x.Branch).SingleOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchName", warehouse.BranchId);
            return View(warehouse);
        }

        // POST: Warehouse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BranchId,WarehouseId,WarehouseName,Description,street1,street2,city,province,country,createdAt")] Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.WarehouseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Warehouse " + warehouse.WarehouseName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: Warehouse/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouse
                    .Include(x => x.Branch)
                    .SingleOrDefaultAsync(m => m.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchName", warehouse.BranchId);
            return View(warehouse);
        }




        // POST: Warehouse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var warehouse = await _context.Warehouse.SingleOrDefaultAsync(m => m.WarehouseId == id);
            try
            {
                _context.Warehouse.Remove(warehouse);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Warehouse " + warehouse.WarehouseName + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(warehouse);
            }
            
        }

        private bool WarehouseExists(string id)
        {
            return _context.Warehouse.Any(e => e.WarehouseId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Warehouse
        {
            public const string Controller = "Warehouse";
            public const string Action = "Index";
            public const string Role = "Warehouse";
            public const string Url = "/Warehouse/Index";
            public const string Name = "Warehouse";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Warehouse")]
        public bool WarehouseRole { get; set; } = false;
    }
}



