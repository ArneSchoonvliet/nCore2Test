using Digipolis.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nCore2Test.DataAccess.Entities;

namespace nCore2Test.DataAccess
{
    public class MyEntityContext : EntityContextBase<MyEntityContext>
    {
        public MyEntityContext(DbContextOptions<MyEntityContext> options) : base(options)
        {
        }

        DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("main");
            base.OnModelCreating(modelBuilder);
        }

    }
}
