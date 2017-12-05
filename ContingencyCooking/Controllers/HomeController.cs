﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            string apikey = ConfigurationManager.AppSettings["BigOven"];
            return View();
        }
        public ActionResult Search()
        {
            return View();
        }

        //TODO: Rename
        public ActionResult GetLasagna()
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/1115910?api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            ViewBag.RawJSON = data;

            JObject JsonData = JObject.Parse(data);

            //Data is pulled like this:
            //

            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];

            return View();
        }

        public ActionResult SearchRecipes(string input)
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("https://api2.bigoven.com/recipes?any_kw=" + input + "&isbookmark=0&api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            ViewBag.Data = data;

            JObject JsonData = JObject.Parse(data);

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
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/" + RecipeID + "?api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            ViewBag.RawJSON = data;

            JObject JsonData = JObject.Parse(data);

            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level3");
        }


        public ActionResult ChooseDifficulty2(string RecipeID)
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/" + RecipeID + "?api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            ViewBag.RawJSON = data;

            JObject JsonData = JObject.Parse(data);

            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level2");
        }

        public ActionResult ChooseDifficulty1(string RecipeID)
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/" + RecipeID + "?api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            ViewBag.RawJSON = data;

            JObject JsonData = JObject.Parse(data);

            ViewBag.RecipeID = JsonData["RecipeID"];
            ViewBag.Cuisine = JsonData["Cuisine"];
            ViewBag.Ingredients = JsonData["Ingredients"]; //In view add .(whatever) to pull info and throw into for loop
            ViewBag.Instructions = JsonData["Instructions"];
            ViewBag.Photo = JsonData["PhotoUrl"];

            return View("Level1");
        }

        public ActionResult RateExperience()
        {

            return View();
        }

        //TODO: Rating
        public ActionResult Rating(string rating)
        {
            if (rating == "great")
            {
                return View("About");
            }

            else
            {
                return View("Contact");

            }
        }

    }

}