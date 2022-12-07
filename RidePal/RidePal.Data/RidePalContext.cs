using Microsoft.EntityFrameworkCore;
using RidePal.Data.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RidePal.Data
{
    public class RidePalContext : DbContext
    {
        public RidePalContext(DbContextOptions<RidePalContext> options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Audience> Audience { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<PlaylistTracks> PlaylistTracks { get; set; }

        public DbSet<Reaction> Reactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .HasQueryFilter(album => album.IsDeleted == false);
            modelBuilder.Entity<Artist>()
                .HasQueryFilter(artist => artist.IsDeleted == false);
            modelBuilder.Entity<Genre>()
                .HasQueryFilter(genre => genre.IsDeleted == false);
            modelBuilder.Entity<Playlist>()
                .HasQueryFilter(playlist => playlist.IsDeleted == false);
            modelBuilder.Entity<Track>()
                .HasQueryFilter(track => track.IsDeleted == false);
            modelBuilder.Entity<Trip>()
                .HasQueryFilter(trip => trip.IsDeleted == false);
            //modelBuilder.Entity<User>()
            //    .HasQueryFilter(user => user.IsDeleted == false);
            modelBuilder.Entity<FriendRequest>()
                .HasQueryFilter(friendRequest => friendRequest.IsDeleted == false);
            modelBuilder.Entity<PlaylistTracks>()
                .HasQueryFilter(playlistTracks => playlistTracks.IsDeleted == false);
            modelBuilder.Entity<Reaction>()
                .HasQueryFilter(reactions => reactions.IsDeleted == false);
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

            //TODO: Uncomment next line in case you have no seeded data
            //modelBuilder.Seed().Wait();

            // SetMinLengthConstraints(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        break;
                }
            }
        }
    }
}