using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using LR.Models;

namespace LR.Data.Configuration
{
    public class CustomDatabaseInitializer : 
        //DropCreateDatabaseAlways<DataContext> 
        //DropCreateDatabaseIfModelChanges<DataContext>
         CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            Category language = new Category();
            language.Name = "Language";
            context.Category.Add(language);

            Category math = new Category();
            math.Name = "Math";
            context.Category.Add(math);

            Category algebra = new Category();
            algebra.Name = "Algebra";
            algebra.Description = "Deeper into ariphmetics";
            algebra.Parent = math;
            context.Category.Add(math);

            Question question1 = new Question()
            {
                Category = algebra,
                QuestionText = "sin(30)"
            };
            context.Questions.Add(question1);

            string[] vars = {"1/2", "1", "2"};
            AddVariants(context, question1, vars, 0);

            base.Seed(context);
        }

        private void AddVariants(DataContext context, Question question, string[] vars, int rightId)
        {
            for(int index =0; index<vars.Count();index++)
            {
                Variant var1 = new Variant()
                {
                    Value = vars[index],
                    Question = question,
                    IsCorrect = index == rightId
                };
                context.Variants.Add(var1);
            }
        }
    }
}
