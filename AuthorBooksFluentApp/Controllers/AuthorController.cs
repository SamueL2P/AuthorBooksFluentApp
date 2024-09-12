using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorBooksFluentApp.Data;
using AuthorBooksFluentApp.Models;

namespace AuthorBooksFluentApp.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var authors = session.Query<Author>().ToList();
                return View(authors);
            }
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Author author)
        {
        
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    author.AuthorDetail.Author =author;
                    session.Save(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }


        }

        public ActionResult Edit(int id)
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

                    author.AuthorDetail.Author = author;
                    session.Update(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var author = session.Query<Author>().FirstOrDefault(u => u.Id == id);
                return View(author);
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteAuthor(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var author = session.Query<Author>().FirstOrDefault(u => u.Id == id);
                    session.Delete(author);
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
            }
        }


    }
}