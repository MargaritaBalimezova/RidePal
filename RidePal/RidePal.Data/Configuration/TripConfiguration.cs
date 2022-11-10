using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configuration
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder
              .HasOne(a => a.Playlist).WithOne(b => b.Trip)
              .HasForeignKey<Playlist>(e => e.TripId)
              .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
