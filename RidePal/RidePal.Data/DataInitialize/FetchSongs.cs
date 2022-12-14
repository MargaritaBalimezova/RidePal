using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Data.DataInitialize.Models;
using RidePal.Data.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RidePal.Data.DataInitialize
{
    public class FetchSongs : IFetchSongs
    {
        //private readonly HttpClient client;
        private static HashSet<Artist> artists = new HashSet<Artist>();

        private static HashSet<Track> tracks = new HashSet<Track>();
        private static HashSet<Album> albums = new HashSet<Album>();

        public async Task<PlaylistsResult> GetPlaylistAsync(string url)
        {
            HttpClient client = new HttpClient();

            var reponse = await client.GetAsync(url);

            using var responseStream = await reponse.Content.ReadAsStreamAsync();

            var responseDesirialized = await JsonSerializer.DeserializeAsync<PlaylistsResult>(responseStream);

            return responseDesirialized;
        }

        public async Task<ArtistTrackAlbumWrap> GetTracksAsync(string playlistsUrl, Genre genre, int playlistToFetch = int.MaxValue)
        {
            HttpClient client = new HttpClient();
            var playlists = await this.GetPlaylistAsync(playlistsUrl);

            //TODO: consider removing imagepath from track!
            List<Artist> new_artists = new List<Artist>();
            List<Track> new_tracks = new List<Track>();
            List<Album> new_albums = new List<Album>();

            for (int i = 0; i < playlists.data.Length; i++)
            {
                if (i == playlistToFetch)
                {
                    break;
                }

                string tracklistUrl = playlists.data[i].tracklist;

                do
                {
                    var response = await client.GetAsync(tracklistUrl);
                    response.EnsureSuccessStatusCode();

                    using var reposnseStream = await response.Content.ReadAsStreamAsync();

                    var trackListDesirialized = await JsonSerializer.DeserializeAsync<TrackFetch>(reposnseStream);

                    for (int j = 0; j < trackListDesirialized.data.Length; j++)
                    {
                        var track = new Track
                        {
                            Id = trackListDesirialized.data[j].id,
                            Title = trackListDesirialized.data[j].title.Trim(),
                            Rank = trackListDesirialized.data[j].rank,
                            Duration = trackListDesirialized.data[j].duration,
                            GenreId = genre.Id,
                            PreviewURL = trackListDesirialized.data[j].preview,
                            ImagePath = trackListDesirialized.data[j].md5_image,
                            IsDeleted = false
                        };

                        if (tracks.Contains(track) || trackListDesirialized.data[j].album == null || trackListDesirialized.data[j].album.id == 0
                            || trackListDesirialized.data[j].artist == null || trackListDesirialized.data[j].artist.id == 0 || trackListDesirialized.data[j].album.title == null)
                        {
                            continue;
                        }

                        var artist = new Artist
                        {
                            Id = trackListDesirialized.data[j].artist.id,
                            Name = trackListDesirialized.data[j].artist.name,
                            IsDeleted = false,
                            ImagePathBig = trackListDesirialized.data[j].artist.picture_big,
                            ImagePathMedium = trackListDesirialized.data[j].artist.picture_medium,
                        };

                        if (artists.Contains(artist))
                        {
                            track.ArtistId = trackListDesirialized.data[j].artist.id;
                        }
                        else
                        {
                            artists.Add(artist);
                            new_artists.Add(artist);
                        }

                        var album = new Album
                        {
                            Id = trackListDesirialized.data[j].album.id,
                            Name = trackListDesirialized.data[j].album.title,
                            ArtistId = trackListDesirialized.data[j].artist.id,
                            GenreId = genre.Id,
                            IsDeleted = false,
                            ImagePath = trackListDesirialized.data[j].album.cover_medium
                        };

                        if (albums.Contains(album))
                        {
                            track.AlbumId = trackListDesirialized.data[j].album.id;
                        }
                        else
                        {
                            albums.Add(album);
                            new_albums.Add(album);
                        }

                        track.AlbumId = trackListDesirialized.data[j].album.id;
                        track.ArtistId = trackListDesirialized.data[j].artist.id;
                        tracks.Add(track);
                        new_tracks.Add(track);
                    }

                    tracklistUrl = trackListDesirialized.next;
                } while (tracklistUrl != null);
            }

            return new ArtistTrackAlbumWrap { artists = new_artists, albums = new_albums, tracks = new_tracks };
        }
    }
}