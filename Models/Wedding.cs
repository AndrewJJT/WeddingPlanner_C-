using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WeddingPlanner.Models{

    public class Wedding{
        public int WeddingId { get; set; }
        public string WedderOne { get; set; }   
        public string WedderTwo {get;set;}

        public DateTime Date { get; set; }

        public String Address { get; set; }

        public int CreatedByUserId { get; set; }

        public List<Attendance> Guests { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}