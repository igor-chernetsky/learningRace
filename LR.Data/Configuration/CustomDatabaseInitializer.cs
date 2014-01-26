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
        DropCreateDatabaseIfModelChanges<DataContext>
        //CreateDatabaseIfNotExists<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            Category сategory = AddCategory("Английский язык", "Изучаем английский язык", null, context);
            сategory = AddCategory("Основы, словарный запас", "Пополняем словарный запас частоупотребляемыми словами", сategory, context);
            сategory = AddCategory("Звери", "Знакомимся с названиями зверей на английском", сategory, context);
            string[] vars = { "bird", "animal", "fish", "insect" };
            AddQuestion(сategory, context, "Животное", vars, 1);
            vars = new string[4] { "cow", "rat", "duck", "hawk" };
            AddQuestion(сategory, context, "Корова", vars, 0);
            vars = new string[4] { "titmouse", "rat", "humster", "cat" };
            AddQuestion(сategory, context, "Кот", vars, 3);
            vars = new string[4] { "raccoon", "cat", "dog", "deer" };
            AddQuestion(сategory, context, "Собака", vars, 2);
            vars = new string[4] { "horse", "mouse", "parrot", "pig" };
            AddQuestion(сategory, context, "Мышь", vars, 1);
            vars = new string[4] { "gus", "chicken", "tiger", "elk" };
            AddQuestion(сategory, context, "Лось", vars, 3);

            сategory = AddCategory("Russian language", "Study of Russian language", null, context);
            сategory = AddCategory("Basic, vocabulary", "Study comman words", сategory, context);
            сategory = AddCategory("Animals", "Acquainted with the names of animals in Russian", сategory, context);
            vars = new string[4] { "птица", "животное", "рыба", "насекомое" };
            AddQuestion(сategory, context, "Animal", vars, 1);
            vars = new string[4] { "корова", "крыса", "рыба", "лось" };
            AddQuestion(сategory, context, "Cow", vars, 0);
            vars = new string[4] { "кот", "крыса", "попугай", "слон" };
            AddQuestion(сategory, context, "Rat", vars, 1);
            vars = new string[4] { "енот", "кот", "собака", "олень" };
            AddQuestion(сategory, context, "Dog", vars, 2);
            vars = new string[4] { "лошадь", "мышь", "свинья", "зебра" };
            AddQuestion(сategory, context, "Mouse", vars, 1);
            vars = new string[4] { "гусь", "курица", "тигр", "лось" };
            AddQuestion(сategory, context, "Elk", vars, 3);

            сategory = AddCategory("Mathematics", "Science of numbers", null, context);
            сategory = AddCategory("Basic of math", "addition, subtraction, inequality signs", сategory, context);
            vars = new string[4] { "12", "13", "8", "11" };
            AddQuestion(сategory, context, "4 + 7 =", vars, 3);
            vars = new string[4] { "4", "8", "0", "10" };
            AddQuestion(сategory, context, "10 - 5 + 3 =", vars, 1);
            vars = new string[4] { "2", "3", "4", "1" };
            AddQuestion(сategory, context, "11 - 9 =", vars, 0);
            vars = new string[4] { "3", "2", "1", "0" };
            AddQuestion(сategory, context, "1 + 1 + 2 - 3 =", vars, 2);
            vars = new string[4] { "10", "15", "0", "5" };
            AddQuestion(сategory, context, "20 - 10 - 5", vars, 3);
            vars = new string[4] { "1 + 1", "2 + 3", "5 + 5", "1 + 4" };
            AddQuestion(сategory, context, "5 - 3 =", vars, 0);

            сategory = AddCategory("Математика", "Science of numbers", null, context);
            сategory = AddCategory("Основы математики", "сложение, вычитание, знаки неравенства", сategory, context);
            vars = new string[4] { "12", "13", "8", "11" };
            AddQuestion(сategory, context, "4 + 7 =", vars, 3);
            vars = new string[4] { "4", "8", "0", "10" };
            AddQuestion(сategory, context, "10 - 5 + 3 =", vars, 1);
            vars = new string[4] { "2", "3", "4", "1" };
            AddQuestion(сategory, context, "11 - 9 =", vars, 0);
            vars = new string[4] { "3", "2", "1", "0" };
            AddQuestion(сategory, context, "1 + 1 + 2 - 3 =", vars, 2);
            vars = new string[4] { "10", "15", "0", "5" };
            AddQuestion(сategory, context, "20 - 10 - 5", vars, 3);
            vars = new string[4] { "1 + 1", "2 + 3", "5 + 5", "1 + 4" };
            AddQuestion(сategory, context, "5 - 3 =", vars, 0);


            base.Seed(context);
        }

        private Category AddCategory(string name, string description, Category parent, DataContext context)
        {
            Category newCategory = new Category();
            newCategory.Name = name;
            newCategory.Description = description;
            if (parent != null) newCategory.Parent = parent;
            context.Category.Add(newCategory);
            newCategory.IsVisible = true;
            return newCategory;
        }

        private void AddQuestion(Category category, DataContext context, string text, string[] vars, int rightIndex)
        {
            Question question1 = new Question()
            {
                Category = category,
                QuestionText = text
            };
            context.Questions.Add(question1);

            for (int index = 0; index < vars.Count(); index++)
            {
                Variant var1 = new Variant()
                {
                    Value = vars[index],
                    Question = question1,
                    IsCorrect = index == rightIndex
                };
                context.Variants.Add(var1);
            }
        }
    }
}
