using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.DAL;
using Entities;

namespace DAL.DataManager
{
    public class MainContext
    {
        private LearningRaceDALDataContext dataContext;
        protected readonly int coutOfVariantsInQuestion = 4;

        public LearningRaceDALDataContext RaceDataContext
        {
            get
            {
                if (dataContext == null)
                {
                    string conString = System.Configuration.ConfigurationSettings.AppSettings["LearningRaceConnectionString"];
                    dataContext = new LearningRaceDALDataContext(conString);
                }
                return dataContext;
            }
        }

        protected CategoryBE CategoryToBE(Category cat)
        {
            CategoryBE result = new CategoryBE()
            {
                Id = cat.Id,
                Name = cat.Name.Trim(),
                Description = cat.Description == null ? null : cat.Description.Trim(),
                ParentId = cat.ParentId,
                Difficulty = cat.Difficulty.HasValue ? cat.Difficulty.Value : 0
            };
            return result;
        }

        protected Category BEtoCategory(CategoryBE cat)
        {
            Category result = new Category()
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                ParentId = cat.ParentId,
                Difficulty = cat.Difficulty
            };
            return result;
        }

        protected QuestionBE QuestionToBE(Question qu, VariantBE variant, List<VariantBE> variantsList = null)
        {
            if (variantsList != null && variantsList.FirstOrDefault(v => v.Id == variant.Id) == null)
            {
                Random rnd = new Random();
                variantsList[rnd.Next(coutOfVariantsInQuestion)] = variant;
            }
            QuestionBE result = new QuestionBE()
            {
                Id = qu.Id,
                Question = qu.Question1.Trim(),
                RightVariant = variant,
                CategoryId = qu.CategoryId,
                Variants = variantsList
            };
            return result;
        }

        protected Question BEtoQuestion(QuestionBE question)
        {
            Question result = new Question()
            {
                Id = question.Id,
                RightId = question.RightVariant.Id,
                CategoryId = question.CategoryId,
                Question1 = question.Question 
            };
            return result;
        }

        protected VariantBE VariantToBE(Variant variant)
        {
            VariantBE result = new VariantBE()
            {
                Id = variant.Id,
                Value = variant.Value,
                CategoryId = variant.categoryId 
            };
            return result;
        }

        protected Variant BEtoVariant(VariantBE variant)
        {
            Variant result = new Variant()
            {
                Id = variant.Id,
                Value = variant.Value,
                categoryId = variant.CategoryId
            };
            return result;
        }
    }
}
