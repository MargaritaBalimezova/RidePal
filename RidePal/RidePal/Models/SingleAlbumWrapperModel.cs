using RidePal.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RidePal.WEB.Models
{
    public class SingleAlbumWrapperModel
    {
        public AlbumDTO Album { get; set; }
        public IEnumerable<AlbumDTO> SuggestedAlbums { get; set; } = new List<AlbumDTO>();
    }
}
