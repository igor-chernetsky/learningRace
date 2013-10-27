using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LearningRace.Models
{
    public class EditCategoryViewModel
    {
        [HiddenInput(DisplayValue=false)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name="Description")]
        public string Description { get; set; }

        [Display(Name="Parent Category")]
        public Guid? ParentId { get; set; }

        [Display(Name = "Category Difficulty")]
        public int Difficulty { get; set; }
    }
}