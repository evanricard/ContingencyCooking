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
    public class ContingencyCookingDAL
    {
        public JObject SearchByInput(string input)
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("https://api2.bigoven.com/recipes?any_kw=" + input + "&isbookmark=0&api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            JObject JsonData = JObject.Parse(data);

            return JsonData;
        }

        public JObject GetRecipeByID(string RecipeID)
        {
            string apiKey = ConfigurationManager.AppSettings["BigOven"];

            HttpWebRequest WR = WebRequest.CreateHttp("http://api2.bigoven.com/recipe/" + RecipeID + "?api_key=" + apiKey);

            WR.Accept = "application/json";

            WR.UserAgent = ".Net Framework Test Client";

            HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();

            StreamReader Reader = new StreamReader(Response.GetResponseStream());

            string data = Reader.ReadToEnd();

            JObject JsonData = JObject.Parse(data);

            return JsonData;
        }

        public List<string> GetUserEmailsFromAttempts(List<RecipeAttempt> UserList)
        {
            ApplicationDbContext UserORM = new ApplicationDbContext();
            List<string> UserEmails = new List<string>();
            foreach (RecipeAttempt attempt in UserList)
            {
                ApplicationUser tempUser = UserORM.Users.Find(attempt.User_ID);
                UserEmails.Add(tempUser.Email);
            }
            return UserEmails;

        }
    }
}