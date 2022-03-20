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
using Microsoft.AspNetCore.Http;
//dt191g projekt, Av Alice Fagerberg
namespace dt191gProjectApp.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Member")]
        // GET: Bookings
        public async Task<IActionResult> IndexAsync()
        {

            //Bokningsinfo hämtas från session
            string bookMsg = HttpContext.Session.GetString($"{User.Identity.Name} booked");
            string deleteMsg = HttpContext.Session.GetString($"{User.Identity.Name} deleted");

            ViewBag.delete = deleteMsg;
            ViewBag.Message = bookMsg;

            var bookings = await _context.Booking.Include(b => b.Workout).Include(b => b.Workout.TypeOfWorkout).ToListAsync();

            var timeNow = DateTime.Now.ToShortTimeString();
            var today = DateTime.Now.ToString("dddd");
            string capToday = today.Substring(0, 1).ToUpper() + today.Substring(1);


            foreach (var booking in bookings)
            {
                //tar bort bokning på schemat när bokat pass dag och tid har passerat
                if (booking.Workout.DayofWorkout == capToday)
                {
                    if (booking.Workout.Time == timeNow)
                    {
                        _context.Booking.Remove(booking);
                        await _context.SaveChangesAsync();
  
                        return RedirectToAction(nameof(Index));
                    }    

                }
                
            }
                       
            return View(await _context.Workout.Include(w => w.TypeOfWorkout).Include(w => w.Bookings).ToListAsync());
        }


        [Authorize(Roles = "Member")]
        // GET: Member/BookWorkout
        public async Task<IActionResult> BookWorkoutAsync(int? id)
        {
            var workout = await _context.Workout.Include(w => w.TypeOfWorkout).Include(w => w.Bookings)
                .SingleOrDefaultAsync(m => m.WorkoutId == id);

            ViewBag.wName = workout.TypeOfWorkout.TypeName;
            ViewBag.day = workout.DayofWorkout;
            ViewBag.time = workout.Time;

            if(workout.Bookings != null)
            {
                foreach(var booking in workout.Bookings)
                {  
                    if(workout.Bookings.Count == 5)
                    {
                        return RedirectToAction("Index", "Member");
                    }else if(workout.Bookings.Count < 5)
                    {
                        ViewData["WorkoutId"] = workout.WorkoutId;
                        return View();
                    }
                }   
            }
                ViewData["WorkoutId"] = workout.WorkoutId;
                return View();            
        }

        // POST: Member/BookWorkout
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookWorkout([Bind("BookingId,Member,WorkoutId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();

                var Booking = await _context.Booking
                .Include(b => b.Workout).Include(b => b.Workout.TypeOfWorkout)
                .FirstOrDefaultAsync(m => m.BookingId == booking.BookingId);

                string booked = $"{Booking.Workout.TypeOfWorkout.TypeName} kl {Booking.Workout.Time} på {Booking.Workout.DayofWorkout} är bokad";
                HttpContext.Session.SetString($"{User.Identity.Name} booked", booked);
                HttpContext.Session.Remove($"{User.Identity.Name} deleted");

                return RedirectToAction("Index", "Member");
            }
            ViewData["WorkoutId"] = (_context.Workout, "WorkoutId", "TypeName", booking.WorkoutId);
            return View(booking);
        }


        [Authorize(Roles = "Member")]
        // GET: Bookings/Delete/5
        public async Task<IActionResult> DeleteBooking(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Workout).Include(b => b.Workout.TypeOfWorkout)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        [Authorize(Roles = "Member")]
        // POST: Bookings/Delete/5
        [HttpPost, ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);

            var Booking = await _context.Booking
                .Include(b => b.Workout).Include(b => b.Workout.TypeOfWorkout)
                .FirstOrDefaultAsync(m => m.BookingId == booking.BookingId);

            string deleted = $"{Booking.Workout.TypeOfWorkout.TypeName} på {Booking.Workout.DayofWorkout} kl {Booking.Workout.Time} är avbokad";
            HttpContext.Session.SetString($"{User.Identity.Name} deleted", deleted);
            HttpContext.Session.Remove($"{User.Identity.Name} booked");

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Member");
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
