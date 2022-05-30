using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WMS.Data;
using WMS.Models.Invent;

namespace WMS.Controllers.Api
{

    [Produces("application/json")]
    [Route("api/CustomerLine")]
    public class CustomerLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerLine
        [HttpGet]
        [Authorize]
        public IActionResult GetCustomerLine(string masterid)
        {
            return Json(new { data = _context.CustomerLine.Where(x => x.CustomerId.Equals(masterid)).ToList() });
        }

        // POST: api/CustomerLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostCustomerLine([FromBody] CustomerLine customerLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (customerLine.CustomerLineId == string.Empty)
            {
                customerLine.CustomerLineId = Guid.NewGuid().ToString();
                _context.CustomerLine.Add(customerLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new contact " + customerLine.FirstName + " " + customerLine.LastName + " success." });
            }
            else
            {
                _context.Update(customerLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit contact " + customerLine.FirstName + " " + customerLine.LastName + " success." });
            }

        }

        // DELETE: api/CustomerLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeleteCustomerLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerLine = await _context.CustomerLine.SingleOrDefaultAsync(m => m.CustomerLineId == id);
            if (customerLine == null)
            {
                return NotFound();
            }

            _context.CustomerLine.Remove(customerLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete " + customerLine.FirstName + " " + customerLine.LastName + " success." });
        }


        private bool CustomerLineExists(string id)
        {
            return _context.CustomerLine.Any(e => e.CustomerLineId == id);
        }


    }

}
