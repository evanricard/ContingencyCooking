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

namespace ContingencyCooking.Controllers
{
    public class LeaderboardController : Controller
    {
        public ActionResult DisplayLeaderboard()
        {
            RecipeDBEntities ORM = new RecipeDBEntities();
            ApplicationDbContext UserORM = new ApplicationDbContext();


            List<LeaderboardUser> LBlist = new List<LeaderboardUser>();

            List<string> tempList = ORM.RecipeAttempts.Select(x => x.User_ID).Distinct().ToList();
            foreach (string UserID in tempList)
            {
                LeaderboardUser temp = new LeaderboardUser();
                ApplicationUser tempUser = UserORM.Users.Find(UserID);
                temp.UserName = tempUser.Email;
                temp.TotalAttempts = ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Count();
                temp.AveDifficulty = ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average();
                temp.AveIngredients = (double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average();

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist;
            return View("../Home/Leaderboard");
        }

    }
}