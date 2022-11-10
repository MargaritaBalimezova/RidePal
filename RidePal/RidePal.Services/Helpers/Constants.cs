﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RidePal.Services.Helpers
{
    public class Constants
    {
        public const string LocationUrlWithoutAddress = "Locations/{0}/{1}?key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";

        public const string LocationUrlWithAddress = "Locations/{0}/{1}?key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";

        public const string MatrixUr = "Routes/DistanceMatrix?origins={0},{1}&destinations={2},{3}&travelMode=driving&key=Ap4BY1B20Ldbim2QZCCJK3EAW0jdSisoIOvt3kJjv8dBjM_3pzfu9442oM8AsrxJ";



        //Exception messages
        public const string TRACK_NOT_FOUND = "Track with id {0} not found!";
        public const string ARTIST_NOT_FOUND = "Artist not found!";
        public const string GENRE_NOT_FOUND = "Unexisting genre passed!";
        public const string ALBUM_NOT_FOUND = "Album {0} not found!";
        public const string INVALID_DATA = "Invalid data passed!";
    }
}
