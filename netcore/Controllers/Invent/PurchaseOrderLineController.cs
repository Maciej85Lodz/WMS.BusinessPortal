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


    [Authorize(Roles = "PurchaseOrderLine")]
    public class PurchaseOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrderLine
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.PurchaseOrderLine.Include(p => p.ItemType).Include(p => p.PurchaseOrder);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: PurchaseOrderLine/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseOrderLine = await _context.PurchaseOrderLine
                .Include(p => p.ItemType)
                .Include(p => p.PurchaseOrder)
                    .SingleOrDefaultAsync(m => m.purchaseOrderLineId == id);
        if (purchaseOrderLine == null)
        {
            return NotFound();
        }

        return View(purchaseOrderLine);
    }


    // GET: PurchaseOrderLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.PurchaseOrderLine.SingleOrDefault(m => m.purchaseOrderLineId == id);
        var selected = _context.PurchaseOrder.SingleOrDefault(m => m.PurchaseOrderId == masterid);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemCode");
            ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderId");
        if (check == null)
        {
            PurchaseOrderLine objline = new PurchaseOrderLine();
            objline.purchaseOrderLineId = string.Empty;
            objline.PurchaseOrder = selected;
            objline.PurchaseOrderId = masterid;
            return View(objline);
        }
        else
        {
            return View(check);
        }
    }




    // POST: PurchaseOrderLine/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("purchaseOrderLineId,PurchaseOrderId,ItemTypeId,Quantity,ItemPrice,DiscountAmount,TotalAmount,createdAt")] PurchaseOrderLine purchaseOrderLine)
    {
        if (ModelState.IsValid)
        {
            _context.Add(purchaseOrderLine);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", purchaseOrderLine.ItemTypeId);
                ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderLine.PurchaseOrderId);
        return View(purchaseOrderLine);
    }

    // GET: PurchaseOrderLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseOrderLine = await _context.PurchaseOrderLine.SingleOrDefaultAsync(m => m.purchaseOrderLineId == id);
        if (purchaseOrderLine == null)
        {
            return NotFound();
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", purchaseOrderLine.ItemTypeId);
                ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderLine.PurchaseOrderId);
        return View(purchaseOrderLine);
    }

    // POST: PurchaseOrderLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("purchaseOrderLineId,PurchaseOrderId,ItemTypeId,Quantity,ItemPrice,DiscountAmount,TotalAmount,createdAt")] PurchaseOrderLine purchaseOrderLine)
    {
        if (id != purchaseOrderLine.purchaseOrderLineId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(purchaseOrderLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderLineExists(purchaseOrderLine.purchaseOrderLineId))
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
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", purchaseOrderLine.ItemTypeId);
                ViewData["PurchaseOrderId"] = new SelectList(_context.PurchaseOrder, "PurchaseOrderId", "PurchaseOrderId", purchaseOrderLine.PurchaseOrderId);
        return View(purchaseOrderLine);
    }

    // GET: PurchaseOrderLine/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var purchaseOrderLine = await _context.PurchaseOrderLine
                .Include(p => p.ItemType)
                .Include(p => p.PurchaseOrder)
                .SingleOrDefaultAsync(m => m.purchaseOrderLineId == id);
        if (purchaseOrderLine == null)
        {
            return NotFound();
        }

        return View(purchaseOrderLine);
    }




    // POST: PurchaseOrderLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var purchaseOrderLine = await _context.PurchaseOrderLine.SingleOrDefaultAsync(m => m.purchaseOrderLineId == id);
            _context.PurchaseOrderLine.Remove(purchaseOrderLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PurchaseOrderLineExists(string id)
    {
        return _context.PurchaseOrderLine.Any(e => e.purchaseOrderLineId == id);
    }

  }
}





namespace WMS.MVC
{
  public static partial class Pages
  {
      public static class PurchaseOrderLine
      {
          public const string Controller = "PurchaseOrderLine";
          public const string Action = "Index";
          public const string Role = "PurchaseOrderLine";
          public const string Url = "/PurchaseOrderLine/Index";
          public const string Name = "PurchaseOrderLine";
      }
  }
}
namespace WMS.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "PurchaseOrderLine")]
      public bool PurchaseOrderLineRole { get; set; } = false;
  }
}



