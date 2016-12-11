using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MovieModel;
using System;
using System.Linq;

namespace MovieProject.Controllers
{
    public class CinemaAdminController : Controller
    {
        private MovieContext db = new MovieContext();
        // GET: CinemaAdmin
        //Shows details on all cinemas in the database 
        //Includes a filter search for cinema names that include the search string, if empty displays all
        public ActionResult Index(string searchString)
        {
            var cinemas = db.Cinemas.Include(c => c.Movies);

            if (!String.IsNullOrEmpty(searchString))
            {
                cinemas = cinemas.Where(s => s.Name.Contains(searchString));
            }

            return View(cinemas);
        }

        //GET: CinemaAdmin/Details/CinemaID
        //Shows details on a specific Cinema
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await db.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        //POST: CinemaAdmin/Add
        public ActionResult Add()
        {
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title");
            return View();
        }

        // POST: CinemaAdmin/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "CinemaID,Name,Website,PhoneNumber,TicketPrice,MovieID")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                db.Cinemas.Add(cinema);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", cinema.MovieID);
            return View(cinema);
        }

        // GET: CinemaAdmin/Edit/CinemaID
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await db.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", cinema.MovieID);
            return View(cinema);
        }

        // POST: CinemaAdmin/Edit/CinemaID
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CinemaID,Name,Website,PhoneNumber,TicketPrice,MovieID")] Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cinema).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MovieID = new SelectList(db.Movies, "MovieID", "Title", cinema.MovieID);
            return View(cinema);
        }

        // GET: CinemaAdmin/Delete/CinemaID
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = await db.Cinemas.FindAsync(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }
            return View(cinema);
        }

        // POST: CinemaAdmin/Delete/CinemaID
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Cinema cinema = await db.Cinemas.FindAsync(id);
            db.Cinemas.Remove(cinema);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}