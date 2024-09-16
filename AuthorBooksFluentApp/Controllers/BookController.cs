using System;
using System.Linq;
using System.Web.Mvc;
using AuthorBooksFluentApp.Data;
using AuthorBooksFluentApp.Models;

namespace AuthorBooksFluentApp.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BookDetails(Guid authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var books = session.Query<Book>().Where(o => o.Author.Id == authId).ToList();
                Session["AuthId"] = authId; 

                return View(books);
            }
        }

        public ActionResult Create()
        {
            if (Session["AuthId"] != null)
            {
                Guid authId = (Guid)Session["AuthId"];
                ViewBag.AuthId = authId;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book, Guid authId)
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
                if (Session["AuthId"] != null)
                {
                    Guid authId = (Guid)Session["AuthId"];
                    ViewBag.AuthId = authId;
                }
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Edit(Book book, Guid authId)
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

                    return RedirectToAction("BookDetails", new { authId = authId });
                }
            }
        }

        public ActionResult Delete(int id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var book = session.Get<Book>(id);
                Guid authId = (Guid)Session["AuthId"];
                ViewBag.AuthId = authId;
                return View(book);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, Guid authId)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var book = session.Get<Book>(id);
                    session.Delete(book);
                    transaction.Commit();

                    return RedirectToAction("BookDetails", new { authId = authId });
                }
            }
        }
    }
}
