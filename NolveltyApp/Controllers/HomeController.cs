using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using NolveltyApp.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace NolveltyApp.Controllers
{
    //Home Controller used to direct user based on roles
    public class HomeController : Controller
    {
        ApplicationDbContext context;

        public HomeController()
        {
            //Create database connect.
            context = new ApplicationDbContext();
        }       
        

        public ActionResult Index()
        {

            //Sets up User for correct page according to user role.
            
                if (isAdminUser())
                {
                    
                    return RedirectToAction("Home", "Admin");
                }                     
                     
            return View();
           
        }

        //Checks if the user is an admin user.
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}