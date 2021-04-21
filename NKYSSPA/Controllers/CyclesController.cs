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

    }
}
