using RidePal.Data.Models;

namespace RidePal.WEB.Models
{
    public class UpdatePlaylistViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Audience Audience { get; set; }
    }
}