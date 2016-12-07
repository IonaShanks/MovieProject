using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
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
            if (db.Cinemas.Count() == 0)
            {
                return NotFound();
            }

            else
            {
                return Ok(db.Cinemas.OrderBy(l => l.Name).ToList());       // 200 OK, listings serialized in response body 
            }
        }

        // GET: api/Cinemas/5
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

        // GET: api/Cinemas/Search/5
        [Route("Cinemas/Search/{id}")]
        public IHttpActionResult GetCinemasBySearch(string id)
        {
            string search = id.ToUpper();
            IEnumerable<Cinema> finds = db.Cinemas.Where(c => c.Name.ToUpper().Contains(search)).OrderBy(c => c.Name);
            if (finds.Count() == 0)
            {
                return NotFound();
            }
            return Ok(finds.ToList());
        }

        // PUT: api/Cinemas/5
        [Route("Cinemas/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCinema(string id, Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cinema.CinemaID)
            {
                return BadRequest();
            }

            db.Entry(cinema).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cinemas
        [ResponseType(typeof(Cinema))]
        public IHttpActionResult PostCinema(Cinema cinema)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cinemas.Add(cinema);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CinemaExists(cinema.CinemaID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cinema.CinemaID }, cinema);
        }

        // DELETE: api/Cinemas/5
        [ResponseType(typeof(Cinema))]
        public IHttpActionResult DeleteCinema(string id)
        {
            Cinema cinema = db.Cinemas.Find(id);
            if (cinema == null)
            {
                return NotFound();
            }

            db.Cinemas.Remove(cinema);
            db.SaveChanges();

            return Ok(cinema);
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