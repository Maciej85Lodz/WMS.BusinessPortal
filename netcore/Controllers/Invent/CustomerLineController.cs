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


    [Authorize(Roles = "CustomerLine")]
    public class CustomerLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerLine
        public async Task<IActionResult> Index()
        {
                    var applicationDbContext = _context.CustomerLine.Include(c => c.Customer);
                    return View(await applicationDbContext.ToListAsync());
        }        

    // GET: CustomerLine/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customerLine = await _context.CustomerLine
                .Include(c => c.Customer)
                    .SingleOrDefaultAsync(m => m.CustomerLineId == id);
        if (customerLine == null)
        {
            return NotFound();
        }

        return View(customerLine);
    }


    // GET: CustomerLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.CustomerLine.SingleOrDefault(m => m.CustomerLineId == id);
        var selected = _context.Customer.SingleOrDefault(m => m.CustomerId == masterid);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
        if (check == null)
        {
            CustomerLine objline = new CustomerLine();
            objline.CustomerLineId = string.Empty;
            objline.Customer = selected;
            objline.CustomerId = masterid;
            return View(objline);
        }
        else
        {
            return View(check);
        }
    }




    // POST: CustomerLine/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("customerLineId,jobTitle,CustomerId,firstName,lastName,middleName,nickName,gender,salutation,mobilePhone,officePhone,fax,personalEmail,workEmail,createdAt")] CustomerLine customerLine)
    {
        if (ModelState.IsValid)
        {
            _context.Add(customerLine);
            await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.CustomerId);
        return View(customerLine);
    }

    // GET: CustomerLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customerLine = await _context.CustomerLine.SingleOrDefaultAsync(m => m.CustomerLineId == id);
        if (customerLine == null)
        {
            return NotFound();
        }
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.CustomerId);
        return View(customerLine);
    }

    // POST: CustomerLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("customerLineId,jobTitle,CustomerId,firstName,lastName,middleName,nickName,gender,salutation,mobilePhone,officePhone,fax,personalEmail,workEmail,createdAt")] CustomerLine customerLine)
    {
        if (id != customerLine.CustomerLineId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(customerLine);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerLineExists(customerLine.CustomerLineId))
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
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.CustomerId);
        return View(customerLine);
    }

    // GET: CustomerLine/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customerLine = await _context.CustomerLine
                .Include(c => c.Customer)
                .SingleOrDefaultAsync(m => m.CustomerLineId == id);
        if (customerLine == null)
        {
            return NotFound();
        }

        return View(customerLine);
    }




    // POST: CustomerLine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var customerLine = await _context.CustomerLine.SingleOrDefaultAsync(m => m.CustomerLineId == id);
            _context.CustomerLine.Remove(customerLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CustomerLineExists(string id)
    {
        return _context.CustomerLine.Any(e => e.CustomerLineId == id);
    }

  }
}





namespace WMS.MVC
{
  public static partial class Pages
  {
      public static class CustomerLine
      {
          public const string Controller = "CustomerLine";
          public const string Action = "Index";
          public const string Role = "CustomerLine";
          public const string Url = "/CustomerLine/Index";
          public const string Name = "CustomerLine";
      }
  }
}
namespace WMS.Models
{
  public partial class ApplicationUser
  {
      [Display(Name = "CustomerLine")]
      public bool CustomerLineRole { get; set; } = false;
  }
}



