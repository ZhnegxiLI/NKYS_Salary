using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKYSSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYSSPA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CyclesController : ControllerBase
    {
        private readonly Context _context;

        public CyclesController(Context context)
        {
            _context = context;

        }

        [HttpGet]
        public async Task<List<Cycle>> getCycles()
        {
            return await _context.Cycle.ToListAsync();
        }

        [HttpPost]
        public async Task<long> CreateOrEdit(Cycle cycle)
        {
            cycle.Year = cycle.FromDate.Year;// place into sql trigger 
            cycle.Month = cycle.FromDate.Month; // place into sql trigger
            if (cycle.Id>0)
            {
                // Update
                _context.Cycle.Update(cycle);
            }
            else
            {
                cycle.CreatedOn = DateTime.Now;
                await _context.Cycle.AddAsync(cycle);
                // Add
            }
            await _context.SaveChangesAsync();
            return cycle.Id;
        }
    }
}
