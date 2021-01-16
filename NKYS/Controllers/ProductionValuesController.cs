using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NKYS.Models;
using NKYS.Models.ViewModel;

namespace NKYS.Controllers
{
    [Authorize]
    public class ProductionValuesController : Controller
    {
        private readonly Context _context;

        public ProductionValuesController(Context context)
        {
            _context = context;
        }

        // GET: ProductionValues
        public async Task<IActionResult> Index(long? CycleId, ProductionValueType? ProductionValueTypeId)
        {
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label");

            var list = await (from value in _context.ProductionValue
                        join c in _context.Cycle on value.CycleId equals c.Id
                        where (CycleId == null || c.Id == CycleId ) && (ProductionValueTypeId == null || value.ProductionValueTypeId == ProductionValueTypeId)
                        orderby c.Year descending, c.Month descending
                        select value).Include(p => p.Cycle).ToListAsync();
           
            var obj = new ProductionValuesIndex();
            obj.ProductionValues = list;

            return View(obj);
        }

        // GET: ProductionValues/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionValue = await _context.ProductionValue
                .Include(p => p.Cycle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionValue == null)
            {
                return NotFound();
            }

            return View(productionValue);
        }

        // GET: ProductionValues/Create
        public async Task<IActionResult> CreateOrEdit(long? id)
        {
            if (id == null)
            {
                ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label");
                ViewData["Action"] = "Create";
                return View();
            }

            var productionValue = await _context.ProductionValue.FindAsync(id);
            ViewData["Action"] = "Update";
            if (productionValue == null)
            {
                return NotFound();
            }
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label", productionValue.CycleId);
            return View(productionValue);
        }

        // POST: ProductionValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("CycleId,Value,Comment,ProductionValueTypeId,Id,CreatedBy,UpdatedBy,CreatedOn,UpdatedOn,Validity")] ProductionValue productionValue)
        {
            if (ModelState.IsValid)
            {
                var existantProductionValue = _context.ProductionValue.Where(p => p.CycleId == productionValue.CycleId && p.ProductionValueTypeId == productionValue.ProductionValueTypeId).FirstOrDefault();
                /* Production Value not yet exist */
                if (existantProductionValue == null)
                {
                    if (productionValue.Id>0)
                    {
                        _context.Update(productionValue);
                    }
                    else
                    {
                        productionValue.CreatedOn = DateTime.Now;
                        _context.Add(productionValue);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label");
                    ModelState.AddModelError(string.Empty, "This type of production value is already exists, cannot create repeately");
                    return View(productionValue);
                }
            
            }
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label", productionValue.CycleId);
            return View(productionValue);
        }


        // GET: ProductionValues/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionValue = await _context.ProductionValue
                .Include(p => p.Cycle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionValue == null)
            {
                return NotFound();
            }

            return View(productionValue);
        }

        // POST: ProductionValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var productionValue = await _context.ProductionValue.FindAsync(id);
            _context.ProductionValue.Remove(productionValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionValueExists(long id)
        {
            return _context.ProductionValue.Any(e => e.Id == id);
        }

        // API
        [HttpGet]
        public JsonResult GetProductionValueTypeList()
        {
            var list = Enum.GetValues(typeof(ProductionValueType)).Cast<ProductionValueType>().Select(p=> new { Type  = p.ToString(), Value = p }).ToList();

            return Json(list);
        }
    }
}
