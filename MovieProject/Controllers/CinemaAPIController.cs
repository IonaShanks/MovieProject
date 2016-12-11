using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MovieModel;

namespace prototypeMovieAPI.Controllers
{
    public class CinemasController : ApiController
    {
        private MovieContext db = new MovieContext();

        // GET: api/Cinemas
        [Route("Cinemas/")]
        public IHttpActionResult GetAllCinemas()
        {
            return Ok(db.Cinemas.OrderBy(l => l.Name).ToList());       // 200 OK, listing ordered alphabetically 
        }

        // GET: api/Cinemas/CinemaID
        [Route("Cinemas/{id}")]
        [ResponseType(typeof(Cinema))]
        public IHttpActionResult GetCinema(string id)
        {
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound();
            }

            return Ok(cinema);
        }

        // GET: api/Cinemas/Search/CinemaID
        [Route("Cinemas/Search/{id}")]
        public IHttpActionResult GetCinemasBySearch(string id)
        {
            string search = id.ToUpper();
            IEnumerable<Cinema> finds = db.Cinemas.Where(c => c.Name.ToUpper().Contains(search)).OrderBy(c => c.Name);
            if (finds == null)
            {
                return NotFound();
            }
            return Ok(finds.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CinemaExists(string id)
        {
            return db.Cinemas.Count(e => e.CinemaID == id) > 0;
        }
    }
}