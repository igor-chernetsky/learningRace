using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils;
using Racing.Core;
using WebMatrix.WebData;
using System.Threading;
using System.Web.Security;
using LearningRace.Models;
using LR.Models;
using LR.Models.RaceModels;
using LR.Data.Providers;

namespace LearningRace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Category> categories = DataProvider.Category.GetAllCategories(true);
            categories.ForEach(c => InitializeCategoryImage(c));
            ViewBag.Categories = categories;
            ViewBag.IsAdmin = User.IsInRole("admin");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [Authorize]
        public ActionResult Game(Guid categoryId)
        {
            ViewBag.questionList = DataProvider.Questions.GetQuestionForCategory(categoryId, true, true);
            ViewBag.categoryId = categoryId;
            RaceModel race = RaceManager.AddRacer(WebSecurity.CurrentUserId, categoryId);

            return View(race);
        }

        #region Race

        [Authorize]
        public JsonResult GetRaceInfo(Guid raceId,int version)
        {
            RaceModel race = RaceManager.GetRaceById(raceId);
            for (int i = 0; i < 3; i++)
            {
                if (race.Version > version)
                    break;
                Thread.Sleep(1000);
            }
            return Json(race, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult RacerReady(Guid raceId, bool isReady)
        {
            RaceManager.ChangeRaceState(raceId, WebSecurity.GetUserId(User.Identity.Name), isReady);
            RaceModel race = RaceManager.GetRaceById(raceId);
            return Json(race, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult SendAnswer(Guid questionId, Guid variantId, Guid raceId)
        {
            Question question = DataProvider.Questions.GetQuestionById(questionId);
            bool result = question.RightVariant.Id == variantId;
            int delta = result ? 15 : -10;
            RaceManager.ChangeSpeed(raceId, WebSecurity.GetUserId(User.Identity.Name), delta);
            RaceModel race = RaceManager.GetRaceById(raceId);
            return Json(new { result = result, rightId = question.RightVariant.Id, race = race }, JsonRequestBehavior.AllowGet);
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
