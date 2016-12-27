using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NolveltyApp.Models
{   // class for Create Award 
    public class CertEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Award Date:")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime awardDate { get; set; }

        //HiddenFor
        [Display(Name = "Create Date:")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime createDate { get; set; }

        [Required]
        [Display(Name = "Employee of the Week or Month?")]
        //note: 0 equals week, 1 equals month
        public int timePeriod { get; set; }

        [Required]
        [Display(Name = "Recipient Email:")]
        [EmailAddress]
        public string recipientEmail { get; set; }

        //recipient's name for display on the pdf
        [Required]
        [Display(Name = "Recipient Name:")]
        public string recipient { get; set; }

        //HiddenFor
        [Display(Name = "Giver Email")]
        public string giverEmail { get; set; }

        //need a foreign key reference (or whatever the ASP.NET MVC version of that is) to the dbo.AspNetUser table for the giver's record
        public string giver { get; set; }

        public bool sendToGiver { get; set; }
    }


}