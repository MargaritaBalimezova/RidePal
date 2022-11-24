using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;

namespace RidePal.Data.Configuration
{
    public class PlaylistGenreConfiguration : IEntityTypeConfiguration<PlaylistGenre>
    {
        public void Configure(EntityTypeBuilder<PlaylistGenre> playlistGenre)
        {
            playlistGenre
               .HasKey(t => new { t.PlaylistId, t.GenreId });

            //For MovieId in MovieTags table
            playlistGenre
                .HasOne(m => m.Playlist)
                .WithMany(m => m.Genres)
                .HasForeignKey(m => m.PlaylistId)
                .OnDelete(DeleteBehavior.NoAction);

            //For TagId in MovieTags table
            playlistGenre
                .HasOne(a => a.Genre)
                .WithMany(a => a.Playlists)
                .HasForeignKey(a => a.GenreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}