using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Data.Configuration
{
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder
                 .HasOne(x => x.Sender)
                .WithMany(x => x.SentFriendRequests)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Recipient)
               .WithMany(x => x.ReceivedFriendRequests)
               .HasForeignKey(x => x.RecipientId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
