using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seasonless.Data;

namespace Seasonless.Controllers
{
    public class RepaymentsController : Controller
    {
        private readonly AppDb _context;

        public RepaymentsController(AppDb context)
        {
            _context = context;
        }

        // GET: Repayments
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                var repayments = _context.Repayments.Include(r => r.Customer).Include(r => r.Season);
                return View(await repayments.ToListAsync());
            }
            else
            {
                var repayments = _context.Repayments.Where(cs => cs.CustomerID == id)
                    .Include(c => c.Customer).Include(c => c.Season);
                return View(await repayments.ToListAsync());
            }
        }

        // GET: Repayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repayment = await _context.Repayments
                .Include(r => r.Customer)
                .Include(r => r.Season)
                .FirstOrDefaultAsync(m => m.RepaymentID == id);

            if (repayment == null)
            {
                return NotFound();
            }

            return View(repayment);
        }
    }
}