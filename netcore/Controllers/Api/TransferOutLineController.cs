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
    [Route("api/TransferOutLine")]
    public class TransferOutLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferOutLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TransferOutLine
        [HttpGet]
        [Authorize]
        public IActionResult GetTransferOutLine(string masterid)
        {
            return Json(new { data = _context.TransferOutLine.Include(x => x.ItemType).Where(x => x.TransferOutId.Equals(masterid)).ToList() });
        }

        // POST: api/TransferOutLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTransferOutLine([FromBody] TransferOutLine transferOutLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (transferOutLine.TransferOutLineId == string.Empty)
            {
                transferOutLine.TransferOutLineId = Guid.NewGuid().ToString();
                _context.TransferOutLine.Add(transferOutLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new transfer item success." });
            }
            else
            {
                _context.Update(transferOutLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit transfer item success." });
            }

        }

        // DELETE: api/TransferOutLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeleteTransferOutLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transferOutLine = await _context.TransferOutLine.SingleOrDefaultAsync(m => m.TransferOutLineId == id);
            if (transferOutLine == null)
            {
                return NotFound();
            }

            _context.TransferOutLine.Remove(transferOutLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete transfer item success." });
        }


        private bool TransferOutLineExists(string id)
        {
            return _context.TransferOutLine.Any(e => e.TransferOutLineId == id);
        }


    }

}
