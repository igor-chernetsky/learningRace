using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LR.Models;
using LR.Data.Providers;

namespace LearningRace.Models.ModelManager
{
    public class CategoryModelManager
    {
        public static Category GetCategoryFromModel(EditCategoryViewModel model)
        {
            Category result = new Category()
            {
                Id = model.Id,
                Name = model.Name,
                Parent = model.ParentId == Guid.Empty ? null : DataProvider.Category.GetCategoryById(model.ParentId.Value),
                Difficulty = model.Difficulty
            };
            return result;
        }
    }
}