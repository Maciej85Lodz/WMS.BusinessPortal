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


    [Authorize(Roles = "TransferOrderLine")]
    public class TransferOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransferOrderLine
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransferOrderLine.Include(t => t.ItemType).Include(t => t.TransferOrder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransferOrderLine/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrderLine = await _context.TransferOrderLine
                    .Include(t => t.ItemType)
                    .Include(t => t.TransferOrder)
                        .SingleOrDefaultAsync(m => m.TransferOrderLineId == id);
            if (transferOrderLine == null)
            {
                return NotFound();
            }

            return View(transferOrderLine);
        }


        // GET: TransferOrderLine/Create
        public IActionResult Create(string masterid, string id)
        {
            var check = _context.TransferOrderLine.SingleOrDefault(m => m.TransferOrderLineId == id);
            var selected = _context.TransferOrder.SingleOrDefault(m => m.TransferOrderId == masterid);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode");
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber");
            if (check == null)
            {
                TransferOrderLine objline = new TransferOrderLine();
                objline.TransferOrderLineId = string.Empty;
                objline.TransferOrder = selected;
                objline.TransferOrderId = masterid;
                return View(objline);
            }
            else
            {
                return View(check);
            }
        }




        // POST: TransferOrderLine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("transferOrderLineId,transferOrderId,ItemTypeId,Quantity,createdAt")] TransferOrderLine transferOrderLine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transferOrderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", transferOrderLine.ItemTypeId);
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOrderLine.TransferOrderId);
            return View(transferOrderLine);
        }

        // GET: TransferOrderLine/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrderLine = await _context.TransferOrderLine.SingleOrDefaultAsync(m => m.TransferOrderLineId == id);
            if (transferOrderLine == null)
            {
                return NotFound();
            }
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", transferOrderLine.ItemTypeId);
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOrderLine.TransferOrderId);
            return View(transferOrderLine);
        }

        // POST: TransferOrderLine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("transferOrderLineId,transferOrderId,ItemTypeId,Quantity,createdAt")] TransferOrderLine transferOrderLine)
        {
            if (id != transferOrderLine.TransferOrderLineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transferOrderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferOrderLineExists(transferOrderLine.TransferOrderLineId))
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
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode", transferOrderLine.ItemTypeId);
            ViewData["transferOrderId"] = new SelectList(_context.TransferOrder, "transferOrderId", "transferOrderNumber", transferOrderLine.TransferOrderId);
            return View(transferOrderLine);
        }

        // GET: TransferOrderLine/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferOrderLine = await _context.TransferOrderLine
                    .Include(t => t.ItemType)
                    .Include(t => t.TransferOrder)
                    .SingleOrDefaultAsync(m => m.TransferOrderLineId == id);
            if (transferOrderLine == null)
            {
                return NotFound();
            }

            return View(transferOrderLine);
        }




        // POST: TransferOrderLine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transferOrderLine = await _context.TransferOrderLine.SingleOrDefaultAsync(m => m.TransferOrderLineId == id);
            _context.TransferOrderLine.Remove(transferOrderLine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferOrderLineExists(string id)
        {
            return _context.TransferOrderLine.Any(e => e.TransferOrderLineId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class TransferOrderLine
        {
            public const string Controller = "TransferOrderLine";
            public const string Action = "Index";
            public const string Role = "TransferOrderLine";
            public const string Url = "/TransferOrderLine/Index";
            public const string Name = "TransferOrderLine";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "TransferOrderLine")]
        public bool TransferOrderLineRole { get; set; } = false;
    }
}



