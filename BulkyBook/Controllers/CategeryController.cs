using BulkyBook.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Controllers;

public class CategeryController : Controller
{
    private static int id = 1;
    private readonly ApplecationDbContext _db;

    public CategeryController(ApplecationDbContext db)
    {
        _db = db;
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Categery> categery = from VAR in _db.categeries
            select VAR;

        //var categerys = _db.categeries.ToList();
        return View(categery);
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
            categery.id = id++;
            _db.categeries.Add(categery);
            _db.SaveChanges();
            TempData["success"] = "Categery  Created successfully!";
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

        var categeryInDataBase = _db.categeries.Find(id);
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
                _db.Update(categery);
                TempData["success"] = "Categery  updated successfully!";
                _db.SaveChangesAsync();
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
        if (id == null || _db.categeries == null)
        {
            return NotFound();
        }

        var categeries = _db.categeries.Find(id);
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
        if (_db.categeries == null)
        {
            return Problem("Entity set 'Bulky.categeries'  is null.");
        }

        var users = _db.categeries.Find(id);
        if (users != null)
        {
            _db.categeries.Remove(users);
            TempData["success"] = "Categery  deleted successfully!";
        }

        _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool categeryExists(double id)
    {
        return (_db.categeries?.Any(e => e.id == id)).GetValueOrDefault();
    }
}