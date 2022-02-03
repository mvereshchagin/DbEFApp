using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEFApp
{
    public class Project
    {
        protected string ConnectionString = null!;
        protected ILoggerFactory? loggerFactory;

        public void Initialize()
        {
            //loggerFactory = LoggerFactory.Create(builder => {
            //    builder.AddFilter("Microsoft", LogLevel.Warning)
            //           .AddFilter("System", LogLevel.Warning)
            //           .AddFilter("SampleApp.Program", LogLevel.Debug)
            //           .AddConsole();
            //    }
            //);

            // Build a config object, using env vars and JSON providers.
            IConfiguration appConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddXmlFile("appsettings.xml", optional: true, reloadOnChange: true)
                //.AddIniFile("appsettings.ini")
                .Build();

            this.ConnectionString = appConfig.GetConnectionString("Zoo");
        }

        public bool CheckDB()
        {
            using (ApplicationContext db = GetContext())
            {
                return db.Database.CanConnect();
            }
        }

        ApplicationContext GetContext()
        {
            //return new ApplicationContext(this.ConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            // optionsBuilder.LogTo(Log);
            optionsBuilder.UseLoggerFactory(this.loggerFactory);
            var options = optionsBuilder.UseSqlServer(this.ConnectionString, 
                opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)).Options;
            return new ApplicationContext(options);
        }

        public void Log(string msg)
        {
            Console.WriteLine("============ Start EF Message =============");
            Console.WriteLine(msg);
            Console.WriteLine("============ End EF Message ===============");
        }

        //public void CreateEnclosures()
        //{
        //    using (ApplicationContext context = GetContext())
        //    {
        //        Enclosure enc1 = new Enclosure()
        //        { ID = Guid.NewGuid(), Number = 1 };
        //        Enclosure enc2 = new Enclosure { ID = Guid.NewGuid(), Number = 2 };
        //        Enclosure enc3 = new Enclosure { ID = Guid.NewGuid(), Number = 3 };

        //        context.Enclosures.AddRange(new Enclosure[] { enc1, enc2, enc3 });
        //        context.SaveChanges();
        //    }
        //}

        public void AddBoryaAndManyaInEnc1()
        {
            using (ApplicationContext context = GetContext())
            {
                var enc1 = context.GetEnclosureByNumber(1);

                if (enc1 is null)
                    return;

                var borya = new Animal("Borya")
                { 
                    ID = Guid.NewGuid(), 
                    Name = "Borya", 
                    Species = "Giraffe", 
                    EnclosureID = enc1.ID 
                };
                context.Animals.Add(borya);

                var manya = new Animal("Manya")
                { 
                    ID = Guid.NewGuid(), 
                    Name = "Manya", 
                    Species = "Giraffe", 
                    EnclosureID = enc1.ID
                };
                context.Animals.Add(manya);

                //borya.Enclosure = enc1;
                //manya.Enclosure = enc1;

                //enc1.Animals.AddRange(new[] { borya, manya });

                context.SaveChanges();
            }
        }

        //void AddPetya()
        //{
        //    using (ApplicationContext context = GetContext())
        //    {
        //        var petya = new Animal { ID = Guid.NewGuid(), Name = "Petya", Species = "Giraffe" };
        //        context.Animals.Add(petya);

        //        var enc = (from enclosure in context.Enclosures
        //                  where enclosure.Number == 1
        //                  select enclosure).Single();

        //        var item1 = new AnimalsEnclosures { ID = Guid.NewGuid(), Animal = petya, Enclosure = enc };
        //        context.AnimalsEnclosures.Add(item1);

        //        context.SaveChanges();
        //    }
        //}

        public void PrintAnimals()
        {
            using (var context = GetContext())
            {
                //var animals = context.Animals.
                //    Include(p => p.Enclosure).ToList();

                var enc2 = context.GetEnclosureByNumber(2);

                // context.Animals.Where(a => a.EnclosureID == enc2!.ID).Load();

                foreach(var animal in enc2!.Animals)
                {
                    Console.WriteLine(
                        $"{animal.Name} in Enclure with Number {animal.Enclosure?.Number}");
                }
            }
        }

        public void ChangeBorya()
        {
            Animal? borya = null;
            using (var context = GetContext())
            {
                borya = context.Animals.Where(x => x.Name == "Boris").Single();
                borya.Name = "Boris Ivanovich";
            }

            using (var context = GetContext())
            {  
                borya.Species = "Crocodile";
                context.Update(borya);
                borya.Species = "Giraffe";
                context.SaveChanges();
            }
        }
    }


}
