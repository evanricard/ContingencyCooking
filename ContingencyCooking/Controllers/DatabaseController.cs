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
using Newtonsoft.Json;
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
                if (ORM.Recipes.Find(attempt.RecipeID) == null) //happens if no recipe exists. only for first time entries.
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

                return RedirectToAction("DisplayUserAttempts", new { User_ID = attempt.User_ID });

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return View("../Home/ErrorPage");
            }
        }

        //Look at profile info but only if user is logged in 
        //TODO: Add the ability to view a profile picture
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

            List<RecipeAttempt> YourList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            ViewBag.AllResults = JsonConvert.SerializeObject(ORM.RecipeAttempts.ToList());
            ViewBag.YourResults = JsonConvert.SerializeObject(YourList);

            return View("../Home/UserProfile");
        }

        //Order profile by Title
        public ActionResult OrderByTitle(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Recipe.Title).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            List<RecipeAttempt> YourList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            ViewBag.AllResults = JsonConvert.SerializeObject(ORM.RecipeAttempts.ToList());
            ViewBag.YourResults = JsonConvert.SerializeObject(YourList);

            return View("../Home/UserProfile");
        }

        //Order profile by Difficulty
        public ActionResult OrderByDifficulty(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Difficulty).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            List<RecipeAttempt> YourList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            ViewBag.AllResults = JsonConvert.SerializeObject(ORM.RecipeAttempts.ToList());
            ViewBag.YourResults = JsonConvert.SerializeObject(YourList);

            return View("../Home/UserProfile");
        }

        //Order profile by Rating
        public ActionResult OrderByRating(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.OrderBy(w => w.Rating).Where(x => x.User_ID == User_ID).ToList();

            ViewBag.Results = UserList;

            List<RecipeAttempt> YourList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            ViewBag.AllResults = JsonConvert.SerializeObject(ORM.RecipeAttempts.ToList());
            ViewBag.YourResults = JsonConvert.SerializeObject(YourList);

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

            string[] splitInput = InputTitle.Split(' ');

            if (InputTitle != null && InputDifficulty == null && InputRating == null)
            {
                for (int i = 0; i < splitInput.Length; i++)
                {
                    if (InputTitle.Contains(splitInput[i]))
                    {
                        var temp = splitInput[i];
                        UserList = ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(temp.ToString())).ToList();
                    }
                }

                //UserList = (from p in ORM.RecipeAttempts where splitInput.Any(val => p.Recipe.Title.Contains(val)) select p).ToList();
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

        public ActionResult AdvancedSearch(string InputTitle, string InputDifficulty, string InputRating)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();
            List<RecipeAttempt> UserList = new List<RecipeAttempt>();

            if (InputTitle != null && InputDifficulty != null && InputRating != null) //111
            {
                UserList = ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).ToList().Where(x => x.Difficulty.Contains(InputDifficulty)).ToList().Where(x => x.Rating.ToString().Contains(InputRating)).ToList();
            }
            if (InputTitle != null && InputDifficulty != null && InputRating == null) //110
            {
                UserList = (ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).Where(x => x.Difficulty.Contains(InputDifficulty)).ToList());
            }
            if (InputTitle != null && InputDifficulty == null && InputRating == null) //100
            {
                UserList = (ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).ToList());
            }
            if (InputTitle != null && InputDifficulty == null && InputRating != null) //101
            {
                UserList = (ORM.RecipeAttempts.Where(x => x.Recipe.Title.ToLower().Contains(InputTitle.ToLower())).Where(x => x.Rating.ToString().Contains(InputRating)).ToList());
            }
            if (InputTitle == null && InputDifficulty != null && InputRating != null) //011
            {
                UserList = ORM.RecipeAttempts.Where(x => x.Difficulty.Contains(InputDifficulty)).Where(x => x.Rating.ToString().Contains(InputRating)).ToList();
            }
            if (InputTitle == null && InputDifficulty != null && InputRating == null) //010
            {
                UserList = ORM.RecipeAttempts.Where(x => x.Difficulty.Contains(InputDifficulty)).ToList();
            }
            if (InputTitle == null && InputDifficulty == null && InputRating != null) //001
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
        [Authorize]
        public ActionResult SaveUserRating(int AttemptID, int Value)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<Rating> RatingList = ORM.Ratings.Where(x => x.AttemptID == AttemptID).ToList();
            List<string> UserList = ORM.Ratings.Where(x => x.AttemptID == AttemptID).Select(x => x.User_ID).ToList();

            if (UserList.Contains(User.Identity.GetUserId()))
            {
                Rating update = RatingList.FirstOrDefault(x => x.User_ID == User.Identity.GetUserId());
                update.Rating1 = Value;
                ORM.Entry(update).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                Rating temp = new Rating();
                temp.Rating1 = Value;
                temp.User_ID = User.Identity.GetUserId();
                temp.AttemptID = AttemptID;
                ORM.Ratings.Add(temp);
            }


            ORM.SaveChanges();


            return RedirectToAction("DisplayAllAttempts");
        }
    }
}