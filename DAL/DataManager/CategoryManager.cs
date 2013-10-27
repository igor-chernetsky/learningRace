using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using System.Data.Linq;
using DAL.DAL;

namespace DAL.DataManager
{
    public class CategoryManager : MainContext
    {
        public List<CategoryBE> GetAllCategories()
        {
            List<CategoryBE> result = RaceDataContext.Categories.Select(c => CategoryToBE(c)).ToList();
            result.ForEach(c => c.ChildId = result.Where(ct => ct.ParentId.HasValue && ct.ParentId.Value == c.Id).ToList());
            return result;
        }

        public CategoryBE GetCategoryById(Guid id)
        {
            CategoryBE result = CategoryToBE(RaceDataContext.Categories.First(c => c.Id == id));
            return result;
        }

        public Guid AddCategory(CategoryBE category)
        {
            Category inputCategory = BEtoCategory(category);
            RaceDataContext.Categories.InsertOnSubmit(inputCategory);
            RaceDataContext.SubmitChanges();
            return inputCategory.Id;
        }

        public void EditCategory(CategoryBE category)
        {
            Category categoryToUpdate = RaceDataContext.Categories.First(c => c.Id == category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.ParentId = category.ParentId;
            categoryToUpdate.Difficulty = category.Difficulty;
            RaceDataContext.SubmitChanges();
        }

        public void DeleteCategory(Guid id)
        {
            RaceDataContext.Categories.DeleteOnSubmit(RaceDataContext.Categories.First(c => c.Id == id));
            RaceDataContext.SubmitChanges();
        }
    }
}
