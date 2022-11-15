using RidePal.Data.DataInitialize.Interfaces;
using RidePal.Data.DataInitialize.Models;
using RidePal.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace RidePal.Data.DataInitialize
{
    public class FetchSongs : IFetchSongs
    {
       //private readonly HttpClient client;
        public static HashSet<Artist> artists = new HashSet<Artist>();
        public static HashSet<Track> tracks = new HashSet<Track>();
        public static HashSet<Album> albums = new HashSet<Album>();

    /*    public FetchSongs(HttpClient client)
        {
            this.client = client;
        }*/

        public async Task<PlaylistsResult> GetPlaylistAsync(string url)
        {
            HttpClient client = new HttpClient();

            var reponse = await client.GetAsync(url);

            using var responseStream = await reponse.Content.ReadAsStreamAsync();

            var responseDesirialized = await JsonSerializer.DeserializeAsync<PlaylistsResult>(responseStream);

            return responseDesirialized;
        }

        public async Task<ArtistTrackAlbumWrap> GetTracksAsync(string playlistsUrl, Genre genre)
        {
            HttpClient client = new HttpClient();
            var playlists = await this.GetPlaylistAsync(playlistsUrl);

            //TODO: consider removing imagepath from track!
            List<Artist> new_artists = new List<Artist>();
            List<Track> new_tracks = new List<Track>();
            List<Album> new_albums = new List<Album>();

            for (int i = 0; i < playlists.data.Length; i++)
            {
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
                            Title = trackListDesirialized.data[j].title,
                            Rank = trackListDesirialized.data[j].rank,
                            Duration = trackListDesirialized.data[j].duration,
                            GenreId = genre.Id,
                            PreviewURL = trackListDesirialized.data[j].preview,
                            ImagePath = trackListDesirialized.data[j].md5_image,
                            IsDeleted = false
                        };

                        if (tracks.Contains(track))
                        {
                            continue;
                        }

                        var artist = new Artist
                        {
                            Id = trackListDesirialized.data[j].artist.id,
                            Name = trackListDesirialized.data[j].artist.name,
                            IsDeleted = false
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
                            IsDeleted = false
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
