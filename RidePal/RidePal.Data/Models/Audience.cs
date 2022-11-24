using MovieForum.Data.Models.Interfaces;

namespace RidePal.Data.Models
{
    public class Audience : IHasId
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}