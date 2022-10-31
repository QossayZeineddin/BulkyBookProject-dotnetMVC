// using BulkyBook.Areas.Admin.Models;
// using BulkyBook.Repository.IRepository;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
//
// namespace BulkyBook.Areas.Admin.Controllers;
// [Area("Admin")]
// public class ProductController : Controller
// {
//     private static int id = 1;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public ProductController(IUnitOfWork unitOfWork)
//     {
//         _unitOfWork = unitOfWork;
//     }
//
//     // GET
//     public IActionResult Index()
//     {
//         // IEnumerable<Categery> categery = from VAR in _db.getAll();
//         //     select VAR;
//
//         IEnumerable<Product> objs = _unitOfWork.product.getAll();
//         //var categerys = _db.categeries.ToList();
//         return View(objs);
//     }
//
//
//     public IActionResult Create()
//     {
//         return View();
//     }
//
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public IActionResult Create(Product product)
//     {
//         // var users =  _db.categeries.FirstOrDefaultAsync(m => m.name == categery.name);
//         // Console.Write(users.ToString());
//         // if (categery.name == users.ToString())
//         // {
//         //     ModelState.AddModelError("customError" , "there is alrady the same name of categorys");
//         // }
//         if (ModelState.IsValid)
//         {
//             product.Id = id++;
//             _unitOfWork.product.add(product);
//             _unitOfWork.save();
//             TempData["success"] = "Categery  Created successfully!";
//             return RedirectToAction(nameof(Index));
//         }
//
//         return View(product);
//     }
//
//     public IActionResult Edit(double? id)
//     {
//         if (id == null || id == 0)
//         {
//             return BadRequest();
//         }
//
//         var productInDataBase = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
//         if (productInDataBase is null)
//         {
//             return NotFound();
//         }
//
//         return View(productInDataBase);
//     }
//
//
//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public IActionResult Edit(double id, Product product)
//     {
//         if (id != product.Id)
//         {
//             return NotFound();
//         }
//
//         if (ModelState.IsValid)
//         {
//             try
//             {
//                 _unitOfWork.product.update(product);
//                 TempData["success"] = "Product  updated successfully!";
//                 _unitOfWork.save();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!productExists(product.Id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }
//
//             return RedirectToAction(nameof(Index));
//         }
//
//         return View(product);
//     }
//
//
//     public IActionResult Delete(double? id)
//     {
//         if (id == null || _unitOfWork.product.getAll() == null)
//         {
//             return NotFound();
//         }
//
//         var products = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
//         if (products == null)
//         {
//             return NotFound();
//         }
//
//         return View(products);
//     }
//
//
//     [HttpPost, ActionName("Delete")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> DeleteProduct(double id)
//     {
//         if (_unitOfWork.product.getAll() == null)
//         {
//             return Problem("Entity set 'Bulky.products'  is null.");
//         }
//
//         var products = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
//         if (products != null)
//         {
//             _unitOfWork.product.remove(products);
//             TempData["success"] = "product  deleted successfully!";
//         }
//
//         _unitOfWork.save();
//         return RedirectToAction(nameof(Index));
//     }
//
//     private bool productExists(double id)
//     {
//         return (_unitOfWork.product.getAll()?.Any(e => e.Id == id)).GetValueOrDefault();
//     }
// }