﻿using Newtonsoft.Json.Linq;
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
            return RedirectToAction("DisplayUserAttempts", new { User_ID = attempt.User_ID });
        }

        [Authorize]
        public ActionResult DisplayUserAttempts(string User_ID)
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            string username = User.Identity.GetUserName().ToString();

            username = username.Substring(0, username.IndexOf("@"));

            ViewBag.Results = UserList;

            ViewBag.Username = username;

            return View("../Home/UserProfile");
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
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();
            // ApplicationUser user1 = UserORM.Users.Find(/*---pk is needed-----*/);


            List<RecipeAttempt> UserList = ORM.RecipeAttempts.ToList();
            /*List<string> titles = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                titles.Add(attempt.Recipe.Title);
            } */

            List<string> UserEmails = DAL.GetUserEmailsFromAttempts(UserList);

            ViewBag.Emails = UserEmails;
            ViewBag.Results = UserList;
            ViewBag.ListDifficulty = ORM.RecipeAttempts.Select(y => y.Difficulty).Distinct().ToList();
            ViewBag.ListRating = ORM.RecipeAttempts.Select(y => y.Rating).Distinct().ToList();

            return View("../Home/AllResults");
        }

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