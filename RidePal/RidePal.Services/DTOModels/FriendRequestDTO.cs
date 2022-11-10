
using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.DTOModels
{
    public class FriendRequestDTO
    {
        public int Id { get; set; }
        public virtual UserDTO Recipient { get; set; }
        public virtual UserDTO Sender { get; set; }
    }
}
