using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieModel;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;

namespace MovieProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        
    }
}