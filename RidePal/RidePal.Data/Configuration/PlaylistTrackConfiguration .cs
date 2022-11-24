using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;

namespace RidePal.Data.Configuration
{
    public class PlaylistTrackConfiguration : IEntityTypeConfiguration<PlaylistTracks>
    {
        public void Configure(EntityTypeBuilder<PlaylistTracks> playlistTracks)
        {
            playlistTracks
               .HasKey(t => new { t.PlaylistId, t.TrackId });

            //For MovieId in MovieTags table
            playlistTracks
                .HasOne(m => m.Playlist)
                .WithMany(m => m.Tracks)
                .HasForeignKey(m => m.PlaylistId)
                .OnDelete(DeleteBehavior.NoAction);

            //For TagId in MovieTags table
            playlistTracks
                .HasOne(a => a.Track)
                .WithMany(a => a.Playlists)
                .HasForeignKey(a => a.TrackId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}