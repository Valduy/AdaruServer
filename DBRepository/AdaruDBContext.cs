using Microsoft.EntityFrameworkCore;
using Models;

#nullable disable

namespace DBRepository
{
    public partial class AdaruDBContext : DbContext
    {
        public AdaruDBContext()
        {
        }

        public AdaruDBContext(DbContextOptions<AdaruDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<ClientInfo> ClientInfos { get; set; }
        public virtual DbSet<CustomerInfo> CustomerInfos { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageTag> ImageTags { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<PerformerInfo> PerformerInfos { get; set; }
        public virtual DbSet<PerformerTag> PerformerTags { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileImage> ProfileImages { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskInfo> TaskInfos { get; set; }
        public virtual DbSet<TaskStatus> TaskStatuses { get; set; }
        public virtual DbSet<TaskTag> TaskTags { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseNpgsql("Host=localhost;Database=AdaruDB;Username=postgres;Password=1423");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("chat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdSource).HasColumnName("id_source");

                entity.Property(e => e.IdTarget).HasColumnName("id_target");

                entity.HasOne(d => d.IdSourceNavigation)
                    .WithMany(p => p.ChatIdSourceNavigations)
                    .HasForeignKey(d => d.IdSource)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("chat_id_source_fkey");

                entity.HasOne(d => d.IdTargetNavigation)
                    .WithMany(p => p.ChatIdTargetNavigations)
                    .HasForeignKey(d => d.IdTarget)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("chat_id_target_fkey");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.HasIndex(e => e.Login, "client_login_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdImage).HasColumnName("id_image");

                entity.Property(e => e.IdRole).HasColumnName("id_role");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.HasOne(d => d.IdImageNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.IdImage)
                    .HasConstraintName("client_id_image_fkey");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.IdRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("client_id_role_fkey");
            });

            modelBuilder.Entity<ClientInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("client_info");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(200)
                    .HasColumnName("path");

                entity.Property(e => e.Resume)
                    .HasMaxLength(1000)
                    .HasColumnName("resume");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<CustomerInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("customer_info");

                entity.Property(e => e.Expirience).HasColumnName("expirience");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(200)
                    .HasColumnName("path");

                entity.Property(e => e.Raiting).HasColumnName("raiting");

                entity.Property(e => e.Resume)
                    .HasMaxLength(1000)
                    .HasColumnName("resume");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("image");

                entity.HasIndex(e => e.Path, "image_path_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("path");
            });

            modelBuilder.Entity<ImageTag>(entity =>
            {
                entity.HasKey(e => new { e.IdImage, e.IdTag })
                    .HasName("image_tags_pkey");

                entity.ToTable("image_tags");

                entity.Property(e => e.IdImage).HasColumnName("id_image");

                entity.Property(e => e.IdTag).HasColumnName("id_tag");

                entity.HasOne(d => d.IdImageNavigation)
                    .WithMany(p => p.ImageTags)
                    .HasForeignKey(d => d.IdImage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("image_tags_id_image_fkey");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.ImageTags)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("image_tags_id_tag_fkey");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("content");

                entity.Property(e => e.IdChat).HasColumnName("id_chat");

                entity.Property(e => e.IdSender).HasColumnName("id_sender");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.IdChatNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.IdChat)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_id_chat_fkey");

                entity.HasOne(d => d.IdSenderNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.IdSender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("message_id_sender_fkey");
            });

            modelBuilder.Entity<PerformerInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("performer_info");

                entity.Property(e => e.Expirience).HasColumnName("expirience");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(200)
                    .HasColumnName("path");

                entity.Property(e => e.Raiting).HasColumnName("raiting");

                entity.Property(e => e.Resume)
                    .HasMaxLength(1000)
                    .HasColumnName("resume");

                entity.Property(e => e.Role)
                    .HasMaxLength(20)
                    .HasColumnName("role");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<PerformerTag>(entity =>
            {
                entity.HasKey(e => new { e.IdPerformer, e.IdTag })
                    .HasName("performer_tags_pkey");

                entity.ToTable("performer_tags");

                entity.Property(e => e.IdPerformer).HasColumnName("id_performer");

                entity.Property(e => e.IdTag).HasColumnName("id_tag");

                entity.HasOne(d => d.IdPerformerNavigation)
                    .WithMany(p => p.PerformerTags)
                    .HasForeignKey(d => d.IdPerformer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("performer_tags_id_performer_fkey");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.PerformerTags)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("performer_tags_id_tag_fkey");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(e => e.IdClient)
                    .HasName("profile_pkey");

                entity.ToTable("profile");

                entity.Property(e => e.IdClient)
                    .ValueGeneratedNever()
                    .HasColumnName("id_client");

                entity.Property(e => e.Resume)
                    .HasMaxLength(1000)
                    .HasColumnName("resume");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithOne(p => p.Profile)
                    .HasForeignKey<Profile>(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profile_id_client_fkey");
            });

            modelBuilder.Entity<ProfileImage>(entity =>
            {
                entity.HasKey(e => new { e.IdProfile, e.IdImage })
                    .HasName("profile_images_pkey");

                entity.ToTable("profile_images");

                entity.Property(e => e.IdProfile).HasColumnName("id_profile");

                entity.Property(e => e.IdImage).HasColumnName("id_image");

                entity.HasOne(d => d.IdImageNavigation)
                    .WithMany(p => p.ProfileImages)
                    .HasForeignKey(d => d.IdImage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profile_images_id_image_fkey");

                entity.HasOne(d => d.IdProfileNavigation)
                    .WithMany(p => p.ProfileImages)
                    .HasForeignKey(d => d.IdProfile)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("profile_images_id_profile_fkey");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("review");

                entity.HasIndex(e => new { e.IdSource, e.IdTarget }, "review_id_source_id_target_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(500)
                    .HasColumnName("content");

                entity.Property(e => e.IdSource).HasColumnName("id_source");

                entity.Property(e => e.IdTarget).HasColumnName("id_target");

                entity.Property(e => e.Mark).HasColumnName("mark");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.IdSourceNavigation)
                    .WithMany(p => p.ReviewIdSourceNavigations)
                    .HasForeignKey(d => d.IdSource)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("review_id_source_fkey");

                entity.HasOne(d => d.IdTargetNavigation)
                    .WithMany(p => p.ReviewIdTargetNavigations)
                    .HasForeignKey(d => d.IdTarget)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("review_id_target_fkey");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.HasIndex(e => e.Name, "tag_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("task");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("content");

                entity.Property(e => e.Header)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("header");

                entity.Property(e => e.IdCustomer).HasColumnName("id_customer");

                entity.Property(e => e.IdPerformer).HasColumnName("id_performer");

                entity.Property(e => e.IdStatus).HasColumnName("id_status");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.IdCustomerNavigation)
                    .WithMany(p => p.TaskIdCustomerNavigations)
                    .HasForeignKey(d => d.IdCustomer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_id_customer_fkey");

                entity.HasOne(d => d.IdPerformerNavigation)
                    .WithMany(p => p.TaskIdPerformerNavigations)
                    .HasForeignKey(d => d.IdPerformer)
                    .HasConstraintName("task_id_performer_fkey");

                entity.HasOne(d => d.IdStatusNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.IdStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_id_status_fkey");
            });

            modelBuilder.Entity<TaskInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("task_info");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .HasColumnName("content");

                entity.Property(e => e.Header)
                    .HasMaxLength(100)
                    .HasColumnName("header");

                entity.Property(e => e.IdClient).HasColumnName("id_client");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .HasColumnName("status");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.IdTask).HasColumnName("id_task");

                entity.Property(e => e.Time).HasColumnName("time");
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.ToTable("task_status");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<TaskTag>(entity =>
            {
                entity.HasKey(e => new { e.IdTask, e.IdTag })
                    .HasName("task_tags_pkey");

                entity.ToTable("task_tags");

                entity.Property(e => e.IdTask)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id_task");

                entity.Property(e => e.IdTag).HasColumnName("id_tag");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.TaskTags)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_tags_id_tag_fkey");

                entity.HasOne(d => d.IdTaskNavigation)
                    .WithMany(p => p.TaskTags)
                    .HasForeignKey(d => d.IdTask)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("task_tags_id_task_fkey");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
