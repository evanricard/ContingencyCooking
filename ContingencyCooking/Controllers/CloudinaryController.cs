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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ContingencyCooking.Controllers
{

    public class CloudinaryController : Controller
    {
        public ActionResult ImageUpload(HttpPostedFileBase file, string RecipeID, string Difficulty)
        {
            string apiKey = ConfigurationManager.AppSettings["CloudinaryApi"];
            string apiSecret = ConfigurationManager.AppSettings["CloudinarySecret"];
            Account account = new Account("cookingcontingency", apiKey, apiSecret);

            Cloudinary cloudinary = new Cloudinary(account);

            if (file != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.InputStream)
                };
                var apiReturn = cloudinary.Upload(uploadParams);

                JObject jsonData = (JObject)apiReturn.JsonObj;


                ViewBag.PictureURL = jsonData["secure_url"].ToString();
                ViewBag.RecipeID = RecipeID;
                ViewBag.Difficulty = Difficulty;

                ViewBag.Upload = jsonData["secure_url"];


            }
            else
            {
                ViewBag.Upload = "FAIL";
            }

            JObject JsonData = (JObject)Session["Recipe"];
            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeImageURL = JsonData["PhotoUrl"];

            return View("../Home/RateExperience");
        }
    }
}