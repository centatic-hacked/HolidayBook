using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Model;
using Microsoft.EntityFrameworkCore;
using Bogus;
using System.IO;
using System.ComponentModel;

namespace Model.Infrastructure
{
    public class HolidayBookContext : DbContext
    {
        public DbSet<Currency> Currencies => Set<Currency>();

        public DbSet<User> Users => Set<User>();

        public DbSet<Airport> Airports => Set<Airport>();

        public HolidayBookContext()
        { }

        public HolidayBookContext(DbContextOptions options) : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Das muss weg, weil im Source Code gefährlich!!!!!!
            // "Data Source=/123.456.789.456 catalog=DbName UserId=Ich Pwd=Geheim!"
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=HolidayBook.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Alle zusätzlichen Infos um aus den Entities eine DB zu erstellen.
            //modelBuilder.Entity<Product>().HasKey(new Product().GetType().GetProperties().SingleOrDefault(p => p.Name == "Description").Name);
            modelBuilder.Entity<Currency>().HasKey(c => c.Currency_Code);
            modelBuilder.Entity<Airport>().HasKey(a => a.IATA);
            
            modelBuilder.Entity<Currency>().HasIndex(c => c.Currency_Code);

            modelBuilder.Entity<User>().OwnsOne(u => u.Address);
        }

        public void Seed()
        {
            IDictionary<string, string> start_currency;
            start_currency = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture => {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.ISOCurrencySymbol)
                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);

            var map_currency = start_currency.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<Currency> currencies = new();

            foreach (var key in map_currency.Keys)
            {
                //Console.WriteLine($"{key}...{map_currency[key]}");
                currencies.Add(new Currency(key, map_currency[key]));
            }

            Currencies.AddRange(currencies);
            SaveChanges();
            Randomizer.Seed = new Random(125620);

            List<User> users = new Faker<User>()
                .CustomInstantiator(f => new User(f.Name.FirstName(), f.Name.LastName(), f.Internet.Email(), f.Internet.Password(), f.Internet.UserName()))
                .Rules((f, c) =>
                {


                })
                .Generate(90);
                
            Users.AddRange(users);
            SaveChanges();
            string FileToRead = @"C:\HTL\3BHIF\HolidayBook\airports.txt";
            // Creating enumerable object  
            IEnumerable<string> stringAirports = File.ReadLines(FileToRead);
            List<Airport> airports = new List<Airport>();
            foreach (string line in stringAirports)
            {
                var newLine = line.Replace("\"", "");
                var os = newLine.Split(",");
                if (os[4].Length != 3) continue;
                airports.Add(new Airport(os[4], os[1], os[5], os[3], os[2]));
            }
            Airports.AddRange(airports);
            SaveChanges();
        }
    }
}
