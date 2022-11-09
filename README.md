## RidePal

### Playlist generator for your travel


### Project Description

Your task is to develop the **RidePal** web application. **RidePal** enables your users to

### generate playlists for specific travel duration periods based on their preferred genres.

A playlist consists of a list of individual tracks (songs). Each track has an artist, title,
album, duration (playtime length) and rank (a numeric value). Tracks may or may not
have a preview URL to an audio stream (e.g. the users can click and play the preview

### part of the track).

Each playlist has a user given title, associated tags (e.g. musical genres), a list of
tracks with the associated track details, total playtime (the sum of the playtimes of
all tracks in that playlist) and rank (the average of the ranks of all tracks in that

playlist).

### Functional Requirements

User must be able to enter starting and destination address, selects musical genres

and clicks “Generate”. Something like:

_Note: The purpose of the above wireframe is to_ **_only illustrate_** _the basic UI
components and is not a guideline of how your UI must look like._


**RidePal** calculates the travel duration time between the starting and destination
locations and combines tracks chosen randomly in the specified genres, until the

playing time roughly matches the travel time duration. Rounding of +/- 5 minutes is
allowed (e.g. for a travel duration of 89 minutes a playlist with total playtime
between 84 or 94 and is fine).

The generated playlist must be saved under this user’s profile, and they can start

listening to it.

The application must offer the option to browse playlists created by other users and

### allows filtering by total duration and genre tags.

By default, playlists must be sorted by average rank descending.

The application should offer certain configuration over the playlist generation
algorithm and the possibility to play a preview of the tracks in a playlist.

#### Generation Algorithm Spec

- The playlist must not have duplicate tracks.
- Should not repeat artists (unless “allow tracks from the same artists” is
    selected)
- Should generate random playlists. For example, for the same two starting and
    destination locations, clicking Generate multiple times should not generate
    the same playlist twice (repeating tracks/artists between playlists is allowed).
- Should allow the user to specify if they want more from specific genre using
    percentage as shown in the wireframe (or it can be left blank)
- Generated playlist total playtime should be +/- 5mins of the calculated travel
    duration

**RidePal** could provide synchronization job for the Deezer Genres; the running period

could be configurable (e.g. each hour synchs the genres from Deezer). In the Admin

panel display the last time the synchronization was ran, the status (success/failure)

and relevant details (e.g. if failure some detailed message of the Exception).

**RidePal** could provide “use top ranks” functionality. Each track has rank score. If this

option is checked the algorithm should pick the highest-ranking tracks from the

tracks pool in the DB and generate a playlist with them (descending from highest to

lowest rank).

**RidePal** could provide “allow tracks from the same artists” functionality. By default,

the algorithm should not allow the same artist to appear twice in a single generated

playlist. This checkbox overrides the default behavior.

**RidePal** could provide play functionality using the preview URL of each track. Use

browser support or JS to bootstrap a player that can stream audio.


**RidePal** could use Pixabay REST service to attach images related to music to newly

generated playlists, for better presentation on the public page.

#### Public Part

The public part of your project must be visible without authentication. This includes
the application start page, the user login and user registration forms, as well as the
list of all user generated playlists. People that are not authenticated cannot see any
user specific details, neither they can interact with the website. They can only browse
the playlists and see the tracks list and details of them.

- Public page must contain at least 3 generated playlists (they can be the top 3
    playlists on the platform)
- Playlists can be clicked/expanded/opened to show the list of artists/tracks
- Playlists show average rank (average of its tracks) and total playtime without
    being opened
- Playlists are sorted by rank and can be filtered by name, genre, and duration

#### Private Part

The private part is accessible only to users who have successfully authenticated
(registered users).

The private part of the web application provides the users with the ability to
generate new playlists, control the generation algorithm, edit, or delete their own

### existing playlists.

Editing existing playlists is limited to changing the title or associated genre tags but

### does not include editing of the track list (e.g. removing or adding individual songs).

#### Administration Part

System administrators can administer all major information objects in the system.
On top of the regular user capabilities, the administrators have the following
capabilities:

- Administrators must be able to edit/delete users and other administrators
- Administrators must be able to edit/delete over the playlists
- Administrators must be able to manually trigger Genre synchronization (if
    that optional requirement is implemented)

#### REST API

Provide a RESTful API that supports the full functionality of the system. (must)


#### External Services

The **RidePal** web application will consume two public REST services to achieve the
main functionality.

##### Microsoft Bing Maps

**Microsoft Bing Maps** offers similar functionality to Google maps but is free for non-
commercial use. For usage details please see the Appendix.

##### Deezer

**Deezer** is a subscription music streaming service like Spotify and Google Play. Its API

usage is free for non-commercial use and does not require any registration. For
usage details please see the Appendix.

#### External Services Data

- Pre-fetch the genres from Deezer and store them in the DB
- Choose at least 3 genres and pre-fetch 1,000 tracks from Deezer and store
    them in the DB
- Each track should have the following properties stored in the DB – **id** , **title** ,
    **link** , **duration** , **rank** , **preview URL** , **artist** ( _id_ , _name_ , _artist track list URL_ ),
    **album** ( _id_ , _name_ , _album track list URL_ )

It’s mandatory to create a relational database model between tracks, artists, albums,
generated playlists etc. and store them in the database.

Each team is advised to choose at least 3 genres from Deezer and pre-fetch at least
1,000 tracks per genre.

The tracks should be stored in the DB and when the algorithm runs, it should read
the tracks from the DB.

_Note: We want to demonstrate the principles of working with external service and
understanding its API and transforming the data from the external service into a
local domain model._

We don’t look for a total completeness (e.g. being able to work with all genres and all
playlists and tracks). For example, the genre “ _Rock_ ” has 300 playlists with each
playlist containing hundreds (about 200) of tracks. The tracks are returned in pages
of 25 items per page. It’s inadvisable to dump all tracks for “ _Rock_ ” (~60K). Doing this
programmatically will most likely exceed the quota.


### Use Cases

#### Main use case

A user travelling from point A to point B wants to have something to listen to during

the duration of the travel. The user wants to generate a track list based on his
musical tastes (the user selects genres from a predefined list). An algorithm, which

uses external service as track sample data, generates the playlist.

### Technical Requirements

#### General

- Follow REST API design best practices when designing the REST API (see
    Appendix)
- Use tiered project structure (separate the application in layers)
- You should implement proper exception handling and propagation
- Try to think ahead. When developing something, think – “How hard would it

##### be to change/modify this later?”

#### Database

The data of the application must be stored in a relational database. You need to
identify the core domain objects and model their relationships accordingly.

Database structure should avoid data duplication and empty data (normalize your
database).

Your repository must include two scripts – one to create the database and one to fill
it with data.

#### Git

Commits in the GitLab repository should give a good overview of how the project
was developed, which features were created first and the people who contributed.
Contributions from all team members must be evident through the git commit

history! The repository must contain the complete application source code and any
scripts (database scripts, for example).

Provide a link to a GitLab repository with the following information in the
README.md file:

- Project description
- Link to the Swagger documentation (must)
- Link to the hosted project (if hosted online)


- Instructions on how to set up and run the project locally
- Images of the database relations (must)

#### Optional Requirements

Besides all requirements marked as should and could, here are some more _optional_
requirements:

- Integrate your project with a Continuous Integration server (e.g., GitLab’s
    own) and configure your unit tests to run on each commit to your master
    branch
- Host your application's backend in a public hosting provider of your choice
    (e.g., AWS, Azure, Heroku)
- Use branches while working with Git

### Teamwork Guidelines

Please see the Teamwork Guidelines document.

### Appendix

- Guidelines for designing good REST API
- Guidelines for URL encoding
- Git commits - an effective style guide
- How to Write a Git Commit Message
- Microsoft Bing Maps External Service Docs
- Deezer Docs

#### Microsoft Bing Maps External Service

We will use this external service to help us calculate the travel duration between two
addresses.

Note: We’re using this service because it’s free. It doesn’t require any payment
information to be used, the registration process is straight forward as well as the
generation and usage of API Key.

Because of the service limitations, for addresses outside of the US, we can only use
travelMode=driving. We can’t use walking or transit. The point is to get familiar with
consuming a REST service, understanding its domain and do some data
transformations.


#### API Key

Each team needs to register at least one account and get a free API key, which will
allow them to make HTTP calls to the REST service. Each API key is limited in the
number of requests etc. so it’s not advisable to share you API key with another team
as it may lead to locking or completely disabling the API key from Bing Maps. The
API key is passed to every HTTP request in “key” query parameter.

#### Integration Guidelines

##### Locations

The first API endpoint that needs to be called is “Locations” and we will use it to find
a Location by address. (official docs). We will use the structured URL form, which
specifies the location query parameters as part of the URL path:

_[http://dev.virtualearth.net/REST/v1/Locations/{countryRegion}/{adminDi](http://dev.virtualearth.net/REST/v1/Locations/{countryRegion}/{adminDi)
strict}/{postalCode}/{locality}/{addressLine}?key=<YOUR_API_KEY>_

For example, the structured URL query for Telerik Academy’s Location will look like:

_[http://dev.virtualearth.net/REST/v1/Locations/BG/Sofia%20City/Mladost/](http://dev.virtualearth.net/REST/v1/Locations/BG/Sofia%20City/Mladost/)
1729/Alexandar%20Malinov%2031?key=<YOUR_API_KEY>_

The result contains a collection of “geocodePoints”. If there’s more than one Geo
Point returned, we want the one of usageType “route”.

Note: You can use the lat/long values returned as a result in any mapping service
(e.g. Google Maps) just to test if your results are correct.

##### Distance Matrix

The next API endpoint that we will use is the Route’s API distance matrix. which can
calculate the time to travel from point A (lat, long) to point B (lat, long) for us. Official
docs. For example, the URL to calculate the distance between The National Place of
Culture and Telerik Academy will look like:

_https://dev.virtualearth.net/REST/v1/Routes/DistanceMatrix?origins=42.
685428619384766,23.318979263305664&destinations=42.6508241,23.3790428&
travelMode=driving&key=<YOUR_API_KEY>_

#### Deezer External Service

Deezer is an audio streaming service, which will help us search and gather the tracks
needed for the playlist generation.

Note: We’re using this service because it’s free. The service does not require
registration, but there’s a quota: “The number of requests per second is limited to 50


requests within 5 seconds.” If you abuse the service, it’s very possible that the IP from
which you’re doing calls will be blocked.

#### Integration Guidelines

##### The resources in the Deezer public API have the following relationship:

##### Genres → Playlists → Tracks

##### Genres

The core genres offered to the user by your application will be seeded from here. It’s
preferable to store the genres in your DB and when needed fetch them from the DB
instead of calling the Deezer service each time.

Playlists

Playlists can be loaded using the following format, which will fetch a list of playlists,
which contain the word <GENRE> in their title.

_https://api.deezer.com/search/playlist?q=<GENRE>_

There’s a total number of existing playlists as well as links to next and previous pages
of results. Each playlist contains a list of tracks, which can be fetched like:

_https://api.deezer.com/playlist/1306931615/tracks_

There’s a total number of tracks in the playlist as well as links to next and previous
pages of results.

#### Pixabay External Service (Optional)

If you want to attach a random music picture over your generated playlists, you can
use this service like:

_https://pixabay.com/api/?key=<YOUR_API_KEY>&q=music_

##### It will return you random music pics which you can associate with your playlists.

### Suggested Approach

- Proof of Concept for work with 3rd party REST APIs
- Test out how to store incoming data
- Carefully design your Database (no user access control at this point)
- Start Implementing the algorithm
- Design and create your user access control tables in the DB
- UI
- Optional Requirements


### Legend

- Must – Implement these first.
- Should – if you have time left, try to implement these.
- Could – only if you are ready with everything else give these a go.