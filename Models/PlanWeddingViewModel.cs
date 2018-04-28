using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;

namespace wedding.Models

{
    public class PlanWeddingViewModel
    {
        [Required(ErrorMessage="Wedder One is required")]
        [Display(Name="Wedder One")]
        [MinLength(2)]
        public string WedderOne { get; set; }
        
        [Required(ErrorMessage="Wedder Two is required")]
        [Display(Name="Wedder Two")]
        [MinLength(2)]
        public string WedderTwo { get; set; }
            
        [Required(ErrorMessage="Date is required")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage="Wedding address is required")]
        [Display(Name="Wedding Address")]
        public string WedAddress { get; set; }
    }
}
