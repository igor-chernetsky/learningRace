using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class CategoryBE
    {
        #region publicProperties

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }

        public List<CategoryBE> ChildId { get; set; }

        public Guid? ParentId { get; set; }

        public int Difficulty { get; set; }

        public string ImagePath { get; set; }

        #endregion

    }
}
