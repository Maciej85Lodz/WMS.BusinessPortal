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
    [Route("api/PurchaseOrderLine")]
    public class PurchaseOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PurchaseOrderLine
        [HttpGet]
        [Authorize]
        public IActionResult GetPurchaseOrderLine(string masterid)
        {
            return Json(new { data = _context.PurchaseOrderLine.Include(x => x.ItemType).Where(x => x.PurchaseOrderId.Equals(masterid)).ToList() });
        }

        // POST: api/PurchaseOrderLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostPurchaseOrderLine([FromBody] PurchaseOrderLine purchaseOrderLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PurchaseOrder purchaseOrder = await _context.PurchaseOrder.Where(x => x.PurchaseOrderId.Equals(purchaseOrderLine.PurchaseOrderId)).FirstOrDefaultAsync();

            if (purchaseOrder.PurchaseOrderStatus == PurchaseOrderStatus.Completed)
            {
                return Json(new { success = false, message = "Error. Can not edit [Completed] order" });
            }

            purchaseOrderLine.TotalAmount = (decimal)purchaseOrderLine.Quantity * purchaseOrderLine.ItemPrice;

            if (purchaseOrderLine.PurchaseOrderLineId == string.Empty)
            {
                purchaseOrderLine.PurchaseOrderLineId = Guid.NewGuid().ToString();
                _context.PurchaseOrderLine.Add(purchaseOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new order line success." });
            }
            else
            {
                _context.Update(purchaseOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit order line success." });
            }

        }

        // DELETE: api/PurchaseOrderLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeletePurchaseOrderLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var purchaseOrderLine = await _context.PurchaseOrderLine
                .Include(x => x.PurchaseOrder)
                .SingleOrDefaultAsync(m => m.PurchaseOrderLineId == id);

            if (purchaseOrderLine == null)
            {
                return NotFound();
            }

            if (purchaseOrderLine.PurchaseOrder.PurchaseOrderStatus == PurchaseOrderStatus.Completed)
            {
                return Json(new { success = false, message = "Error. Can not delete [Completed] order" });
            }

            _context.PurchaseOrderLine.Remove(purchaseOrderLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete order line success." });
        }


        private bool PurchaseOrderLineExists(string id)
        {
            return _context.PurchaseOrderLine.Any(e => e.PurchaseOrderLineId == id);
        }


    }

}
