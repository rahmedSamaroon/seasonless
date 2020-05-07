using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Seasonless.Data;
using Seasonless.Models;

namespace Seasonless.Controllers
{
    public class RepaymentsController : Controller
    {
        private readonly AppDb _context;

        public RepaymentsController(AppDb context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm(Name = "data")] IFormFile file)
        {
            string fileContents;
            using (var stream = file.OpenReadStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    fileContents = await reader.ReadToEndAsync();
                }
            }

            var repaymentUploads = JsonConvert.DeserializeObject<List<RepaymentUpload>>(fileContents);

            var repayments = new List<Repayment>();
            for (var index = 0; index < repaymentUploads.Count; index++)
            {
                repayments.AddRange(_context.ProcessPaymentAt(repaymentUploads, index));
            }

            return RedirectToAction(nameof(Index), repayments);
        }

        [HttpPost]
        public async Task<IActionResult> Reset()
        {
            _context.Repayments.RemoveRange(_context.Repayments);
            _context.RepaymentUploads.RemoveRange(_context.RepaymentUploads);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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