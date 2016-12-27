using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NolveltyApp.Models
{  

    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        [Display(Name = "First Name")]
        public string FName { get; set; }
        [Display(Name = "Last Name")]
        public string LName { get; set; }
        [Display(Name = "Date Created")]
        [DataType(DataType.DateTime)]  
        public DateTime DateCreated { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        [Display(Name ="Export To CSV?")]
        public bool YesNoCSV { get; set; }
    }

}