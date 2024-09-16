using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using AuthorBooksFluentApp.Data;
using AuthorBooksFluentApp.Models;
using AuthorBooksFluentApp.ViewModels;
using NHibernate.Linq;

namespace AuthorBooksFluentApp.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            if (Session["LoggedInAuthorId"] != null)
            {
                using (var session = NHibernateHelper.CreateSession())
                {
                    Guid loggedInAuthorId = (Guid)Session["LoggedInAuthorId"]; 

                    var author = session.Get<Author>(loggedInAuthorId);

                    return View(author);
                }
            }

            return RedirectToAction("Login");
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var txt = session.BeginTransaction())
                {
                    var user = session.Query<Author>().SingleOrDefault(u => u.UserName == loginVM.UserName);
                    if (user != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(loginVM.Password, user.Password))
                        {
                            FormsAuthentication.SetAuthCookie(loginVM.UserName, true);

                            Session["LoggedInAuthorId"] = user.Id;  
                            return RedirectToAction("Index");
                        }
                    }

                    ModelState.AddModelError("", "UserName/Password doesn't match");
                    return View();
                }
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {

                using (var txt = session.BeginTransaction())
                {
                    author.AuthorDetail.Author = author;
                    author.Password = BCrypt.Net.BCrypt.HashPassword(author.Password);
                    session.Save(author);
                    txt.Commit();
                    return RedirectToAction("Login");
                }
            }
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public ActionResult Create(Author author)
        {
        
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    author.AuthorDetail.Author =author;
                    author.Password = BCrypt.Net.BCrypt.HashPassword(author.Password);
                    session.Save(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }


        }

        public ActionResult Edit(Guid id)  // Update parameter type
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Get<Author>(id); 
                return View(author);
            }
        }

        [HttpPost]
        public ActionResult Edit(Author author)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var existingAuthor = session.Get<Author>(author.Id);
                    existingAuthor.Name = author.Name;
                    existingAuthor.Email = author.Email;
                    existingAuthor.Age = author.Age;
                    existingAuthor.AuthorDetail.City = author.AuthorDetail.City;
                    existingAuthor.AuthorDetail.Street = author.AuthorDetail.Street;
                    existingAuthor.AuthorDetail.State = author.AuthorDetail.State;
                    existingAuthor.AuthorDetail.Country = author.AuthorDetail.Country;
                    author.AuthorDetail.Author = author;
                    session.Update(existingAuthor);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Query<Author>().FirstOrDefault(u => u.Id == id);
                return View(author);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAuthor(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var author = session.Query<Author>().FirstOrDefault(u => u.Id == id);

                    author.AuthorDetail.City = "";
                    author.AuthorDetail.Street = "";
                    author.AuthorDetail.State = "";
                    author.AuthorDetail.Country = "";
                    session.Update(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        [AllowAnonymous]
        public ActionResult BookList()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                
                var books = session.Query<Book>().Fetch(b => b.Author).ToList();
                return View(books);
            }
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}