using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Areas.Identity.Data;
using MovieStore.Models;

namespace MovieStore.Controllers
{
    public class LoginController : Controller
    {     
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET method
        public IActionResult Index()
        {
            List<Movie> movies = _context.Movies.OrderBy(x => x.MovieName).ToList();
            
            foreach(var movie in movies)
            {
                string movieWithoutWhitespace = movie.MovieName.Replace(" ", "_");
                movie.MovieName = movieWithoutWhitespace;
            }

            return View(movies);
        }

        // PUT method (Edit)
        [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_Employee}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var movieFromDB = _context.Movies.Find(id);

            if (movieFromDB == null)
                return NotFound();

            return View(movieFromDB);
        }

        // POST method (Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_Employee}")]
        public IActionResult Edit(Movie movie)
        {
            // Server-side Model Validation
            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                _context.SaveChanges();
                TempData["success"] = $"Movie: {movie.MovieName} updated successfully";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Login");
        }

        // DELETE method
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var movieFromDB = _context.Movies.Find(id);

            if (movieFromDB == null)
                return NotFound();

            _context.Movies.Remove(movieFromDB);
            _context.SaveChanges();

            TempData["success"] = $"Movie: {movieFromDB.MovieName} deleted successfully";
            return RedirectToAction("Index");
        }

        // POST method (Show a form to add a new movie)
        [Authorize(Roles = Roles.Role_Admin)]
        public IActionResult Add()
        {
            return View();
        }

        // POST method (Update)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Roles.Role_Admin}")]
        public IActionResult Add(Movie movie)
        {
            // Server-side Model Validation
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                TempData["success"] = $"Movie: {movie.MovieName} added successfully";
                return RedirectToAction("Index");
            }

            return View(movie);
        }

    }
}
