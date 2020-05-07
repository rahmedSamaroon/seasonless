using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seasonless.Data;
using Seasonless.Models;

namespace Seasonless.Controllers
{
    public class ServicesController : Controller
    {
        private readonly AppDb _context;

        public ServicesController(AppDb context)
        {
            _context = context;
        }
    }
}