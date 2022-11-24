using RidePal.Data.Models;

namespace RidePal.Services.DTOModels
{
    public class UpdatePlaylistDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Audience Audience { get; set; }
    }
}