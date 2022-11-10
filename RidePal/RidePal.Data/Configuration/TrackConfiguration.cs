using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configuration
{
    public class TrackConfiguration : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder
               .HasOne(x => x.Album)
               .WithMany(x => x.Tracks)
               .HasForeignKey(x => x.AlbumId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
