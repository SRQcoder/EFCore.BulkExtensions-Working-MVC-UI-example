using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication12_bulkExtensions.Models;

namespace WebApplication12_bulkExtensions.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<State> States { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
