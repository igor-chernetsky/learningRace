using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LR.Data.Providers
{
    public class CategoryProvider : MainProvider
    {
        public List<Category> GetRootCategories()
        {
            List<Category> result = Context.Category.Where(c=> c.Parent == null).ToList();
            return result;
        }

        public List<Category> GetAllCategories(bool withRandomQuestions = false)
        {
            List<Category> result = Context.Category.ToList();
            result.Where(c => c.Parent != null).ToList().
                ForEach(c=> result.First(cc=>cc.Id == c.ParentId).ChildCategories.Add(c));

            if (withRandomQuestions)
            {
                result.ForEach(c => c.RandomQuestions = DataProvider.Questions.GetRandomQuestions(c.Id, 3).
                    Select(q => q.QuestionText).ToList());
            }

            return result;
        }

        public Category GetCategoryById(Guid id, bool initChild = false)
        {
            Category result = Context.Category.FirstOrDefault(c => c.Id == id);
            if (initChild)
            {
                result.ChildCategories = GetChildCategories(result, Context.Category.ToList());
            }
            return result;
        }

        public Guid AddCategory(Category category)
        {
            Context.Category.Add(category);
            Context.SaveChanges();
            return category.Id;
        }

        public void EditCategory(Category category)
        {
            Category categoryToUpdate = GetCategoryById(category.Id);
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.Parent = category.Parent;
            categoryToUpdate.Difficulty = category.Difficulty;
            categoryToUpdate.IsVisible = category.IsVisible;
            categoryToUpdate.Language = category.Language;
            Context.SaveChanges();
        }

        public void DeleteCategory(Guid id)
        {
            Context.Category.Remove(Context.Category.First(c => c.Id == id));
            Context.SaveChanges();
        }

        private List<Category> GetChildCategories(Category target, List<Category> allCategories)
        {
            List<Category> result = new List<Category>();
            result = allCategories.Where(c => c.ParentId == target.Id).ToList();
            result.ForEach(c => c.ChildCategories = GetChildCategories(c, allCategories));

            result.ForEach(c => c.RandomQuestions = DataProvider.Questions.GetRandomQuestions(c.Id, 3).
                    Select(q => q.QuestionText).ToList());
            return result;
        }
    }
}
