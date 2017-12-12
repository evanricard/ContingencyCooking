using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ContingencyCooking.Models;
using System.Text;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace ContingencyCooking.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            //string apikey = ConfigurationManager.AppSettings["BigOven"];
            return View();
        }

        //Go to search and start cooking but we need authorization first 
        [Authorize]
        public ActionResult Search()
        {
            return View();
        }


        //Take the user's input for recipes, pass it through the data access layer 
        //and return corresponding view
        public ActionResult SearchRecipes(string input)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.SearchByInput(input);

            ViewBag.Results = JsonData["Results"];

            return View("DisplayRecipes");
        }

        //Take the RecipeID for a given recipe and present them with a choice of difficulty
        //TODO: Clean-up the difficulty page
        public ActionResult SearchById(string RecipeID)
        {
            ViewBag.RecipeID = RecipeID;

            return View("Difficulty");
        }

        public ActionResult ChooseDifficulty3(string RecipeID)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.GetRecipeByID(RecipeID);

            if (Session["Recipe"] == null)
            {
                Session.Add("Recipe", JsonData);
            }
            else
            {
                Session["Recipe"] = JsonData;
            }

            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level3");
        }


        public ActionResult ChooseDifficulty2(string RecipeID)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.GetRecipeByID(RecipeID);

            if (Session["Recipe"] == null)
            {
                Session.Add("Recipe", JsonData);
            }
            else
            {
                Session["Recipe"] = JsonData;
            }

            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            //Goes through instructions, then removes ingredients from instructions
            List<string> ingString = new List<string>();

            foreach (var item in JsonData["Ingredients"])
            {
                ingString.AddRange(item["Name"].ToString().Split(' '));
            }

            string instString = JsonData["Instructions"].ToString();

            StringBuilder sb = new StringBuilder(instString);

            for (int i = 0; i < ingString.Count; i++)
            {
                if (!string.IsNullOrEmpty(ingString[i]))
                {
                    sb.Replace(ingString[i].ToLower(), "_____");

                    sb.Replace(ingString[i][0].ToString().ToUpper() + ingString[i].Substring(1), "_____");

                }
                //instString = instString.Replace(ingString[i], "____");
                //Regex.Replace(sb.ToString(), ingString[i], "____", RegexOptions.IgnoreCase);
            }

            ViewBag.stingsomething = ingString;

            ViewBag.finalInstructions = sb.ToString();

            return View("Level2");
        }

        public ActionResult ChooseDifficulty1(string RecipeID)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.GetRecipeByID(RecipeID);

            if (Session["Recipe"] == null)
            {
                Session.Add("Recipe", JsonData);
            }
            else
            {
                Session["Recipe"] = JsonData;
            }

            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level1");
        }

        //After attempt display read-only info about the recipe (ask user to upload image here)
        public ActionResult RateExperience(string RecipeID, string Difficulty)
        {
            JObject JsonData = (JObject)Session["Recipe"];
            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Difficulty = Difficulty;
            return View("AttemptUpload");
        }

        //View full-page spread of recipe but nothing else
        public ActionResult ViewRecipe(string RecipeID)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.GetRecipeByID(RecipeID);

            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("SeeFullRecipe");
        }

        public ActionResult FrontPage()
        {
            return View();
        }

        //G.R.A.P.H
        //G.reatest R.epresentation of A.DO.NET - P.icturesque H.ealth
        public ActionResult Graph()
        {
            RecipeDBEntities ORM = new RecipeDBEntities();

            var User_ID = User.Identity.GetUserId();

            List<RecipeAttempt> UserList = ORM.RecipeAttempts.Where(x => x.User_ID == User_ID).ToList();

            ViewBag.AllResults = JsonConvert.SerializeObject(ORM.RecipeAttempts.ToList());
            ViewBag.YourResults = JsonConvert.SerializeObject(UserList);
            return View();
        }
    }

}