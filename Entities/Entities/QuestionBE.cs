using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class QuestionBE
    {
        #region publicProperties

        public Guid Id { get; set; }

        public string Question { get; set; }

        public VariantBE RightVariant { get; set; }

        public Guid CategoryId { get; set; }

        public List<VariantBE> Variants { get; set; }

        #endregion
    }
}
