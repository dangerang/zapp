using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coddit.Models.Entities
{
    public partial class CodditContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MsSqlLocalDb;Initial Catalog=Zapp;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("varchar(max)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.ImageSrc).HasColumnType("varchar(max)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<AccountIdea>(entity =>
            {
                entity.HasKey(e => new { e.IdeaId, e.AccountId })
                    .HasName("PK__Account___8D685859F7514489");

                entity.ToTable("Account_Idea");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountIdea)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Account_I__Accou__4222D4EF");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.AccountIdea)
                    .HasForeignKey(d => d.IdeaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Account_I__IdeaI__412EB0B6");
            });

            modelBuilder.Entity<AccountSkill>(entity =>
            {
                entity.HasKey(e => new { e.SkillId, e.AccountId })
                    .HasName("PK__Account___ACE94BDD4651364F");

                entity.ToTable("Account_Skill");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountSkill)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Account_S__Accou__45F365D3");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.AccountSkill)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Account_S__Skill__44FF419A");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Comment__Account__4D94879B");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.IdeaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Comment__IdeaId__4CA06362");
            });

            modelBuilder.Entity<Idea>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("varchar(max)");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Uploads).HasColumnType("varchar(max)");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Idea)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Idea__CreatorId__38996AB5");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Rating__AccountI__5070F446");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.Rating)
                    .HasForeignKey(d => d.IdeaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Rating__IdeaId__5165187F");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.Property(e => e.SkillId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(25)");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(25)");
            });

            modelBuilder.Entity<TagIdea>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.IdeaId })
                    .HasName("PK__Tag_Idea__4A9EE18C88F7C1C1");

                entity.ToTable("Tag_Idea");

                entity.HasOne(d => d.Idea)
                    .WithMany(p => p.TagIdea)
                    .HasForeignKey(d => d.IdeaId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Tag_Idea__IdeaId__48CFD27E");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.TagIdea)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Tag_Idea__TagId__49C3F6B7");
            });
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountIdea> AccountIdea { get; set; }
        public virtual DbSet<AccountSkill> AccountSkill { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Idea> Idea { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<TagIdea> TagIdea { get; set; }
    }
}