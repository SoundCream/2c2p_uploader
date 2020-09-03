using System;
using System.Collections.Generic;
using System.Text;
using _2C2P.FileUploader.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2C2P.FileUploader.Data
{
    public class TransactionEntitiesDbContext : DbContext
    {
        public TransactionEntitiesDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<TransactionEntity> Transactions { get; set;}

        public DbSet<TransactionStatusEntity> TransactionStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionEntity>().HasOne(entity => entity.Status).WithMany().HasForeignKey(x => x.StatusId);
        }
    }
}
