using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContingencyCooking.Models;
using Microsoft.AspNet.Identity;
using System.Collections;


namespace ContingencyCooking.Controllers
{
    public class DatabaseController : Controller
    {
        public ActionResult SubmitRecipeAttempt(RecipeAttempt attempt)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            string apiKey = ConfigurationManager.AppSettings["BigOven"];
            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/" + attempt.RecipeID + "?api_key=" + apiKey);
            WR.Accept = "application/json";
            WR.UserAgent = ".Net Framework Test Client";
            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();
            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string data = Reader.ReadToEnd();

            ViewBag.RawJSON = data;

            JObject JsonData = JObject.Parse(data);
            if (ORM.Recipes.Find(attempt.RecipeID) == null)
            {
                Recipe RecipeForDB = new Recipe();

                RecipeForDB.RecipeID = JsonData["RecipeID"].ToString();
                RecipeForDB.Description = JsonData["Description"].ToString();
                RecipeForDB.Ingredients_Num = JsonData["Ingredients"].ToList().Count;
                RecipeForDB.Category = JsonData["Cuisine"].ToString();
                RecipeForDB.Title = JsonData["Title"].ToString();

                ORM.Recipes.Add(RecipeForDB);
            }
            ORM.RecipeAttempts.Add(attempt);
            ORM.SaveChanges();

            return View("../Home/About");
        }

        public ActionResult DisplayUserAttempts(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

<<<<<<< HEAD
=======
            string username = User.Identity.GetUserName().ToString();

            username = username.Substring(0, username.IndexOf("@"));

            ViewBag.Results = UserList;

            ViewBag.Username = username;

            return View("../Home/Results");
        }

        public ActionResult OrderByTitle(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Recipe.Title).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/Results");
        }

        public ActionResult OrderByDifficulty(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Difficulty).Where(x => x.User_ID == User_ID).ToList();

>>>>>>> 10aa3022266d6757f66d84556d2e2792f433e20f
            ViewBag.Results = UserList;

            return View("../Home/Results");
        }

<<<<<<< HEAD
        public ActionResult OrderByTitle(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Recipe.Title).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/Results");
        }

        public ActionResult OrderByDifficulty(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Difficulty).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/Results");
        }

=======
>>>>>>> 10aa3022266d6757f66d84556d2e2792f433e20f
        public ActionResult OrderByRating(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Rating).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/Results");
        }

        public ActionResult DisplayAllAttempts()
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ApplicationDbContext UserORM = new ApplicationDbContext();
            // ApplicationUser user1 = UserORM.Users.Find(/*---pk is needed-----*/);


            List<RecipeAttempt> UserList = ORM.RecipeAttempts.ToList();
            /*List<string> titles = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                titles.Add(attempt.Recipe.Title);
            } */

            List<string> UserEmails = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                ApplicationUser tempUser = UserORM.Users.Find(attempt.User_ID);
                UserEmails.Add(tempUser.Email);
            }

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;

            return View("../Home/AllResults");
        }

    }
}