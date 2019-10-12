using System;
using Leo.Chimp;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
namespace ef_dapper
{
    class Program
    {
        static void Main(string[] args)
        {           
            
           
            Console.WriteLine("Hello World!");
            using (ChimpDbContext dbContext = new ChimpDbContext(options: new DbContextOptionsBuilder().UseMySql("Server=127.0.0.1;Database=Chimp;Uid=root;Pwd=123456").Options))
            {
                SchoolRepository schoolRepository = new SchoolRepository(dbContext);
                var school = new School() { Id = new Guid(), Name = "buzhidao"+new Random().Next(1,10) };
                  schoolRepository.Insert(school);//新增 
                dbContext.SaveChanges();//新增保存
                                        // var school = schoolRepository.;
               var has=  schoolRepository.TableNoTracking.AnyAsync(o=>o.Name.Contains("ceshi")).Result;

                var schools = schoolRepository.TableNoTracking.ToPagedListAsync(1, 10).Result;
                Console.WriteLine("___查询出ceshi有_"+ has+ "_ 分页查询结果__"+JsonConvert.SerializeObject(schools));
               
            }


             Console.ReadKey();
        }


        public class School : IEntity
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
        public interface ISchoolRepository : IRepository<School>
        {
        }
        public class SchoolRepository : EfCoreRepository<School>, ISchoolRepository
        {
            public SchoolRepository(DbContext context) : base(context)
            {
            }
        }
        public class ChimpDbContext : BaseDbContext
        {
            public ChimpDbContext(DbContextOptions options) : base(options)
            {
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                //your code
            }
        }
    }
}
