using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBooksFluentApp.Models;
using FluentNHibernate.Mapping;

namespace AuthorBooksFluentApp.Mappings
{
    public class AuthorMap:ClassMap<Author>
    {
        public AuthorMap() {
            Table("Authors");
            Id(a=> a.Id).GeneratedBy.Identity();
            Map(a => a.Name);
            Map(a=>a.Email);
            Map(a => a.Age);
            HasOne(s => s.AuthorDetail).Cascade.All().PropertyRef(s => s.Author);
            HasMany(e => e.Book).Inverse().Cascade.All();



        }
    }
}