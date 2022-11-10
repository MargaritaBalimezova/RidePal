using MovieForum.Data.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RidePal.Data.Models
{
    public class FriendRequest : IHasId
    {
        public int Id { get; set; }

        [Required]
        public int RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        [Required]
        public int SenderId { get; set; }

        public virtual User Sender { get; set; }
    }
}
