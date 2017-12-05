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
            if (ORM.Recipes.Find(attempt.RecipeID)==null)
            {
                Recipe RecipeForDB = new Recipe();

                RecipeForDB.RecipeID = JsonData["RecipeID"].ToString();
                RecipeForDB.Description = JsonData["Description"].ToString();
                RecipeForDB.Ingredients_Num = JsonData["Ingredients"].ToList().Count;
                RecipeForDB.Category = JsonData["Description"].ToString();
                RecipeForDB.Title = JsonData["Title"].ToString();

                ORM.Recipes.Add(RecipeForDB);
            }
            ORM.RecipeAttempts.Add(attempt);
            ORM.SaveChanges();

            return View("../Home/About");
        }

    }
}