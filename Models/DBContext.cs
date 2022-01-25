using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AS_Assignment.Models
{
    public class DBContext : DbContext
    {
        private readonly IConfiguration _config;

        public DBContext(IConfiguration _config)
        {
            this._config = _config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optBuilder)
        {
            string connStr = _config.GetConnectionString("myConn");
            optBuilder.UseSqlServer(connStr);
        }

        public DbSet<User> Users { get; set; }
    }
}
