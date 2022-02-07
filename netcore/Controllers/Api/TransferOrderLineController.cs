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
    [Route("api/TransferOrderLine")]
    public class TransferOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TransferOrderLine
        [HttpGet]
        [Authorize]
        public IActionResult GetTransferOrderLine(string masterid)
        {
            return Json(new { data = _context.TransferOrderLine.Include(x => x.ItemType).Where(x => x.TransferOrderId.Equals(masterid)).ToList() });
        }

        // POST: api/TransferOrderLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostTransferOrderLine([FromBody] TransferOrderLine transferOrderLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TransferOrder transferOrder = await _context.TransferOrder.Where(x => x.TransferOrderId.Equals(transferOrderLine.TransferOrderId)).FirstOrDefaultAsync();

            if (transferOrder.TransferOrderStatus == TransferOrderStatus.Completed)
            {
                return Json(new { success = false, message = "Error. Can not edit [Completed] order" });
            }

            if (transferOrder.IsIssued == true)
            {
                return Json(new { success = false, message = "Error. Can not edit [Open] order that already process the goods issue" });
            }

            if (transferOrder.IsReceived == true)
            {
                return Json(new { success = false, message = "Error. Can not edit [Open] order that already process the goods receive" });
            }

            if (transferOrderLine.TransferOrderLineId == string.Empty)
            {
                transferOrderLine.TransferOrderLineId = Guid.NewGuid().ToString();
                _context.TransferOrderLine.Add(transferOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new transfer item success." });
            }
            else
            {
                _context.Update(transferOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit transfer item success." });
            }

        }

        // DELETE: api/TransferOrderLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeleteTransferOrderLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transferOrderLine = await _context.TransferOrderLine
                .Include(x => x.TransferOrder)
                .SingleOrDefaultAsync(m => m.TransferOrderLineId == id);
            if (transferOrderLine == null)
            {
                return NotFound();
            }

            if (transferOrderLine.TransferOrder.TransferOrderStatus == TransferOrderStatus.Completed
                || transferOrderLine.TransferOrder.IsIssued == true
                || transferOrderLine.TransferOrder.IsReceived == true)
            {
                return Json(new { success = false, message = "Error. Can not delete [Completed] order or [Open] order that already process the goods issue or goods receive" });
            }

            _context.TransferOrderLine.Remove(transferOrderLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete transfer item success." });
        }


        private bool TransferOrderLineExists(string id)
        {
            return _context.TransferOrderLine.Any(e => e.TransferOrderLineId == id);
        }


    }

}
