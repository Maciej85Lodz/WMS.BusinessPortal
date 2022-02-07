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


    [Authorize(Roles = "TransferOutLine")]
    public class TransferOutLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferOutLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransferOutLine
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.TransferOutLine.Include(t => t.ItemType).Include(t => t.TransferOut);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: TransferOutLine/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferOutLine = await _context.TransferOutLine
                .Include(t => t.ItemType)
                .Include(t => t.TransferOut)
                    .SingleOrDefaultAsync(m => m.TransferOutLineId == id);
        if (transferOutLine == null)
        {
            return NotFound();
        }

        return View(transferOutLine);
    }


    // GET: TransferOutLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.TransferOutLine.SingleOrDefault(m => m.TransferOutLineId == id);
        var selected = _context.TransferOut.SingleOrDefault(m => m.TransferOutId == masterid);
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId");
            ViewData["transferOutId"] = new SelectList(_context.TransferOut, "transferOutId", "transferOutId");
        if (check == null)
        {
            TransferOutLine objline = new TransferOutLine();
            objline.TransferOutLineId = string.Empty;
            objline.TransferOut = selected;
            objline.TransferOutId = masterid;
            return View(objline);
        }
        else
        {
            return View(check);
        }
    }




    // POST: TransferOutLine/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("transferOutLineId,transferOutId,ItemTypeId,Quantity,QtyInventory,createdAt")] TransferOutLine transferOutLine)
    {
        if (ModelState.IsValid)
        {
            _context.Add(transferOutLine);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferOutLine.ItemTypeId);
                ViewData["transferOutId"] = new SelectList(_context.TransferOut, "transferOutId", "transferOutId", transferOutLine.TransferOutId);
        return View(transferOutLine);
    }

    // GET: TransferOutLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferOutLine = await _context.TransferOutLine.SingleOrDefaultAsync(m => m.TransferOutLineId == id);
        if (transferOutLine == null)
        {
            return NotFound();
        }
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferOutLine.ItemTypeId);
                ViewData["transferOutId"] = new SelectList(_context.TransferOut, "transferOutId", "transferOutId", transferOutLine.TransferOutId);
        return View(transferOutLine);
    }

    // POST: TransferOutLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("transferOutLineId,transferOutId,ItemTypeId,Quantity,QtyInventory,createdAt")] TransferOutLine transferOutLine)
    {
        if (id != transferOutLine.TransferOutLineId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transferOutLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransferOutLineExists(transferOutLine.TransferOutLineId))
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
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", transferOutLine.ItemTypeId);
                ViewData["transferOutId"] = new SelectList(_context.TransferOut, "transferOutId", "transferOutId", transferOutLine.TransferOutId);
        return View(transferOutLine);
    }

    // GET: TransferOutLine/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transferOutLine = await _context.TransferOutLine
                .Include(t => t.ItemType)
                .Include(t => t.TransferOut)
                .SingleOrDefaultAsync(m => m.TransferOutLineId == id);
        if (transferOutLine == null)
        {
            return NotFound();
        }

        return View(transferOutLine);
    }




    // POST: TransferOutLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var transferOutLine = await _context.TransferOutLine.SingleOrDefaultAsync(m => m.TransferOutLineId == id);
            _context.TransferOutLine.Remove(transferOutLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransferOutLineExists(string id)
    {
        return _context.TransferOutLine.Any(e => e.TransferOutLineId == id);
    }

  }
}





namespace WMS.MVC
{
  public static partial class Pages
  {
      public static class TransferOutLine
      {
          public const string Controller = "TransferOutLine";
          public const string Action = "Index";
          public const string Role = "TransferOutLine";
          public const string Url = "/TransferOutLine/Index";
          public const string Name = "TransferOutLine";
      }
  }
}
namespace WMS.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "TransferOutLine")]
      public bool TransferOutLineRole { get; set; } = false;
  }
}



