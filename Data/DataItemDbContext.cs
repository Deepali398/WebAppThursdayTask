using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using static System.Reflection.Metadata.BlobBuilder;
using WebAppThursdayTask.Models;

namespace WebAppThursdayTask.Data
{
    public class DataItemDbContext:DbContext
    {
        public DataItemDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<DataItem> DataItems { get; set; }  
    }
}
