using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using MovieModel;
using System;
using System.Linq;

namespace MovieMVC.Controllers
{
    public class CinemaController : Controller
    {
        private MovieContext db = new MovieContext();

        // GET: Cinema
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

        // GET: Cinema/Details/CinemaID
        //Shows details on a specific cinema
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
