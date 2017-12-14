using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContingencyCooking.Models
{
    public class LeaderboardRating
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string image;

        public string Image
        {
            get { return image; }
            set { image = value; }
        }

        private string aveRating;

        public string AveRating
        {
            get { return aveRating; }
            set { aveRating = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private int numberOfRatings;

        public int NumberOfRatings
        {
            get { return numberOfRatings; }
            set { numberOfRatings = value; }
        }




    }
}