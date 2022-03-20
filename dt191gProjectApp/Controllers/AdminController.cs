#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dt191gProjectApp.Data;
using dt191gProjectApp.Models;
using Microsoft.AspNetCore.Authorization;
//dt191g projekt, Av Alice Fagerberg
namespace dt191gProjectApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /////////////////
        /*CRUD GYMPASS */

        [Authorize(Roles = "Admin")]
        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workout.Include(w => w.TypeOfWorkout).Include(w => w.Bookings).ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        // GET: Workouts/AddWorkout
        public IActionResult AddWorkout()
        {
            ViewData["TypeId"] = new SelectList(_context.TypeOfWorkout, "TypeId", "TypeName");
            return View();
        }

        // POST: Workouts/AddWorkout
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddWorkout([Bind("WorkoutId,Trainer,DayofWorkout,Time,TypeId")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.TypeOfWorkout, "TypeId", "TypeName", workout.TypeId);
            return View(workout);
        }

        // GET: Workouts/EditWorkout/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditWorkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var workout = await _context.Workout.FindAsync(id);
            if (workout == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.TypeOfWorkout, "TypeId", "TypeName", workout.TypeId);
            return View(workout);
        }
        // POST: Workouts/EditWorkout/id
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditWorkout(int id, [Bind("WorkoutId,Trainer,DayofWorkout,Time,TypeId")] Workout workout)
        {
            if (id != workout.WorkoutId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.WorkoutId))
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
            ViewData["TypeId"] = new SelectList(_context.TypeOfWorkout, "TypeId", "TypeName", workout.TypeId);
            return View(workout);
        }

        // GET: Workouts/DeleteWorkout/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteWorkout(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workout
                .Include(w => w.TypeOfWorkout)
                .FirstOrDefaultAsync(m => m.WorkoutId == id);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteWorkout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWorkoutConfirmed(int id)
        {
            var workout = await _context.Workout.FindAsync(id);
            _context.Workout.Remove(workout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        /////////////////////////
        /*CRUD TRÄNINGSKONCEPT */

        // GET: Lägg till koncept
        [Authorize(Roles = "Admin")]
        public IActionResult AddType()
        {
            ViewData["TypeId"] = new SelectList(_context.TypeOfWorkout, "TypeId", "TypeName");
            return View();
        }

        // POST: Lägg till koncept
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddType([Bind("TypeId,TypeName,Description")] TypeOfWorkout typeOfWorkout)
        {
            if (ModelState.IsValid)
            {
                _context.Add(typeOfWorkout);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(typeOfWorkout);
        }

        // GET: TypeOfWorkouts/EditType/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfWorkout = await _context.TypeOfWorkout.FindAsync(id);
            if (typeOfWorkout == null)
            {
                return NotFound();
            }
            return View(typeOfWorkout);
        }

        // POST: TypeOfWorkouts/Edit/id
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditType(int id, [Bind("TypeId,TypeName,Description")] TypeOfWorkout typeOfWorkout)
        {
            if (id != typeOfWorkout.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(typeOfWorkout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfWorkoutExists(typeOfWorkout.TypeId))
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
            return View(typeOfWorkout);
        }

        // GET: TypeOfWorkouts/DeleteType/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfWorkout = await _context.TypeOfWorkout
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (typeOfWorkout == null)
            {
                return NotFound();
            }

            return View(typeOfWorkout);
        }

        // POST: TypeOfWorkouts/DeleteType/id
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTypeConfirmed(int id)
        {
            var typeOfWorkout = await _context.TypeOfWorkout.FindAsync(id);
            _context.TypeOfWorkout.Remove(typeOfWorkout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ///////////////////////////////
        // BOOL KONTROLL

        private bool WorkoutExists(int id)
        {
            return _context.Workout.Any(e => e.WorkoutId == id);
        }

        private bool TypeOfWorkoutExists(int id)
        {
            return _context.TypeOfWorkout.Any(e => e.TypeId == id);
        }
    }
}
