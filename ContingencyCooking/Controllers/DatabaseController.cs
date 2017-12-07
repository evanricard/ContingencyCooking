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
using System.Text.RegularExpressions;

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
                // RecipeForDB.Description = JsonData["Description"].ToString();
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
            if (!string.IsNullOrEmpty(User_ID))
            {
                RecipeDBEntities ORM = new RecipeDBEntities();

                List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

                string username = User.Identity.GetUserName().ToString();

                username = username.Substring(0, username.IndexOf("@"));

                ViewBag.Results = UserList;
                ViewBag.Images = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).Select(x => x.image).ToList();
                ViewBag.Username = username;

                return View("../Home/UserProfile");
            }

            return View("../Home/Index");


        }


        public ActionResult OrderByTitle(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Recipe.Title).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
        }

        public ActionResult OrderByDifficulty(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Difficulty).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
        }

        public ActionResult OrderByRating(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Rating).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
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
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }

        public ActionResult SearchByAllAttemptsTitle(string InputTitle)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ApplicationDbContext UserORM = new ApplicationDbContext();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).ToList();

            List<string> UserEmails = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                ApplicationUser tempUser = UserORM.Users.Find(attempt.User_ID);
                UserEmails.Add(tempUser.Email);
            }

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }

        //public ActionResult SearchByAllAttemptsUser()
        //{
        //    RecipeDBEntities ORM = new RecipeDBEntities();
        //}

        public ActionResult SearchByAllAttemptsDifficulty(string InputDifficulty)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            
            ApplicationDbContext UserORM = new ApplicationDbContext();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.Difficulty.Contains(InputDifficulty)).ToList();

            List<string> UserEmails = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                ApplicationUser tempUser = UserORM.Users.Find(attempt.User_ID);
                UserEmails.Add(tempUser.Email);
            }

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }

        public ActionResult SearchByAllAttemptsRating(string InputRating)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            ApplicationDbContext UserORM = new ApplicationDbContext();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.Rating.ToString().Contains(InputRating)).ToList();

            List<string> UserEmails = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                ApplicationUser tempUser = UserORM.Users.Find(attempt.User_ID);
                UserEmails.Add(tempUser.Email);
            }

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }
    }
}