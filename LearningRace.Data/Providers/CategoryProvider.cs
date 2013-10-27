using LR.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LR.Data.Providers
{
    public class CategoryProvider : MainProvider
    {
        public List<Category> GetAllCategories()
        {
            return Context.Category.ToList();
        }
    }
}
