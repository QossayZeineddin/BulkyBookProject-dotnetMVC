using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BulkyBook.Areas.Customer.Models;
using BulkyBook.Repository.IRepository;



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
            var productList = _unitOfWork.product.getAll(includeProperies: "Categery,CoverType");

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

        public IActionResult Details(int? id)
        {
            ShoppingCart shoppingCart = new()
            {
                count = 1,
                Product = _unitOfWork.product.getFirstOrDefault(u => u.Id == id,
                    includeProperies: "Categery,CoverType")
            };
            return View(shoppingCart);
        }
        
   
    }
}