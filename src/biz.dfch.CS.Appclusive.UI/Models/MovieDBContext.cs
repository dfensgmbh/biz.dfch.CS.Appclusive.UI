using System;
using System.Data.Entity;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
    }
}