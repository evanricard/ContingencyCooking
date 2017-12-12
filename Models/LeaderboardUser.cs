using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContingencyCooking.Models
{
    public class LeaderboardUser
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int totalAttempts;

        public int TotalAttempts
        {
            get { return totalAttempts; }
            set { totalAttempts = value; }
        }

        private double aveDifficulty;

        public double AveDifficulty
        {
            get { return aveDifficulty; }
            set { aveDifficulty = value; }
        }

        private double aveIngredients;

        public double AveIngredients
        {
            get { return aveIngredients; }
            set { aveIngredients = value; }
        }



    }
}