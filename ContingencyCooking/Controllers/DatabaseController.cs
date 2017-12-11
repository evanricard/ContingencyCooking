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
using System.Diagnostics;

namespace ContingencyCooking.Controllers
{
    public class DatabaseController : Controller
    {
        //Submit a user's attempt to RECIPEDB 
        public ActionResult SubmitRecipeAttempt(RecipeAttempt attempt)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            try
            {
                if (ORM.Recipes.Find(attempt.RecipeID) == null)
                {
                    JObject JsonData = (JObject)Session["Recipe"];

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

                Session["Recipe"] = null;

                //TODO:
                return RedirectToAction("DisplayUserAttempts", new { User_ID = attempt.User_ID });
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return View("../Home/ErrorPage");
            }
        }

        //Look at profile info but only if user is logged in 
        //TODO: Add graph data and the ability to view a profile picture
        [Authorize]
        public ActionResult DisplayUserAttempts()
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            //Entity
            var User_ID = User.Identity.GetUserId();

            //-Get the initial portion of a username from recipe 
            //attempts in RECIPEDB
            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            string username = User.Identity.GetUserName().ToString();

            username = username.Substring(0, username.IndexOf("@"));

            ViewBag.Results = UserList;

            ViewBag.Username = username;

            return View("../Home/UserProfile");
        }

        //Order profile by Title
        public ActionResult OrderByTitle(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Recipe.Title).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
        }

        //Order profile by Difficulty
        public ActionResult OrderByDifficulty(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Difficulty).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
        }

        //Order profile by Rating
        public ActionResult OrderByRating(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Rating).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            return View("../Home/UserProfile");
        }

        //Get and display a list of distinct recipes 
        public ActionResult DisplayAllAttempts()
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.ToList();

            List<string> UserEmails = DAL.GetUserEmailsFromAttempts(UserList);

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }


        //TODO: Add complexity to selection system
        //Search all attempts by title, difficulty, or rating
        public ActionResult SearchAllAttempts(string InputTitle, string InputDifficulty, string InputRating)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();
            List<RecipeAttempt> UserList = new List<RecipeAttempt>();

            if (InputTitle != null && InputDifficulty == null && InputRating == null)
            {
                UserList = (ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).ToList());
            }
            else if (InputTitle == null && InputDifficulty != null && InputRating == null)
            {
                UserList = ORM.RecipeAttempts.Where(x => x.Difficulty.Contains(InputDifficulty)).ToList();
            }
            else if (InputTitle == null && InputDifficulty == null && InputRating != null)
            {
                UserList = ORM.RecipeAttempts.Where(x => x.Rating.ToString().Contains(InputRating)).ToList();
            }

            List<string> UserEmails = DAL.GetUserEmailsFromAttempts(UserList);

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }
    }
}