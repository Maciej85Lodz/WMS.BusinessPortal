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
                    var applicationDbContext = _context.CustomerLine.Include(c => c.customer);
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
                .Include(c => c.customer)
                    .SingleOrDefaultAsync(m => m.customerLineId == id);
        if (customerLine == null)
        {
            return NotFound();
        }

        return View(customerLine);
    }


    // GET: CustomerLine/Create
    public IActionResult Create(string masterid, string id)
    {
        var check = _context.CustomerLine.SingleOrDefault(m => m.customerLineId == id);
        var selected = _context.Customer.SingleOrDefault(m => m.customerId == masterid);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
        if (check == null)
        {
            CustomerLine objline = new CustomerLine();
            objline.customerLineId = string.Empty;
            objline.customer = selected;
            objline.customerId = masterid;
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
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.customerId);
        return View(customerLine);
    }

    // GET: CustomerLine/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var customerLine = await _context.CustomerLine.SingleOrDefaultAsync(m => m.customerLineId == id);
        if (customerLine == null)
        {
            return NotFound();
        }
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.customerId);
        return View(customerLine);
    }

    // POST: CustomerLine/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("customerLineId,jobTitle,CustomerId,firstName,lastName,middleName,nickName,gender,salutation,mobilePhone,officePhone,fax,personalEmail,workEmail,createdAt")] CustomerLine customerLine)
    {
        if (id != customerLine.customerLineId)
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
                if (!CustomerLineExists(customerLine.customerLineId))
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
                ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", customerLine.customerId);
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
                .Include(c => c.customer)
                .SingleOrDefaultAsync(m => m.customerLineId == id);
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
        var customerLine = await _context.CustomerLine.SingleOrDefaultAsync(m => m.customerLineId == id);
            _context.CustomerLine.Remove(customerLine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CustomerLineExists(string id)
    {
        return _context.CustomerLine.Any(e => e.customerLineId == id);
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



