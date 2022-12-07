using RidePal.Data.Models.Interfaces;

namespace RidePal.Data.Models
{
    public class Role : IHasId
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}