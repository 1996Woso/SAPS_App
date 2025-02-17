using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAPS_App.Models;

namespace SAPS_App.Context
{
    public class SAPS_Context :DbContext
    {
        public SAPS_Context(DbContextOptions<SAPS_Context> options)
       : base(options)
        {
        }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<CriminalRecord> CriminalRecords { get; set; }
        public DbSet<SuspectSearch> SuspectCriminalRecords { get; set; }
        public DbSet<CaseManager> Case_Managers { get; set; }
        public DbSet<Offences> Offences { get; set; }
        public DbSet<PoliceStations> PoliceStations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure 1-many relation between Suspect and CriminalRecord
            modelBuilder.Entity<Suspect>()
                .HasMany(x => x.CriminalRecords)
                .WithOne(x => x.Suspect)
                .HasForeignKey(x => x.SuspectNumber)
                .IsRequired();
            //Configure 1-many relation between Manager and CriminalRecord
            modelBuilder.Entity<CaseManager>()
                .HasMany(x => x.CriminalRecords)
                .WithOne(x => x.CaseManager)
                .HasForeignKey(x => x.CaseManagerNo)
                .IsRequired();

            modelBuilder.Entity<CriminalRecord>()
           .Property(c => c.Id)
           .UseIdentityColumn();

            // Configure SuspectSearch as a keyless entity
            modelBuilder.Entity<SuspectSearch>().HasNoKey().ToView(null);
            //configure relations between Suspect and CriminalRecord

            //   modelBuilder.Entity<Suspect>()
            //.HasOne(s => s.Manager)
            //.WithMany(manager => manager.ManagedCriminalRecords)
            //.HasForeignKey(s => s.ManagerId)
            //.IsRequired();

            //   modelBuilder.Entity<CriminalRecord>()
            //       .HasOne(cr => cr.Manager)
            //       .WithMany(manager => manager.ManagedCriminalRecords)
            //       .HasForeignKey(cr => cr.ManagerId)
            //       .IsRequired();


        }
        internal Task<IEnumerable<object>> GetUsersInRoleAsync(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}

