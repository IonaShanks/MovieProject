using System;
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
    public class MoviesController : ApiController
    {
        private MovieContext db = new MovieContext();

        [Route("Movies/")]
        // GET: api/Movies
        public IHttpActionResult GetMovies()
        {
            return Ok(db.Movies.OrderBy(m => m.MovieID).ToList());
        }

        [Route("Movies/{id}")]
        [ResponseType(typeof(Movie))]
        public IHttpActionResult GetMovie(string id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        // GET: api/Movies/TitleSearch/id

        [Route("Movies/TitleSearch/{id}")]
        public IHttpActionResult GetMoviesBySearchTerm(string id)
        {
            string search = id.ToUpper();
            IEnumerable<Movie> finds = db.Movies.Where(m => m.Title.ToUpper().Contains(search)).OrderBy(m => m.Title);
            if (finds == null)
            {
                return NotFound();
            }
            return Ok(finds);
        }


        // GET: api/Movies/Genre/id

        [Route("Movies/Genre/{id}")]
        public IHttpActionResult GetMoviesByGenre(string id)
        {
            Genre g = (Genre)(Int32.Parse(id));
            IEnumerable<Movie> gens = db.Movies.Where(m => m.Genre == g).OrderBy(m => m.Title);
            if (gens == null)
            {
                return NotFound();
            }
            return Ok(gens);
        }

        // GET: api/Movies/Screenings/id
        [Route("Movies/Screenings/{id}")]
        public IHttpActionResult GetMovieScreenings(string id)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            IEnumerable<Cinema> screenings = movie.Cinemas.OrderBy(l => l.Name);
            return Ok(screenings);
        }

        //// PUT: api/Movies/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutMovie(string id, Movie movie)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != movie.MovieID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(movie).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MovieExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Movies
        //[ResponseType(typeof(Movie))]
        //public IHttpActionResult PostMovie(Movie movie)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Movies.Add(movie);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (MovieExists(movie.MovieID))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = movie.MovieID }, movie);
        //}

        //// DELETE: api/Movies/5
        //[ResponseType(typeof(Movie))]
        //public IHttpActionResult DeleteMovie(string id)
        //{
        //    Movie movie = db.Movies.Find(id);
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Movies.Remove(movie);
        //    db.SaveChanges();

        //    return Ok(movie);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(string id)
        {
            return db.Movies.Count(e => e.MovieID == id) > 0;
        }
    }
}