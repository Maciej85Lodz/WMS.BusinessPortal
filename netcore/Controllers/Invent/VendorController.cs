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


    [Authorize(Roles = "Vendor")]
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendor
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vendor.OrderByDescending(x => x.CreatedAt).ToListAsync());
        }

        // GET: Vendor/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                        .SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }


        // GET: Vendor/Create
        public IActionResult Create()
        {
            Vendor obj = new Vendor();
            return View(obj);
        }




        // POST: Vendor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,vendorName,Description,size,street1,street2,city,province,country,HasChild,createdAt")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create Vendor " + vendor.VendorName + " Success";
                return RedirectToAction(nameof(Details), new { id = vendor.VendorId });
            }
            return View(vendor);
        }

        // GET: Vendor/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor.SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendor == null)
            {
                return NotFound();
            }
            return View(vendor);
        }

        // POST: Vendor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VendorId,vendorName,Description,size,street1,street2,city,province,country,HasChild,createdAt")] Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.VendorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Vendor " + vendor.VendorName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendor/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendor
                    .SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendor == null)
            {
                return NotFound();
            }

            return View(vendor);
        }




        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vendor = await _context.Vendor.Include(x => x.VendorLine).SingleOrDefaultAsync(m => m.VendorId == id);
            try
            {
                _context.VendorLine.RemoveRange(vendor.VendorLine);
                _context.Vendor.Remove(vendor);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Vendor " + vendor.VendorName + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(vendor);
            }
            
        }

        private bool VendorExists(string id)
        {
            return _context.Vendor.Any(e => e.VendorId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Vendor
        {
            public const string Controller = "Vendor";
            public const string Action = "Index";
            public const string Role = "Vendor";
            public const string Url = "/Vendor/Index";
            public const string Name = "Vendor";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Vendor")]
        public bool VendorRole { get; set; } = false;
    }
}



