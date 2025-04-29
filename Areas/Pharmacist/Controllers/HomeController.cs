using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmacyInventorySystem.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyInventorySystem.Areas.Pharmacist.Controllers
{
    [Area("Pharmacist")]
    [Authorize(Roles = "Pharmacist")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            int lowStockThreshold = 10;
            int expiryThresholdDays = 30;
            DateTime today = DateTime.Today;
            DateTime nearExpiry = today.AddDays(expiryThresholdDays);

            var alerts = await _context.Products
                .Where(p => p.Quantity < lowStockThreshold || p.ExDate <= nearExpiry)
                .ToListAsync();

            ViewBag.HasAlerts = alerts.Any();
            return View();
        }

        public async Task<IActionResult> Report()
        {
            int lowStockThreshold = 10;
            int expiryThresholdDays = 30;
            DateTime today = DateTime.Today;

            var products = await _context.Products
                .Where(p =>
                    p.Quantity < lowStockThreshold ||
                    EF.Functions.DateDiffDay(today, p.ExDate) <= expiryThresholdDays)
                .ToListAsync();

            return View(products);
        }


    }
}
