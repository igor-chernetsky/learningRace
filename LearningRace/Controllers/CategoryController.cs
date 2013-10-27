using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LearningRace.Models;
using LearningRace.Models.ModelManager;
using Utils;
using LR.Models;
using LR.Data.Providers;

namespace LearningRace.Controllers
{
    public class CategoryController : Controller
    {
        [Authorize(Roles="admin")]
        public ActionResult Index()
        {
            IEnumerable<Category> categories = DataProvider.Category.GetAllCategories();
            ViewBag.parentId = null;
            return View(categories);
        }
        
        public ActionResult Details(Guid id)
        {
            Category category = DataProvider.Category.GetCategoryById(id);
            ViewBag.Questions = DataProvider.Questions.GetQuestionForCategory(id, false );

            InitializeCategoryImage(category);

            return View(category);
        }

        public ActionResult Edit(Guid? id)
        {
            Category category = id==null ? null : DataProvider.Category.GetCategoryById(id.Value);
            List<Category> allCategories = new List<Category>() { new Category() { Name = "Not selected" } };
            allCategories.AddRange(DataProvider.Category.GetAllCategories());
            ViewBag.Categories = allCategories;

            if (category != null)
            {
                InitializeCategoryImage(category);
            }

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(EditCategoryViewModel model)
        {
            try
            {
                Category newCategroy = CategoryModelManager.GetCategoryFromModel(model);
                if (newCategroy.Id == Guid.Empty)
                {
                    newCategroy.Id = DataProvider.Category.AddCategory(newCategroy);
                }
                else
                {
                    DataProvider.Category.EditCategory(newCategroy);
                }
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var path = Path.Combine(Server.MapPath("~/Images/Categories"), newCategroy.Id.ToString() + ".png");
                    Request.Files[0].SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(Guid id)
        {
            DataProvider.Category.DeleteCategory(id);
            return RedirectToAction("Index");
        }

        #region Questions

        [HttpGet]
        public ActionResult EditQuestion(Guid? id, Guid categoryId)
        {
            Question question = new Question();
            if (id != null)
            {
                question = DataProvider.Questions.GetQuestionById(id.Value);
            }
            question.CategoryId = categoryId;
            return View(question);
        }

        [HttpPost]
        public ActionResult EditQuestion(Guid? id, FormCollection collection)
        {
            Question newQuestion = new Question()
            {
                QuestionText = collection.Get("QuestionText")
            };
            List<Variant> variants = new List<Variant>();
            foreach (string key in collection.AllKeys)
            {
                if (key.Contains("variantValue"))
                {
                    Variant newVariant = new Variant()
                    {
                        Value = collection[key],
                        IsCorrect = collection["correctVariant"] == key
                    };
                    variants.Add(newVariant);
                }
            }
            if (!id.HasValue || id == Guid.Empty)
            {
                DataProvider.Questions.AddQuestion(newQuestion, variants, new Guid(collection.Get("CategoryId")));
            }
            else
            {
                newQuestion.Id = id.Value;
                DataProvider.Questions.EditQuestion(newQuestion, variants, new Guid(collection.Get("CategoryId")));
            }
            return RedirectToAction("Details", new { id = collection.Get("CategoryId") });
        }

        public ActionResult DeleteQuestion(Guid id, Guid categoryId)
        {
            DataProvider.Questions.DeleteQuestion(id);
            return RedirectToAction("Details", new { id = categoryId });
        }

        #endregion

        #region Variant

        [HttpGet]
        public ActionResult EditVariant(Guid? id, Guid questionId)
        {
            Variant variant = new Variant() { Question = DataProvider.Questions.GetQuestionById(questionId) };
            if (id != null)
            {
                variant = DataProvider.Questions.GetVariantById(id.Value);
            }
            return View(variant);
        }

        [HttpPost]
        public ActionResult EditVariant(Guid? id, string questionId, FormCollection collection)
        {
            string question = collection.Get("questionId");
            string variant = collection.Get("variantValue");

            return View();
            //Variant newVariant = new Variant()
            //{
            //    Value = collection.Get("Value"),
            //    Question = DataProvider.Questions.GetQuestionById(new Guid(collection.Get("QuestionId")))
            //};
            //if (id == null || id == Guid.Empty)
            //{
            //    DataProvider.Questions.AddVariant(newVariant);
            //}
            //else
            //{
            //    newVariant.Id = id.Value;
            //    DataProvider.Questions.EditVariant(newVariant);
            //}
            //return RedirectToAction("Details", new { id = collection.Get("CategoryId") });
        }
        
        public ActionResult DeleteVariant(Guid id, Guid questionId)
        {
            DataProvider.Questions.DeleteVariant(id);
            return RedirectToAction("EditQuestion", new { id = questionId });
        }

        #endregion

        #region PrivateMethods

        private void InitializeCategoryImage(Category category)
        {
            string path = Server.MapPath(PathUtil.GetCategoryImagePath(category.Id));
            if (System.IO.File.Exists(path))
            {
                category.ImagePath = Url.Content(PathUtil.GetCategoryImagePath(category.Id));
            }
            else
            {
                category.ImagePath = Url.Content(Constants.defaultCategoryImage);
            }
        }

        #endregion
    }
}
