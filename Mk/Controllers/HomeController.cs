using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
            var languages = store.All().Select(f => f.Language).Distinct().Where(l => !string.IsNullOrEmpty(l)).ToList();
            var model = new FolderListViewModel
            {
                languages = languages
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
            foreach (HttpCookie cookie in HttpContext.Response.Cookies)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
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
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            return View(new AccountViewModel());
        }

        [HttpPost]
        public ActionResult AddAccount(AccountViewModel account)
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            var store = new AccountStore();
            var mapper = new AccountMapper();
            store.Add(mapper.FromModel(account));
            return RedirectToAction("Accounts");
        }

        public ActionResult Accounts()
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            var store = new AccountStore();
            var accounts = store.All();
            var viewModel = new AccountListViewModel
            {
                emails = accounts.Select(a => a.Email).ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult EditAccount(string email)
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            var store = new AccountStore();
            var account = store.GetByEmail(email);
            var mapper = new AccountMapper();
            var viewModel = mapper.ToModel(account);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditAccount(AccountViewModel account)
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            var store = new AccountStore();
            var mapper = new AccountMapper();
            store.Update(mapper.FromModel(account));

            return RedirectToAction("Accounts");
        }

        public ActionResult DeleteAccount(AccountViewModel account)
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            var store = new AccountStore();
            store.DeleteByEmail(account.email);
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public ActionResult AddQuestion()
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }
            return View(new QuestionIoModel());
        }

        [HttpPost]
        public ActionResult AddQuestion(QuestionIoModel question)
        {
            var httpCookie = HttpContext.Request.Cookies[CookiewKeyAdmin];
            if (httpCookie == null || !httpCookie.Value.Equals(bool.TrueString))
            {
                return RedirectToAction("Login");
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            question.image1 = SaveImage(WebImage.GetImageFromRequest("Image1"));
            question.image2 = SaveImage(WebImage.GetImageFromRequest("Image2"));
            question.image3 = SaveImage(WebImage.GetImageFromRequest("Image3"));
            question.image4 = SaveImage(WebImage.GetImageFromRequest("Image4"));
            
            var folderStore = new FolderStore();
            var folderId = folderStore.GetFolderToAddQuestion(question.language);
            var mapper = new QuestionMapper();
            var questionStore = new QuestionStore();

            questionStore.Add(folderId, mapper.ToModel(question));

            return RedirectToAction("Admin");
        }

        private string SaveImage(WebImage image)
        {
            string newFileName = string.Empty;
            if (image != null)
            {
                newFileName = Path.GetRandomFileName() + "_" + Path.GetFileName(image.FileName);
                var imagePath = @"images\" + newFileName;

                image.Save(@"~\" + imagePath);
            }
            return newFileName;
        }

        public ActionResult DeleteQuestion(QuestionViewModel question)
        {
            bool goToAdmin = false;
            var questionStore = new QuestionStore();
            var folderStore = new FolderStore();
            var folderMapper = new FolderMapper(new QuestionMapper());

            var folderId = questionStore.GetFolderId(question.id);
            if (folderId == 0)
            {
                goToAdmin = true;
            }
            var folder = folderStore.GetById(folderId);
            if (folder == null)
            {
                goToAdmin = true;
            }
            var folderModel = folderMapper.ToModel(folder);

            questionStore.Delete(question.id);

            var count = questionStore.CountByFolderId(folderId);
            if (count == 0)
            {
                folderStore.Delete(folderId);
                if (!goToAdmin)
                {
                    return RedirectToAction("Folders", "Home", new {language = folder.Language});
                }
            }
            return goToAdmin ? RedirectToAction("Admin") : RedirectToAction("Folder", folderModel);
        }
    }
}