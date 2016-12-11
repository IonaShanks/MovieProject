using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using MovieModel;


namespace MovieProject.Controllers
{
    public class MovieAPIController : ApiController
    {
        private MovieContext db = new MovieContext();

        [Route("Movies/")]
        // GET: Movies
        public IHttpActionResult GetMovies()
        {
            return Ok(db.Movies.OrderBy(m => m.MovieID).ToList());
        }

        // GET: Movies/MovieID
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

        // GET: Movies/TitleSearch/Movieid

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


        // GET: Movies/Genre/Movieid

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

        // GET: Movies/Screenings/Movieid
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