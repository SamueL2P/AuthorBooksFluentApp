using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorBooksFluentApp.Data;
using AuthorBooksFluentApp.Models;
using NHibernate.Criterion;
using NHibernate.Loader;

namespace AuthorBooksFluentApp.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BookDetails(int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(o => o.Author.Id == authId).ToList();
                TempData["AuthId"] = authId;

                return View(books);
            }
        }

        public ActionResult Create()
        {
            int authId = (int)TempData["AuthId"];
            ViewBag.AuthId = authId;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Book book, int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var author = session.Get<Author>(authId);
                    book.Author = author;

                    session.Save(book);
                    transaction.Commit();
                    return RedirectToAction("BookDetails", new { authId = author.Id });
                }
            }
        }

        public ActionResult Edit(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {

                var book = session.Get<Book>(id);

                ViewBag.AuthId = book.Author != null ? book.Author.Id : 0;

                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Edit(Book book, int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    var existingBook = session.Get<Book>(book.Id);

                    existingBook.Name = book.Name;
                    existingBook.Description = book.Description;
                    existingBook.Genre = book.Genre;
                    existingBook.Author = session.Get<Author>(authId);

                    session.Update(existingBook);
                    transaction.Commit();


                    ViewBag.AuthId =  authId;

                    return RedirectToAction("BookDetails", new { authId = authId });
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                ViewBag.AuthId = book.Author != null ? book.Author.Id : 0;

                return View(book);
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var book = session.Get<Book>(id);
                    book.Author = session.Get<Author>(authId);
                    session.Delete(book);
                    transaction.Commit();



                    return RedirectToAction("BookDetails", new { authId = authId });
                }
            }
        }

    }
}