using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVCDemo.Models;

namespace WebMVCDemo.Data
{
    public class CountryContext : DbContext
    {
        public CountryContext(DbContextOptions<CountryContext> options) : base(options) 
        {}

        public DbSet<Country> Country{ get; set; }
    }
}
