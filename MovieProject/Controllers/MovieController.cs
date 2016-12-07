using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using MovieModel;
using System;
using System.Collections.Generic;

namespace MovieMVC.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Movie
        //public async Task<ActionResult> Index()
        //{
        //    return View(await db.Movies.ToListAsync());
        //}


        //public ActionResult Index(string title)
        //{
        //    string searchString = title;
        //    var movies = from m in db.Movies
        //                 select m;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        movies = movies.Where(s => s.Title.Contains(searchString));
        //    }

        //    return View(movies);
        //}


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
        // GET: Movie/Details/5
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

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MovieID,Title,Certification,Genre,Description,RunTime")] Movie Movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(Movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(Movie);
        }

        // GET: Movie/Edit/5
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

        // POST: Movie/Edit/5
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

        // GET: Movie/Delete/5
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

        // POST: Movie/Delete/5
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
