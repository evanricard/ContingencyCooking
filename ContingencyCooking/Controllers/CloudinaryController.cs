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
                    //Can specify a variety of upload parameters here to prep it before we upload
                    //it to Cloudinary
                    File = new FileDescription(file.FileName, file.InputStream)
                };

                //ImageUploadResult 
                var apiReturn = cloudinary.Upload(uploadParams);

                JObject jsonData = (JObject)apiReturn.JsonObj;


                ViewBag.PictureURL = jsonData["secure_url"].ToString();
                ViewBag.RecipeID = RecipeID;
                ViewBag.Difficulty = Difficulty;

                ViewBag.Upload = jsonData["secure_url"];


            }

            //Logging a failed upload
            else
            {
                ViewBag.Upload = "FAIL";
            }

            //Utilizing session data for our recipe title 
            JObject JsonData = (JObject)Session["Recipe"];
            ViewBag.RecipeName = JsonData["Title"];
            ViewBag.RecipeImageURL = JsonData["PhotoUrl"];

            return View("../Home/RateExperience");
        }

        public ActionResult ProfileImage(HttpPostedFileBase file)
        {

            string apiKey = ConfigurationManager.AppSettings["CloudinaryApi"];
            string apiSecret = ConfigurationManager.AppSettings["CloudinarySecret"];
            Account account = new Account("cookingcontingency", apiKey, apiSecret);

            Cloudinary cloudinary = new Cloudinary(account);

            if (file != null)
            {
                var uploadParams = new ImageUploadParams
                {
                    //Can specify a variety of upload parameters here to prep it before we upload
                    //it to Cloudinary
                    File = new FileDescription(file.FileName, file.InputStream)
                };

                //ImageUploadResult 
                var apiReturn = cloudinary.Upload(uploadParams);

                JObject jsonData = (JObject)apiReturn.JsonObj;

                //Identity DBContext
                ApplicationDbContext UserORM = new ApplicationDbContext();

                ApplicationUser applicationUser = new ApplicationUser
                {
                    ProfilePic = jsonData["secure_url"].ToString()
                };

                ViewBag.PictureURL = jsonData["secure_url"].ToString();

                ViewBag.Upload = jsonData["secure_url"];

            }

            //Logging a failed upload
            else
            {
                ViewBag.Upload = "FAIL";
            }


            return View("../Home/RateExperience");
        }
    }
}