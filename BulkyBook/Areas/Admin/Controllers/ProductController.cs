using BulkyBook.Areas.Admin.Models;
using BulkyBook.Areas.Admin.Models.ViewModels;
using BulkyBook.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
   // private static int id = 1;
    private readonly IUnitOfWork _unitOfWork;
    private IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    // GET
    public IActionResult Index()
    {
        // IEnumerable<Categery> categery = from VAR in _db.getAll();
        //     select VAR;
        //var categerys = _db.categeries.ToList();

        return View();
    }


    public IActionResult Upsert(double? id)
    {
        ProductVM productVM = new()
        {
            product = new(),
            categoryList = _unitOfWork.categery.getAll().Select(i => new SelectListItem
            {
                Text = i.name,
                Value = i.id.ToString()
            }),
            coverTypeList = _unitOfWork.coverType.getAll().Select(i => new SelectListItem
            {
                Text = i.CoverName,
                Value = i.id.ToString()
            })
        };
        // Product product = new();
        // IEnumerable<SelectListItem> categoryList = _unitOfWork.categery.getAll().Select(
        //     u => new SelectListItem
        //     {
        //         Text = u.name,
        //         Value = u.id.ToString()
        //     }
        // );
        // IEnumerable<SelectListItem> coverTypeList = _unitOfWork.coverType.getAll().Select(
        //     u => new SelectListItem
        //     {
        //         Text = u.CoverName,
        //         Value = u.id.ToString()
        //     }
        // );
        //  
        if (id == null || id == 0)
        {
            // ViewBag.categoryList = categoryList;
            // ViewData["CoverTypeList"] = coverTypeList;
            return View(productVM);
        }
        else
        {
            productVM.product = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
            return View(productVM);
        }
    }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public IActionResult Upsert(ProductVM obj, IFormFile? file)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         string wwwRootPath = _hostEnvironment.WebRootPath;
    //         if (file != null)
    //         {
    //             string fileName = Guid.NewGuid().ToString();
    //             var uploads = Path.Combine(wwwRootPath, @"Images\Products");
    //             var extension = Path.GetExtension(file.FileName);
    //
    //             if (obj.product.ImageUrl != null)
    //             {
    //                 var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
    //                 if (System.IO.File.Exists(oldImagePath))
    //                 {
    //                     System.IO.File.Delete(oldImagePath);
    //                 }
    //             }
    //
    //             using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
    //             {
    //                 file.CopyTo(fileStreams);
    //             }
    //
    //             obj.product.ImageUrl = @"\Images\Products\" + fileName + extension;
    //         }
    //
    //         if (obj.product.Id == 0)
    //         {
    //             obj.product.Id = id++;
    //
    //             _unitOfWork.product.add(obj.product);
    //         }
    //         else
    //         {
    //             _unitOfWork.product.update(obj.product);
    //         }
    //
    //         _unitOfWork.save();
    //         TempData["success"] = "Product created successfully";
    //         return RedirectToAction("Index");
    //     }
    //
    //     return View(obj);
    // }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Images\Products");
                var extension = Path.GetExtension(file.FileName);
                if (obj.product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                obj.product.ImageUrl = @"\Images\Products\" + fileName + extension;
            }

            if (obj.product.Id == 0)
            {
               // obj.product.Id = id++;

                _unitOfWork.product.add(obj.product);
            }
            else
            {
                _unitOfWork.product.update(obj.product);
            }

            _unitOfWork.save();
            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index");
        }

        return View(obj);
    }


    // public IActionResult Delete(double? id)
    // {
    //     if (id == null || _unitOfWork.product.getAll() == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var products = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
    //     if (products == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return View(products);
    // }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(double id)
    {
        if (_unitOfWork.product.getAll() == null)
        {
            return Problem("Entity set 'Bulky.products'  is null.");
        }

        var products = _unitOfWork.product.getFirstOrDefault(u => u.Id == id);
        if (products != null)
        {
            _unitOfWork.product.remove(products);
            TempData["success"] = "product  deleted successfully!";
        }

        _unitOfWork.save();
        return RedirectToAction(nameof(Index));
    }

    private bool productExists(double id)
    {
        return (_unitOfWork.product.getAll()?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    #region API CALLS

    [HttpGet]
    public IActionResult getAll()
    {
        var productList = _unitOfWork.product.getAll(includeProperies: "Categery,CoverType");
        return Json(new { data = productList });
    }

    #endregion
}