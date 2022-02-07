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


    [Authorize(Roles = "ItemType")]
    public class ItemTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemTypes.OrderByDescending(x => x.createdAt).ToListAsync());
        }

        // GET: ItemType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                        .SingleOrDefaultAsync(m => m.ItemTypeId == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }


        // GET: ItemType/Create
        public IActionResult Create()
        {
            ItemType obj = new ItemType();
            return View(obj);
        }




        // POST: ItemType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemTypeId,ItemCode,ItemTypeName,Description,Barcode,ManufacturersNumber,ItemTypeType,UOM,createdAt")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemType);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create ItemType " + itemType.ItemTypeName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(itemType);
        }

        // GET: ItemType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.SingleOrDefaultAsync(m => m.ItemTypeId == id);
            if (itemType == null)
            {
                return NotFound();
            }
            return View(itemType);
        }

        // POST: ItemType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ItemTypeId,ItemCode,ItemTypeName,Description,Barcode,ManufacturersNumber,ItemTypeType,UOM,createdAt")] ItemType itemType)
        {
            if (id != itemType.ItemTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.ItemTypeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit ItemType " + itemType.ItemTypeName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(itemType);
        }

        // GET: ItemType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                    .SingleOrDefaultAsync(m => m.ItemTypeId == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }




        // POST: ItemType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var itemType = await _context.ItemTypes.SingleOrDefaultAsync(m => m.ItemTypeId == id);
            try
            {
                _context.ItemTypes.Remove(itemType);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete ItemType " + itemType.ItemTypeName + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(itemType);
            }
            
        }

        private bool ItemTypeExists(string id)
        {
            return _context.ItemTypes.Any(e => e.ItemTypeId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class ItemType
        {
            public const string Controller = "ItemType";
            public const string Action = "Index";
            public const string Role = "ItemType";
            public const string Url = "/ItemType/Index";
            public const string Name = "ItemType";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "ItemType")]
        public bool ItemTypeRole { get; set; } = false;
    }
}



