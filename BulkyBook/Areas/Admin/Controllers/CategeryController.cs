using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Areas.Admin.Controllers;

[Area("Admin")]
public class CategeryController : Controller
{
    // private static int id = 1;
    private readonly IUnitOfWork _unitOfWork;

    public CategeryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        // IEnumerable<Categery> categery = from VAR in _db.getAll();
        //     select VAR;

        IEnumerable<Categery> objs = _unitOfWork.categery.getAll();
        //var categerys = _db.categeries.ToList();
        return View(objs);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Categery categery)
    {
        // var users =  _db.categeries.FirstOrDefaultAsync(m => m.name == categery.name);
        // Console.Write(users.ToString());
        // if (categery.name == users.ToString())
        // {
        //     ModelState.AddModelError("customError" , "there is alrady the same name of categorys");
        // }
        if (ModelState.IsValid)
        {
            // categery.id = id++;
            _unitOfWork.categery.add(categery);
            _unitOfWork.save();
            TempData["success"] = "Categery Created successfully!";
            return RedirectToAction(nameof(Index));
        }

        return View(categery);
    }

    public IActionResult Edit(double? id)
    {
        if (id == null || id == 0)
        {
            return BadRequest();
        }

        var categeryInDataBase = _unitOfWork.categery.getFirstOrDefault(u => u.id == id);
        if (categeryInDataBase is null)
        {
            return NotFound();
        }

        return View(categeryInDataBase);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(double id, Categery categery)
    {
        if (id != categery.id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _unitOfWork.categery.update(categery);
                TempData["success"] = "Categery  updated successfully!";
                _unitOfWork.save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categeryExists(categery.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        return View(categery);
    }


    public IActionResult Delete(double? id)
    {
        if (id == null || _unitOfWork.categery.getAll() == null)
        {
            return NotFound();
        }

        var categeries = _unitOfWork.categery.getFirstOrDefault(u => u.id == id);
        if (categeries == null)
        {
            return NotFound();
        }

        return View(categeries);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategery(double id)
    {
        if (_unitOfWork.categery.getAll() == null)
        {
            return Problem("Entity set 'Bulky.categeries'  is null.");
        }

        var users = _unitOfWork.categery.getFirstOrDefault(u => u.id == id);
        if (users != null)
        {
            _unitOfWork.categery.remove(users);
            TempData["success"] = "Categery  deleted successfully!";
        }

        _unitOfWork.save();
        return RedirectToAction(nameof(Index));
    }

    private bool categeryExists(double id)
    {
        return (_unitOfWork.categery.getAll()?.Any(e => e.id == id)).GetValueOrDefault();
    }
}

