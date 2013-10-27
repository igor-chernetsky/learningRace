using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public class VariantBE
    {
        public string Value { get; set; }

        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }
    }
}
