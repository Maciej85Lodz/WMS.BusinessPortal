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
    [Route("api/SalesOrderLine")]
    public class SalesOrderLineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderLineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SalesOrderLine
        [HttpGet]
        [Authorize]
        public IActionResult GetSalesOrderLine(string masterid)
        {
            return Json(new { data = _context.SalesOrderLine.Include(x => x.ItemType).Where(x => x.SalesOrderId.Equals(masterid)).ToList() });
        }

        // POST: api/SalesOrderLine
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostSalesOrderLine([FromBody] SalesOrderLine salesOrderLine)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SalesOrder salesOrder = await _context.SalesOrder.Where(x => x.SalesOrderId.Equals(salesOrderLine.SalesOrderId)).FirstOrDefaultAsync();

            if (salesOrder.SalesOrderStatus == SalesOrderStatus.Completed)
            {
                return Json(new { success = false, message = "Error. Can not edit [Completed] order" });
            }

            salesOrderLine.TotalAmount = (decimal)salesOrderLine.Qty * salesOrderLine.Price;

            if (salesOrderLine.SalesOrderLineId == string.Empty)
            {
                salesOrderLine.SalesOrderLineId = Guid.NewGuid().ToString();
                _context.SalesOrderLine.Add(salesOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Add new order line success." });
            }
            else
            {
                _context.Update(salesOrderLine);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Edit order line success." });
            }

        }

        // DELETE: api/SalesOrderLine/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult>  DeleteSalesOrderLine([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesOrderLine = await _context.SalesOrderLine
                .Include(x => x.SalesOrder)
                .SingleOrDefaultAsync(m => m.SalesOrderLineId == id);
            if (salesOrderLine == null)
            {
                return NotFound();
            }

            if (salesOrderLine.SalesOrder.SalesOrderStatus == SalesOrderStatus.Completed)
            {
                return Json(new { success = false, message = "Error. Can not delete [Completed] order" });
            }

            _context.SalesOrderLine.Remove(salesOrderLine);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Delete order line success." });
        }


        private bool SalesOrderLineExists(string id)
        {
            return _context.SalesOrderLine.Any(e => e.SalesOrderLineId == id);
        }


    }

}
