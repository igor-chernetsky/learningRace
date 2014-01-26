using LR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Data.Providers
{
    public class QuestionProvider : MainProvider
    {
        public List<Question> GetQuestionForCategory(Guid categoryId, bool withVariants, bool randomize = false)
        {
            List<Question> result = null;
            result = Context.Questions.Where(q => q.Category.Id == categoryId).ToList();
            if (withVariants)
            {
                result.ForEach(q => q.Variants = GetRandomTopVariantsForQuestion(q.Id, 5));
            }
            result.ForEach(q => q.RightVariant = Context.Variants.FirstOrDefault(v => v.Question.Id == q.Id && v.IsCorrect));
            if (randomize)
            {
                result = result.OrderBy(v => Guid.NewGuid()).ToList();
            }
            return result;
        }

        public void AddQuestion(Question question, List<Variant> variants, Guid categoryId)
        {
            try
            {
                question.Category = Context.Category.First(c => c.Id == categoryId);
                Context.Questions.Add(question);
                Context.SaveChanges();
                variants.ForEach(v => v.Question = question);
                variants.ForEach(v => Context.Variants.Add(v));
                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        sb.Append(string.Format("Property: {0} Error: {1}",
                              validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                throw new Exception(sb.ToString());
            }
        }

        public Question GetQuestionById(Guid id)
        {
            Question result = Context.Questions.FirstOrDefault(q => q.Id == id);
            result.Variants = GetVariantsForQuestion(id);
            result.RightVariant = result.Variants.FirstOrDefault(v => v.IsCorrect);
            return result;
        }

        public List<Question> GetRandomQuestions(Guid categoryId, int questionCount)
        {
            List<Question> result = null;
            result = Context.Questions.Where(q => q.Category.Id == categoryId).AsEnumerable().OrderBy(v => Guid.NewGuid()).Take(questionCount).ToList();
            return result;
        }

        public void EditQuestion(Question question, List<Variant> variants, Guid categoryId)
        {
            Question questionToEdit = GetQuestionById(question.Id);
            questionToEdit.QuestionText = question.QuestionText;
            questionToEdit.Category = Context.Category.First(c => c.Id == categoryId);

            Context.Variants.Where(v => v.Question.Id == question.Id).ToList().ForEach(v=>UpdateVariant(variants, v));

            variants.ForEach(v => v.Question = questionToEdit);
            variants.ForEach(v => Context.Variants.Add(v));
            Context.SaveChanges();
        }

        public void DeleteQuestion(Guid questionId)
        {
            Context.Questions.Remove(GetQuestionById(questionId));
            Context.SaveChanges();
        }

        public List<Variant> GetVariantsForQuestion(Guid questionId)
        {
            List<Variant> result = Context.Variants.Where(v=>v.Question.Id == questionId).ToList();
            return result;
        }

        public List<Variant> GetRandomTopVariantsForQuestion(Guid questionId, int variantCount)
        {
            Variant rightVariant = Context.Variants.First(v => v.Question.Id == questionId && v.IsCorrect);
            List<Variant> result = Context.Variants.Where(v => v.Question.Id == questionId && !v.IsCorrect).AsEnumerable().OrderBy(v => Guid.NewGuid()).
                Take(variantCount).ToList();
            result.Add(rightVariant);
            result = result.AsEnumerable().OrderBy(v => Guid.NewGuid()).ToList();
            return result;
        }

        public Variant GetVariantById(Guid id)
        {
            Variant result = Context.Variants.First(v => v.Id == id);
            return result;
        }

        public void AddVariant(Variant variant)
        {
            Context.Variants.Add(variant);
            Context.SaveChanges();
        }

        public void EditVariant(Variant variant)
        {
            Variant variantToEdit = GetVariantById(variant.Id);
            variantToEdit.Value = variant.Value;
            variantToEdit.Question = variant.Question;
            Context.SaveChanges();
        }

        public void DeleteVariant(Guid variantId)
        {
            Context.Variants.Remove(GetVariantById(variantId));
            Context.SaveChanges();
        }

        #region private_methods

        private void UpdateVariant(List<Variant> newVariants, Variant oldVariant)
        {
            Variant newVariant = newVariants.FirstOrDefault(v => v.Value == oldVariant.Value);
            if (newVariant == null)
            {
                Context.Variants.Remove(oldVariant);
            }
            else
            {
                oldVariant.IsCorrect = newVariant.IsCorrect;
                newVariants.Remove(newVariant);
            }
        }

        #endregion
    }
}
