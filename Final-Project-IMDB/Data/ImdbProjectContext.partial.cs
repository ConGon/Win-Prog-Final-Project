using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Final_Project_IMDB.Data
{
    internal partial class ImdbProjectContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connStr = ConfigurationManager.ConnectionStrings["ImdbProjectContext"].ConnectionString;
                optionsBuilder.UseSqlServer(connStr);
            }
        }
    }
}
