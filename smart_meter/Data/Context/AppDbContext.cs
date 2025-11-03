using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Entities;

namespace smart_meter.Data.Context
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Arrear> Arrears { get; set; }

        public virtual DbSet<Bill> Bills { get; set; }

        public virtual DbSet<Consumer> Consumers { get; set; }

        public virtual DbSet<Consumeraddress> Consumeraddresses { get; set; }

        public virtual DbSet<Meter> Meters { get; set; }

        public virtual DbSet<Meterreading> Meterreadings { get; set; }

        public virtual DbSet<Orgunit> Orgunits { get; set; }

        public virtual DbSet<Tariff> Tariffs { get; set; }

        public virtual DbSet<Tariffslab> Tariffslabs { get; set; }

        public virtual DbSet<Tarrifdetail> Tarrifdetails { get; set; }

        public virtual DbSet<Todrule> Todrules { get; set; }

        public virtual DbSet<User> User { get; set; }

        public  virtual DbSet<ConnectionRequest> ConnectionRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arrear>(entity =>
            {
                entity.HasKey(e => e.Arrearid).HasName("arrears_pkey");

                entity.Property(e => e.Paidstatus).HasDefaultValueSql("'Unpaid'::character varying");

                entity.HasOne(d => d.Bill).WithOne(p => p.Arrear)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("arrears_billid_fkey");

                entity.HasOne(d => d.Consumer).WithMany(p => p.Arrears)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("arrears_consumerid_fkey");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.HasKey(e => e.Billid).HasName("bill_pkey");

                entity.Property(e => e.Generatedat).HasDefaultValueSql("now()");
                entity.Property(e => e.Ispaid).HasDefaultValue(false);
                entity.Property(e => e.Paymentdate).HasDefaultValueSql("CURRENT_DATE");
                entity.Property(e => e.Totalamount).HasComputedColumnSql("(baseamount + taxamount)", true);

                entity.HasOne(d => d.Consumer).WithMany(p => p.Bills)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bill_consumerid_fkey");

                entity.HasOne(d => d.MeterserialnoNavigation).WithMany(p => p.Bills)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bill_meterserialno_fkey");
            });

            modelBuilder.Entity<Consumer>(entity =>
            {
                entity.HasKey(e => e.Consumerid).HasName("consumer_pkey");

                entity.Property(e => e.Createdat).HasDefaultValueSql("now()");
                entity.Property(e => e.Createdby).HasDefaultValueSql("'system'::character varying");
                entity.Property(e => e.Isdeleted).HasDefaultValue(false);
                entity.Property(e => e.Status).HasDefaultValueSql("'Active'::character varying");

                entity.HasOne(d => d.Orgunit).WithMany(p => p.Consumers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("consumer_orgunitid_fkey");

                entity.HasOne(d => d.Tariff).WithMany(p => p.Consumers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("consumer_tariffid_fkey");

                

            });

            modelBuilder.Entity<Consumeraddress>(entity =>
            {
                entity.HasKey(e => e.Addressid).HasName("consumeraddress_pkey");

                entity.HasOne(d => d.Consumer).WithOne(p => p.Consumeraddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("consumeraddress_consumerid_fkey");
            });

            modelBuilder.Entity<Meter>(entity =>
            {
                entity.HasKey(e => e.Meterserialno).HasName("meter_pkey");

                entity.Property(e => e.Status).HasDefaultValueSql("'Active'::character varying");

                entity.HasOne(d => d.Consumer).WithMany(p => p.Meters).HasConstraintName("meter_consumerid_fkey");
            });

            modelBuilder.Entity<Meterreading>(entity =>
            {
                entity.HasKey(e => e.Readingid).HasName("meterreadings_pkey");

                entity.HasOne(d => d.MeterserialnoNavigation).WithMany(p => p.Meterreadings)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("meterreadings_meterserialno_fkey");
            });

            modelBuilder.Entity<Orgunit>(entity =>
            {
                entity.HasKey(e => e.Orgunitid).HasName("orgunit_pkey");

                entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent).HasConstraintName("orgunit_parentid_fkey");
            });

            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.HasKey(e => e.Tariffid).HasName("tariff_pkey");
            });

            modelBuilder.Entity<Tariffslab>(entity =>
            {
                entity.HasKey(e => e.Tariffslabid).HasName("tariffslab_pkey");

                entity.Property(e => e.Isdeleted).HasDefaultValue(false);

                entity.HasOne(d => d.Tariff).WithMany(p => p.Tariffslabs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tariffslab_tariffid_fkey");
            });

            modelBuilder.Entity<Tarrifdetail>(entity =>
            {
                entity.HasKey(e => e.Tarrifdetailid).HasName("tarrifdetails_pkey");

                entity.Property(e => e.Tarrifdetailid).ValueGeneratedNever();

                entity.HasOne(d => d.Tariffslab).WithMany(p => p.Tarrifdetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarrifdetails_tariffslabid_fkey");

                entity.HasOne(d => d.Tarrif).WithMany(p => p.Tarrifdetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarrifdetails_tarrifid_fkey");

                entity.HasOne(d => d.Todrule).WithMany(p => p.Tarrifdetails)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tarrifdetails_todruleid_fkey");
            });

            modelBuilder.Entity<Todrule>(entity =>
            {
                entity.HasKey(e => e.Todruleid).HasName("todrule_pkey");

                entity.Property(e => e.Isdeleted).HasDefaultValue(false);

                entity.HasOne(d => d.Tariff).WithMany(p => p.Todrules)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("todrule_tariffid_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Userid).HasName("User_pkey");

                entity.Property(e => e.Isactive).HasDefaultValue(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}