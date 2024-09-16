using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;

namespace AuthorBooksFluentApp.Models
{
    public class Author
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }
        public virtual string Name { get; set; }

        public virtual string Email { get; set; }

        public virtual int Age { get; set; }

        public virtual AuthorDetail AuthorDetail { get; set; } = new AuthorDetail();

        public virtual IList<Book> Book { get; set; } = new List<Book>();
    }
}