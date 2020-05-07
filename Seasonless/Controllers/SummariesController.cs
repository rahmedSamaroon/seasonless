using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seasonless.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Seasonless.Controllers
{
    public class SummariesController : Controller
    {
        private readonly AppDb _context;

        public SummariesController(AppDb context)
        {
            _context = context;
        }

        // GET: Summaries
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var summaries = _context.Summaries
                    .Include(cs => cs.Customer)
                    .Include(cs => cs.Season);
                return View(await summaries.ToListAsync());
            }
            else
            {
                var summaries = _context.Summaries
                    .Where(cs => cs.CustomerID == id)
                    .Include(c => c.Customer)
                    .Include(c => c.Season);
                return View(await summaries.ToListAsync());
            }
        }
    }
}