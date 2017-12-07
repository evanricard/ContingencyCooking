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

        [Authorize]
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult SearchRecipes(string input)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.SearchByInput(input);

            ViewBag.Results = JsonData["Results"];

            return View("DisplayRecipes");
        }

        public ActionResult SearchById(string RecipeID)
        {

            ViewBag.RecipeID = RecipeID;

            return View("Difficulty");
        }

        public ActionResult ChooseDifficulty3(string RecipeID)
        {
            ContingencyCookingDAL DAL = new ContingencyCookingDAL();

            JObject JsonData = DAL.GetRecipeByID(RecipeID);

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

            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level1");
        }

        public ActionResult RateExperience(string RecipeID, string Difficulty)
        {
            ViewBag.RecipeID = RecipeID;
            ViewBag.Difficulty = Difficulty;
            return View("AttemptUpload");
        }



    }

}