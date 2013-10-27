using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Model
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Category> ChildId { get; set; }

        [ForeignKey("Category")]
        public Guid? ParentId { get; set; }

        public int Difficulty { get; set; }

        public string ImagePath { get; set; }
    }
}
