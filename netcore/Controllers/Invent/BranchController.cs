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


    [Authorize(Roles = "Branch")]
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Branch
        public async Task<IActionResult> Index()
        {
            return View(await _context.Branch.OrderByDescending(x => x.CreatedAt).ToListAsync());
        }

        // GET: Branch/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                        .SingleOrDefaultAsync(m => m.BranchId == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }


        // GET: Branch/Create
        public IActionResult Create()
        {
            Branch obj = new Branch();
            return View(obj);
        }




        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,BranchName,Description,Street1,Street2,City,Province,Country,CreatedAt,IsDefaultBranch")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Create Branch "+branch.BranchName+" Success";
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branch/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.SingleOrDefaultAsync(m => m.BranchId == id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BranchId,branchName,Description,street1,street2,city,province,country,createdAt,isDefaultBranch")] Branch branch)
        {
            if (id != branch.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();

                    if (branch.IsDefaultBranch)
                    {
                        List<Branch> others = await _context.Branch.Where(x => !x.BranchId.Equals(branch.BranchId)).ToListAsync();
                        foreach (var item in others)
                        {
                            item.IsDefaultBranch = false;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.BranchId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["TransMessage"] = "Edit Branch " + branch.BranchName + " Success";
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branch/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                    .SingleOrDefaultAsync(m => m.BranchId == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }




        // POST: Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var branch = await _context.Branch.SingleOrDefaultAsync(m => m.BranchId == id);
            try
            {
                _context.Branch.Remove(branch);
                await _context.SaveChangesAsync();
                TempData["TransMessage"] = "Delete Branch " + branch.BranchName + " Success";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ViewData["StatusMessage"] = "Error. Calm Down ^_^ and please contact your SysAdmin with this message: " + ex;
                return View(branch);
            }
            
            

        }

        private bool BranchExists(string id)
        {
            return _context.Branch.Any(e => e.BranchId == id);
        }

    }
}





namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Branch
        {
            public const string Controller = "Branch";
            public const string Action = "Index";
            public const string Role = "Branch";
            public const string Url = "/Branch/Index";
            public const string Name = "Branch";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Branch")]
        public bool BranchRole { get; set; } = false;
    }
}



