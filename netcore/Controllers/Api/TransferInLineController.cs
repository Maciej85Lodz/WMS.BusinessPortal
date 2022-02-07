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
    [Route("api/TransferInLine")]
    public class TransferInLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferInLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TransferInLine
        [HttpGet]
        [Authorize]
        public IActionResult GetTransferInLine(string masterid)
        {
            return Json(new { data = _context.TransferInLine.Include(x => x.ItemType).Where(x => x.TransferInId.Equals(masterid)).ToList() });
        }

        // POST: api/TransferInLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTransferInLine([FromBody] TransferInLine transferInLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (transferInLine.TransferInLineId == string.Empty)
            {
                transferInLine.TransferInLineId = Guid.NewGuid().ToString();
                _context.TransferInLine.Add(transferInLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new transfer item success." });
            }
            else
            {
                _context.Update(transferInLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit transfer item success." });
            }

        }

        // DELETE: api/TransferInLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeleteTransferInLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transferInLine = await _context.TransferInLine.SingleOrDefaultAsync(m => m.TransferInLineId == id);
            if (transferInLine == null)
            {
                return NotFound();
            }

            _context.TransferInLine.Remove(transferInLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete transfer item success." });
        }


        private bool TransferInLineExists(string id)
        {
            return _context.TransferInLine.Any(e => e.TransferInLineId == id);
        }


    }

}
