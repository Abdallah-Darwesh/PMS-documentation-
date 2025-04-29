// Areas/Manager/Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyInventorySystem.Data;
using PharmacyInventorySystem.ViewModels;
using System.Linq;

namespace PharmacyInventorySystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var vm = new DashboardViewModel
            {
                EmployeeCount = _context.Users.Count(),
                TotalSales = _context.Sales.Sum(s => s.TotalPrice),
                TotalReturns = _context.Returns.Count(),
                InventoryCount = _context.Products.Count(),

                // إجمالي أوامر الموردين المسجّلة
                TotalOrders = _context.SupplierOrders.Count(),

                // تنبيهات المخزون: الكمية صفر
                LowStockAlerts = _context.Products.Count(p => p.Quantity == 0)
            };

            return View(vm);
        }
    }
}
