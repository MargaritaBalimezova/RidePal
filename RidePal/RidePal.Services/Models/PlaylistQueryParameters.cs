namespace RidePal.Services.Models
{
    public class PlaylistQueryParameters
    {
        public int? Duration { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
