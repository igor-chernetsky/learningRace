using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string QuestionText { get; set; }

        [Required]
        public Category Category { get; set; }

        [NotMapped]
        public Guid? CategoryId { get; set; }

        [NotMapped]
        public List<Variant> Variants { get; set; }

        [NotMapped]
        public Variant RightVariant { get; set; }
    }
}
