using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using DAL.DAL;

namespace DAL.DataManager
{
    public class QuestionManager:MainContext
    {

        public List<QuestionBE> GetQuestionForCategory(Guid categoryId, bool withVariants)
        {
            List<QuestionBE> result = null;
            result = RaceDataContext.Questions.Where(q => q.CategoryId == categoryId).
                Select(q => QuestionToBE(q, GetVariantById(q.RightId), withVariants ? 
                    GetRandomTopVariantsForCategory(categoryId, coutOfVariantsInQuestion, q.RightId) : null)).ToList();
            return result;
        }

        public void AddQuestion(QuestionBE question)
        {
            RaceDataContext.Questions.InsertOnSubmit(BEtoQuestion(question));
            RaceDataContext.SubmitChanges();
        }

        public QuestionBE GetQuestionById(Guid id)
        {
            QuestionBE result = RaceDataContext.Questions.Where(q => q.Id == id).Select(q =>
                QuestionToBE(q, GetVariantById(q.RightId), null)).First();
            return result;
        }

        public void EditQuestion(QuestionBE question)
        {
            Question questionToEdit = RaceDataContext.Questions.First(q => q.Id == question.Id);
            questionToEdit.RightId = question.RightVariant.Id;
            questionToEdit.Question1 = question.Question;
            questionToEdit.CategoryId = question.CategoryId;
            RaceDataContext.SubmitChanges();
        }

        public void DeleteQuestion(Guid questionId)
        {
            RaceDataContext.Questions.DeleteOnSubmit(RaceDataContext.Questions.First(q => q.Id == questionId));
            RaceDataContext.SubmitChanges();
        }

        public List<VariantBE> GetVariantsForCategory(Guid categoryId)
        {
            List<VariantBE> result = RaceDataContext.Variants.Where(v => v.categoryId == categoryId).
                Select(v => VariantToBE(v)).ToList();
            return result;
        }

        public List<VariantBE> GetRandomTopVariantsForCategory(Guid categoryId, int variantCount, Guid rightId)
        {
            Random rnd = new Random();
            List<VariantBE> result = RaceDataContext.Variants.Where(v => v.categoryId == categoryId).AsEnumerable().OrderBy(v=> rnd.Next()).
                Take(variantCount).Select(v => VariantToBE(v)).ToList();
            return result;
        }

        public VariantBE GetVariantById(Guid id)
        {
            VariantBE result = VariantToBE(RaceDataContext.Variants.First(v => v.Id == id));
            return result;
        }

        public void AddVariantToCategory(VariantBE variant)
        {
            Variant v = BEtoVariant(variant);
            RaceDataContext.Variants.InsertOnSubmit(v);
            RaceDataContext.SubmitChanges();
        }

        public void EditVariant(VariantBE variant)
        {
            Variant variantToEdit = RaceDataContext.Variants.First(v => v.Id == variant.Id);
            variantToEdit.Value = variant.Value;
            RaceDataContext.SubmitChanges();
        }

        public void DeleteVariant(Guid variantId)
        {
            RaceDataContext.Variants.DeleteOnSubmit(RaceDataContext.Variants.First(v => v.Id == variantId));
            RaceDataContext.SubmitChanges();
        }

    }
}
