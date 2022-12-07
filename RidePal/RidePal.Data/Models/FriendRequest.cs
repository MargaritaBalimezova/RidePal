using RidePal.Data.Models.Interfaces;
using System;

namespace RidePal.Data.Models
{
    public class FriendRequest : IHasId, IDeletable
    {
        public int Id { get; set; }

        public int? RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        public int? SenderId { get; set; }

        public virtual User Sender { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}