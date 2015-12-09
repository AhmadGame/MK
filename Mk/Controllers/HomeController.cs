using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mk.Mappers;
using Mk.Stores;
using Mk.ViewModels;

namespace Mk.Controllers
{
    public class HomeController : Controller
    {
        private const string CookieKeyEmail = "email";
        private const string CookiewKeyAdmin = "admin";
        public ActionResult Index()
        {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(CookieKeyEmail))
            {
                return RedirectToAction("Login");
            }
            var store = new FolderStore();
            var model = new FolderListViewModel
            {
                languages = store.All().Select(f => f.Language).Distinct().ToList()
            };
            return View(model);
        }

        public ActionResult Folders(string language)
        {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(CookieKeyEmail))
            {
                return RedirectToAction("Login");
            }
            var mapper = new FolderMapper(new QuestionMapper());
            var store = new FolderStore();
            var folders = store.GetByLanguage(language);
            var viewModel = new FolderListViewModel
            {
                folders = folders.Select(f => mapper.ToModel(f)).ToList()
            };

            return View(viewModel);
        }

        public ActionResult Folder(FolderViewModel folderModel)
        {
            var mapper = new FolderMapper(new QuestionMapper());
            var folder = mapper.FromModel(folderModel);
            var store = new QuestionStore();
            folder.Questions = store.GetByFolderId(folder.Id);

            return View(mapper.ToModel(folder));
        }

        [HttpPost]
        public ActionResult Correct(FolderViewModel folder)
        {
            var model = folder;
            foreach (var question in model.questions)
            {
                if (question.selected == 1)
                {
                    question.userAnswer = question.answer1;
                    question.result = question.explain1;
                }
                else if (question.selected == 2)
                {
                    question.userAnswer = question.answer2;
                    question.result = question.explain2;
                }
                else if (question.selected == 3)
                {
                    question.userAnswer = question.answer3;
                    question.result = question.explain3;
                }
                else if (question.selected == 4)
                {
                    question.userAnswer = question.answer4;
                    question.result = question.explain4;
                }

                if (string.IsNullOrEmpty(question.result))
                {
                    question.result = "Incorrect";
                }
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewData["Message"] = @"Made by Ahmad Game";

            return View();
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpGet]
        public ActionResult Login(string error)
        {
            return View(new AccountViewModel { error = error });
        }

        [HttpPost]
        public ActionResult Login(AccountViewModel account)
        {
            if (string.IsNullOrEmpty(account.password))
            {
                return RedirectToAction("Login", "Home", new { error = "Password cannot be empty" });
            }
            if (string.IsNullOrEmpty(account.email))
            {
                return RedirectToAction("Login", "Home", new { error = "Email cannot be empty" });
            }
            var store = new AccountStore();
            try
            {
                var mapper = new AccountMapper();
                var model = mapper.ToModel(store.GetByEmail(account.email));
                if (model.password != account.password)
                {
                    return RedirectToAction("Login", "Home", new { error = "Incorrect password, please try again" });
                }
                account = model;
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { error = "Account was not found, please try again" });
            }

            var emailCookie = new HttpCookie(CookieKeyEmail, account.email) {Expires = DateTime.Now.AddHours(4)};
            HttpContext.Response.Cookies.Add(emailCookie);
            if (account.isAdmin)
            {
                HttpContext.Response.Cookies.Add(new HttpCookie(CookiewKeyAdmin, account.isAdmin.ToString())
                {
                    Expires = DateTime.Now.AddHours(4)
                });
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult SignOut()
        {
            HttpContext.Response.Cookies.Clear();
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Admin()
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public ActionResult AddAccount()
        {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(CookiewKeyAdmin))
            {
                return RedirectToAction("Login");
            }
            return View(new AccountViewModel());
        }

        [HttpPost]
        public ActionResult AddAccount(AccountViewModel account)
        {
            var store = new AccountStore();
            var mapper = new AccountMapper();
            store.Add(mapper.FromModel(account));
            return RedirectToAction("Admin");
        }
        [HttpGet]
        public ActionResult AddQuestion()
        {
            if (!HttpContext.Request.Cookies.AllKeys.Contains(CookiewKeyAdmin))
            {
                return RedirectToAction("Login");
            }
            return View(new QuestionIoModel());
        }
        [HttpPost]
        public ActionResult AddQuestion(QuestionIoModel question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }
            var folderStore = new FolderStore();
            var folderId = folderStore.GetFolderToAddQuestion(question.language);
            var mapper = new QuestionMapper();
            var questionStore = new QuestionStore();
            questionStore.Add(folderId, mapper.ToModel(question));
            return RedirectToAction("Admin");
        }
    }
}