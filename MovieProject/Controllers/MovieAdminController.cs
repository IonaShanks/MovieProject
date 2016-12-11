using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using MovieModel;
using System;
using System.Collections.Generic;

namespace MovieProject.Controllers
{
    public class MovieAdminController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Movie
        //Shows details on all movies in the database 
        //Includes a filter search for movie names that include the search string, if empty displays all
        //Includes a genre filter which filters by genre, both filters can work together
        public ActionResult Index(string movieGenre, string searchString)
        {
            var GenreLst = new List<string>();

            var GenreQry = from d in db.Movies
                           orderby d.Genre.ToString()
                           select d.Genre.ToString();

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            var movies = from m in db.Movies
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre.ToString() == movieGenre);
            }

            return View(movies);
        }
        // GET: Movie/Details/MovieID
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie Movie = await db.Movies.FindAsync(id);
            if (Movie == null)
            {
                return HttpNotFound();
            }
            return View(Movie);
        }

        // GET: Movie/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: Movie/Add
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "MovieID,Title,Certification,Genre,Description,RunTime")] Movie Movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(Movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Movie);
        }

        // GET: Movie/Edit/MovieID
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie Movie = await db.Movies.FindAsync(id);
            if (Movie == null)
            {
                return HttpNotFound();
            }
            return View(Movie);
        }

        // POST: Movie/Edit/MovieID
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MovieID,Title,Certification,Genre,Description,RunTime")] Movie Movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Movie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Movie);
        }

        // GET: Movie/Delete/MovieID
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie Movie = await db.Movies.FindAsync(id);
            if (Movie == null)
            {
                return HttpNotFound();
            }
            return View(Movie);
        }

        // POST: Movie/Delete/MovieID
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Movie Movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(Movie);
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