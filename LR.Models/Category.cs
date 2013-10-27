using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class Category
    {
        [NotMapped]
        private List<Category> childCategories = new List<Category>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public List<Category> ChildCategories
        {
            get
            {
                return childCategories;
            }
            set
            {
                childCategories = value;
            }
        }

        [NotMapped]
        public Guid? ParentId
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.Id;
                }
                return null;
            }
        }

        [NotMapped]
        public List<string> RandomQuestions { get; set; }

        public Category Parent { get; set; }

        public int Difficulty { get; set; }

        public string ImagePath { get; set; }
    }
}
