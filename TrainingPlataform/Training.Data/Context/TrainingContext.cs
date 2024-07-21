using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Training.Data.Extensions;
using Training.Data.Mappings;
using Training.Domain.Entities;

namespace Training.Data.Context
{
    public class TrainingContext : DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options) { }

        #region DbSets

        public DbSet<UsersType> UsersTypes { get; set; }
        public DbSet<ProfessionalType> ProfessionalTypes { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Client> Clients { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersTypeMap());
            modelBuilder.ApplyConfiguration(new ProfessionalMap());
            modelBuilder.ApplyConfiguration(new ClientMap());

            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}
