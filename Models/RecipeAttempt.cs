//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ContingencyCooking.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class RecipeAttempt
    {
        public int AttemptID { get; set; }
        public string User_ID { get; set; }
        public string RecipeID { get; set; }
        public string Difficulty { get; set; }
        public Nullable<int> Rating { get; set; }
        public string image { get; set; }

        [JsonIgnore]
        public virtual Recipe Recipe { get; set; }
    }
}