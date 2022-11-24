//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using RidePal.Data.Models;

//namespace RidePal.Data.Configuration
//{
//    public class TripConfiguration : IEntityTypeConfiguration<Trip>
//    {
//        public void Configure(EntityTypeBuilder<Trip> builder)
//        {
//            builder
//              .HasOne(a => a.).WithOne(b => b.Trip)
//              .HasForeignKey<Playlist>(e => e.TripId)
//              .OnDelete(DeleteBehavior.NoAction);
//        }
//    }
//}