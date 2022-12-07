using RidePal.Services.DTOModels;
using System.Collections.Generic;

namespace RidePal.WEB.Models
{
    public class SingleAlbumWrapperModel
    {
        public AlbumDTO Album { get; set; }
        public IEnumerable<AlbumDTO> SuggestedAlbums { get; set; } = new List<AlbumDTO>();
    }
}
