using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Final_Project_IMDB.Data.Generated
{
    public partial class ImdbProjectContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var conn = ConfigurationManager
                    .ConnectionStrings["ImdbProjectContext"]
                    .ConnectionString;

                optionsBuilder.UseSqlServer(conn);
            }
        }
    }
}