namespace RidePal.Services.Helpers
{
    public class Constants
    {
        public const string LocationUrlWithoutAddress = "Locations/{0}/{1}?key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";

        public const string LocationUrlWithAddress = "Locations/{0}/{1}/{2}?key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";

        public const string MatrixUr = "Routes/DistanceMatrix?origins={0},{1}&destinations={2},{3}&travelMode=driving&key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";

        public const string RedirectUrlDeezer = "https://localhost:5000/Auth/DeezerResponse&perms=offline_access,email";

        //Constrains
        public const int USER_FIRSTNAME_MIN_LENGTH = 4;

        public const int USER_LASTNAME_MIN_LENGTH = 4;
        public const int USER_USERNAME_MIN_LENGTH = 4;
        public const int USER_PASSWORD_MIN_LENGTH = 8;

        public const int PLAYLIST_TITLE_MIN_LENGTH = 4;

        //Exception messages
        public const string TRACK_NOT_FOUND = "Track with id {0} not found!";

        public const string AUDIENCE_NOT_FOUND = "Audience not found!";
        public const string ALREADY_TAKEN = "{0} is already taken!";
        public const string ARTIST_NOT_FOUND = "Artist not found!";
        public const string GENRE_NOT_FOUND = "Unexisting genre passed!";
        public const string ALBUM_NOT_FOUND = "Album {0} not found!";
        public const string TRIP_NOT_FOUND = "Trip not found!";
        public const string NO_TRIPS_FOUND = "No trips found!";
        public const string INVALID_DATA = "Invalid data passed!";
        public const string PLAYLIST_NOT_FOUND = "Playlist not found!";
        public const string USER_NOT_FOUND = "User not found!";
        public const string Playlist_NOT_FOUND = "Playlist not found!";
        public const string NOT_ENOUGH_PARAMETERS_PASSED = "Not enough parameters passed to execute an operation!";
    }
}