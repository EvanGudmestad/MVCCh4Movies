using Microsoft.AspNetCore.Mvc;
using MovieProject.Models;

namespace MovieProject.Controllers
{
    public class MovieController : Controller
    {
        private MovieContext Context { get; set; }

        public MovieController(MovieContext ctx)
        {
            Context = ctx;
        }

        [HttpGet]
        //id parameter is sent from the URL
        public IActionResult Delete(int id)
        {
            var movie = Context.Movies.Find(id);
            return View(movie);
        }

        [HttpPost]
        public IActionResult Delete(Movie movie)
        {
            Context.Movies.Remove(movie);
            Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add New Movie";
            ViewBag.Genres = Context.Genres.OrderBy(g => g.Name).ToList();
            return View("Edit", new Movie());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit Movie";
            ViewBag.Genres = Context.Genres.OrderBy(g => g.Name).ToList();
            //LINQ queery to find the movie with the given id - PK search
            var movie = Context.Movies.Find(id);
            return View(movie);
        }

        [HttpPost]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                //Either add a new movie or edit a movie
                if(movie.MovieId == 0)
                {
                    Context.Movies.Add(movie);
                }
                else
                {
                    Context.Movies.Update(movie);
                }
                Context.SaveChanges();
                return RedirectToAction("Index", "Home");
               
            }
            else
            {
                //Show our validation errors
                ViewBag.Action = (movie.MovieId == 0) ? "Add" : "Edit";
                ViewBag.Genres = Context.Genres.OrderBy(g => g.Name).ToList();
                return View(movie);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
