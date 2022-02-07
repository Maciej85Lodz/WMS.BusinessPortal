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


    [Authorize(Roles = "ShipmentLine")]
    public class ShipmentLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShipmentLine
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.ShipmentLine.Include(s => s.Branch).Include(s => s.ItemType).Include(s => s.Shipment).Include(s => s.Warehouse);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: ShipmentLine/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var shipmentLine = await _context.ShipmentLine
                .Include(s => s.Branch)
                .Include(s => s.ItemType)
                .Include(s => s.Shipment)
                .Include(s => s.Warehouse)
                    .SingleOrDefaultAsync(m => m.ShipmentLineId == id);
        if (shipmentLine == null)
        {
            return NotFound();
        }

        return View(shipmentLine);
    }


    // GET: ShipmentLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.ShipmentLine.SingleOrDefault(m => m.ShipmentLineId == id);
        var selected = _context.Shipment.SingleOrDefault(m => m.ShipmentId == masterid);
            ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchId");
            ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId");
            ViewData["shipmentId"] = new SelectList(_context.Shipment, "shipmentId", "shipmentId");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "WarehouseId");
        if (check == null)
        {
            ShipmentLine objline = new ShipmentLine();
            objline.ShipmentLineId = string.Empty;
            objline.Shipment = selected;
            objline.ShipmentId = masterid;
            return View(objline);
        }
        else
        {
            return View(check);
        }
    }




    // POST: ShipmentLine/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("shipmentLineId,shipmentId,BranchId,WarehouseId,ItemTypeId,Quantity,qtyShipment,QtyInventory,createdAt")] ShipmentLine shipmentLine)
    {
        if (ModelState.IsValid)
        {
            _context.Add(shipmentLine);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchId", shipmentLine.BranchId);
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", shipmentLine.ItemTypeId);
                ViewData["shipmentId"] = new SelectList(_context.Shipment, "shipmentId", "shipmentId", shipmentLine.ShipmentId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "WarehouseId", shipmentLine.WarehouseId);
        return View(shipmentLine);
    }

    // GET: ShipmentLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var shipmentLine = await _context.ShipmentLine.SingleOrDefaultAsync(m => m.ShipmentLineId == id);
        if (shipmentLine == null)
        {
            return NotFound();
        }
                ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchId", shipmentLine.BranchId);
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", shipmentLine.ItemTypeId);
                ViewData["shipmentId"] = new SelectList(_context.Shipment, "shipmentId", "shipmentId", shipmentLine.ShipmentId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "WarehouseId", shipmentLine.WarehouseId);
        return View(shipmentLine);
    }

    // POST: ShipmentLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("shipmentLineId,shipmentId,BranchId,WarehouseId,ItemTypeId,Quantity,qtyShipment,QtyInventory,createdAt")] ShipmentLine shipmentLine)
    {
        if (id != shipmentLine.ShipmentLineId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(shipmentLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentLineExists(shipmentLine.ShipmentLineId))
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
                ViewData["BranchId"] = new SelectList(_context.Branch, "BranchId", "BranchId", shipmentLine.BranchId);
                ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "ItemTypeId", "ItemTypeId", shipmentLine.ItemTypeId);
                ViewData["shipmentId"] = new SelectList(_context.Shipment, "shipmentId", "shipmentId", shipmentLine.ShipmentId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouse, "WarehouseId", "WarehouseId", shipmentLine.WarehouseId);
        return View(shipmentLine);
    }

    // GET: ShipmentLine/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var shipmentLine = await _context.ShipmentLine
                .Include(s => s.Branch)
                .Include(s => s.ItemType)
                .Include(s => s.Shipment)
                .Include(s => s.Warehouse)
                .SingleOrDefaultAsync(m => m.ShipmentLineId == id);
        if (shipmentLine == null)
        {
            return NotFound();
        }

        return View(shipmentLine);
    }




    // POST: ShipmentLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var shipmentLine = await _context.ShipmentLine.SingleOrDefaultAsync(m => m.ShipmentLineId == id);
            _context.ShipmentLine.Remove(shipmentLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ShipmentLineExists(string id)
    {
        return _context.ShipmentLine.Any(e => e.ShipmentLineId == id);
    }

  }
}





namespace WMS.MVC
{
  public static partial class Pages
  {
      public static class ShipmentLine
      {
          public const string Controller = "ShipmentLine";
          public const string Action = "Index";
          public const string Role = "ShipmentLine";
          public const string Url = "/ShipmentLine/Index";
          public const string Name = "ShipmentLine";
      }
  }
}
namespace WMS.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "ShipmentLine")]
      public bool ShipmentLineRole { get; set; } = false;
  }
}



