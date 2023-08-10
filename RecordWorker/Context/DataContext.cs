using Microsoft.EntityFrameworkCore;
using RecordWorker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.Context
{
    public class DataContext : DbContext
    {
        public DbSet<WorkReportRecord> WorkReportRecord => Set<WorkReportRecord>();

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkReportRecord>()
                .HasIndex(wrr => wrr.EventId)
                .IsUnique();
        }
    }
}
