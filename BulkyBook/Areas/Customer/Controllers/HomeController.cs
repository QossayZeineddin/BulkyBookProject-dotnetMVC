using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.Areas.Customer.Models;
using BulkyBook.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BulkyBook.Areas.Admin.Models;
using System.Collections.Generic;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.product.getAll(includeProperies: "Categery,CoverType");

            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int prodectId)
        {

            ShoppingCart shoppingCart = new()
            {
                count = 1,
                productId = prodectId,
                Product = _unitOfWork.product.getFirstOrDefault(u => u.Id == prodectId,
                    includeProperies: "Categery,CoverType")
            };

            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var climIdentity = (ClaimsIdentity)User.Identity;
            var clim = climIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.applecationUserId = clim.Value;
            ShoppingCart cartFromDb = _unitOfWork.shoppingCart.getFirstOrDefault(
                u => u.applecationUserId == clim.Value && u.productId == shoppingCart.productId);
            if (cartFromDb == null)
            {
                _unitOfWork.shoppingCart.add(shoppingCart);
            }
            else
            {
                _unitOfWork.shoppingCart.IncrementCount(cartFromDb, shoppingCart.count);

            }
            _unitOfWork.save();

            return RedirectToAction(nameof(Index));
        }


    }
}