using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebMVCDemo.Models;
using System;
using System.Linq;
using WebMVCDemo.Data;


namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CountryContext(serviceProvider.GetRequiredService<DbContextOptions<CountryContext>>()))
            {
                if (context.Country.Any())
                {
                    return;
                }
                context.Country.AddRange(
                    new Country
                    {
                        name = "China",
                        density = 1000,
                        population = 10000000,
                        sqaureMiles = 234,
                    },
                    new Country
                    {
                        name = "Iran",
                        density = 2030,
                        population = 20349910,
                        sqaureMiles = 98726,
                    });
                context.SaveChanges();
            }

        }
    }


}