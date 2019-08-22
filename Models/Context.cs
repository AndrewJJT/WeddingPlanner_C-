    using Microsoft.EntityFrameworkCore; 
    namespace WeddingPlanner.Models
    {
        public class MyContext : DbContext
        {
            public MyContext(DbContextOptions options) : base(options) { }
            
            // "users" table is represented by this DbSet "Users"
            public DbSet<User> users {get;set;}
            public DbSet<Attendance> attendences {get;set;}

            public DbSet<Wedding> weddings {get;set;}
        }
    }
