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


    [Authorize(Roles = "TransferInLine")]
    public class TransferInLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferInLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransferInLine
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.TransferInLine.Include(t => t.ItemType).Include(t => t.TransferIn);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: TransferInLine/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferInLine = await _context.TransferInLine
                .Include(t => t.ItemType)
                .Include(t => t.TransferIn)
                    .SingleOrDefaultAsync(m => m.TransferInLineId == id);
        if (transferInLine == null)
        {
            return NotFound();
        }

        return View(transferInLine);
    }


    // GET: TransferInLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.TransferInLine.SingleOrDefault(m => m.TransferInLineId == id);
        var selected = _context.TransferIn.SingleOrDefault(m => m.TansferInId == masterid);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId");
            ViewData["transferInId"] = new SelectList(_context.TransferIn, "transferInId", "transferInId");
        if (check == null)
        {
            TransferInLine objline = new TransferInLine();
            objline.TransferInLineId = string.Empty;
            objline.TransferIn = selected;
            objline.TransferInId = masterid;
            return View(objline);
        }
        else
        {
            return View(check);
        }
    }




    // POST: TransferInLine/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("transferInLineId,transferInId,ItemTypeId,Quantity,QtyInventory,createdAt")] TransferInLine transferInLine)
    {
        if (ModelState.IsValid)
        {
            _context.Add(transferInLine);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferInLine.ItemTypeId);
                ViewData["transferInId"] = new SelectList(_context.TransferIn, "transferInId", "transferInId", transferInLine.TransferInId);
        return View(transferInLine);
    }

    // GET: TransferInLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferInLine = await _context.TransferInLine.SingleOrDefaultAsync(m => m.TransferInLineId == id);
        if (transferInLine == null)
        {
            return NotFound();
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferInLine.ItemTypeId);
                ViewData["transferInId"] = new SelectList(_context.TransferIn, "transferInId", "transferInId", transferInLine.TransferInId);
        return View(transferInLine);
    }

    // POST: TransferInLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("transferInLineId,transferInId,ItemTypeId,Quantity,QtyInventory,createdAt")] TransferInLine transferInLine)
    {
        if (id != transferInLine.TransferInLineId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transferInLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferInLineExists(transferInLine.TransferInLineId))
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
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferInLine.ItemTypeId);
                ViewData["transferInId"] = new SelectList(_context.TransferIn, "transferInId", "transferInId", transferInLine.TransferInId);
        return View(transferInLine);
    }

    // GET: TransferInLine/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferInLine = await _context.TransferInLine
                .Include(t => t.ItemType)
                .Include(t => t.TransferIn)
                .SingleOrDefaultAsync(m => m.TransferInLineId == id);
        if (transferInLine == null)
        {
            return NotFound();
        }

        return View(transferInLine);
    }




    // POST: TransferInLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var transferInLine = await _context.TransferInLine.SingleOrDefaultAsync(m => m.TransferInLineId == id);
            _context.TransferInLine.Remove(transferInLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransferInLineExists(string id)
    {
        return _context.TransferInLine.Any(e => e.TransferInLineId == id);
    }

  }
}





namespace WMS.MVC
{
  public static partial class Pages
  {
      public static class TransferInLine
      {
          public const string Controller = "TransferInLine";
          public const string Action = "Index";
          public const string Role = "TransferInLine";
          public const string Url = "/TransferInLine/Index";
          public const string Name = "TransferInLine";
      }
  }
}
namespace WMS.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "TransferInLine")]
      public bool TransferInLineRole { get; set; } = false;
  }
}



