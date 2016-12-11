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
