using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AuthorBooksFluentApp.Models;
using FluentNHibernate.Mapping;

namespace AuthorBooksFluentApp.Mappings
{
    public class AuthorDetailsMap:ClassMap<AuthorDetail>
    {

        public AuthorDetailsMap() {
            Table("AuthorDetails");
            Id(a => a.Id).GeneratedBy.Identity();
            Map(a => a.Street);
            Map(a => a.City);
            Map(a => a.State);
            Map(a => a.Country);
            References(c => c.Author).Column("author_id").Unique().Cascade.None();


        }
    }
}