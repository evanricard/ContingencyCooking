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
        //Generate a Leaderboard View by grabbing an ApplicationUser from Identity
        //and using that data we popuplate a LeaderboardUser 
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist;
            return View("../Home/Leaderboard");
        }

        //We can then order that data by using simple OrderBy List<T> methods
        public ActionResult OrderByUserName()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderBy(x => x.UserName);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByUserNameDescending()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderByDescending(x => x.UserName);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByTotalAttempts()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderBy(x => x.TotalAttempts);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByTotalAttemptsDescending()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderByDescending(x => x.TotalAttempts);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByAverageDifficulty()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderBy(x => x.AveDifficulty);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByAverageDifficultyDescending()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderByDescending(x => x.AveDifficulty);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByAverageIngredients()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderBy(x => x.AveIngredients);
            return View("../Home/Leaderboard");
        }

        public ActionResult OrderByAverageIngredientsDescending()
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
                temp.AveDifficulty = Math.Round(ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Difficulty).Select(int.Parse).Average(), 3);
                temp.AveIngredients = Math.Round((double)ORM.RecipeAttempts.Where(x => x.User_ID == UserID).Select(x => x.Recipe.Ingredients_Num).Average(), 3);

                LBlist.Add(temp);
            }
            ViewBag.Users = LBlist.OrderByDescending(x => x.AveIngredients);
            return View("../Home/Leaderboard");
        }
    }
}