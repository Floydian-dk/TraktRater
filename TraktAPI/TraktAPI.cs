﻿namespace TraktRater.TraktAPI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using global::TraktRater.Extensions;
    using global::TraktRater.TraktAPI.DataStructures;
    using global::TraktRater.Web;

    /// <summary>
    /// Object that communicates with the Trakt API
    /// </summary>
    public static class TraktAPI
    {
        const string ApplicationId = "4feebb4e3791029816a401952c09fa5b446ed4a81b01d600031e422f0d3ae86d";
        const string SecretId = "0d4557136b35ab6234ec3bb659bbcc5b04e7781c4019508496b2b0086cba1fa0";
        const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        const string PinUrlId = "365";
        
        public static string AppId { get { return PinUrlId; } }
        
        /// <summary>
        /// Login to trakt to request a user access token for all subsequent requests              
        /// </summary>
        /// <param name="key">Set this to a PinCode for first time oAuth otherwise your previous Access Token</param>
        /// <returns>If successfully an access token will be returned</returns>
        public static TraktOAuthToken GetOAuthToken(string key)
        {
            // set our required headers now
            TraktWeb.CustomRequestHeaders.Clear();

            TraktWeb.CustomRequestHeaders.Add("trakt-api-key", ApplicationId);
            TraktWeb.CustomRequestHeaders.Add("trakt-api-version", "2");

            string response = TraktWeb.PostToTrakt(TraktURIs.LoginOAuth, GetOAuthLogin(key), true);
            var loginResponse = response.FromJSON<TraktOAuthToken>();

            if (loginResponse == null || loginResponse.AccessToken == null)
                return loginResponse;

            // add the token for authenticated methods
            TraktWeb.CustomRequestHeaders.Add("Authorization", string.Format("Bearer {0}", loginResponse.AccessToken));

            return loginResponse;
        }

        /// <summary>
        /// Gets a oAuth Login object
        /// </summary>
        private static string GetOAuthLogin(string key)
        {
            bool isPinCode = key.Length == 8;

            return new TraktOAuthLogin
                        {
                            Code = isPinCode ? key : null,
                            RefreshToken = isPinCode ? null : key,
                            ClientId = ApplicationId, 
                            ClientSecret = SecretId, 
                            RedirectUri = RedirectUri, 
                            GrantType = isPinCode ? "authorization_code" : "refresh_token"
                        }
                        .ToJSON();
        }
        
        #region Sync to Trakt

        #region Watchlist

        /// <summary>
        /// Sends movie sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToWatchlist(TraktMovieSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends show sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddShowsToWatchlist(TraktShowSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Shows == null || syncData.Shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends episode sync data to Trakt Watchlist
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddEpisodesToWatchlist(TraktEpisodeSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Episodes == null || syncData.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlist, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episodes from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of episodes</param>
        public static TraktSyncResponse RemoveEpisodesFromWatchlist(TraktEpisodeSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all shows from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromWatchlist(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all seasons from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of shows with seasons</param>
        public static TraktSyncResponse RemoveSeasonsFromWatchlist(TraktSeasonSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all movies from watchlist from trakt
        /// </summary>
        /// <param name="syncData">list of movies</param>
        public static TraktSyncResponse RemoveMoviesFromWatchlist(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchlistRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Watched

        /// <summary>
        /// Sends episode watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse AddEpisodesToWatchedHistory(TraktEpisodeWatchedSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Episodes == null || syncData.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episodes for each show in users watched history
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromWatchedHistory(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchedRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Sends movies watched sync data to Trakt
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        public static TraktSyncResponse AddMoviesToWatchedHistory(TraktMovieWatchedSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncWatched, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes movies from users watched history
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveMoviesFromWatchedHistory(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncWatchedRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Rated

        /// <summary>
        /// Rates a list of movies on trakt
        /// </summary>
        /// <param name="data">The object containing the list of movies to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToRatings(TraktMovieRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.movies == null || data.movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Rates a list of shows on trakt
        /// </summary>
        /// <param name="data">The object containing the list of shows to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddShowsToRatings(TraktShowRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.shows == null || data.shows.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Rates a list of episodes on trakt
        /// </summary>
        /// <param name="data">The object containing the list of episodes to be rated</param>       
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddsEpisodesToRatings(TraktEpisodeRatingSync data)
        {
            // check that we have everything we need
            if (data == null || data.Episodes == null || data.Episodes.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncRatings, data.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episode ratings from trakt
        /// </summary>
        /// <param name="syncData">list of episodes</param>
        public static TraktSyncResponse RemoveEpisodesFromRatings(TraktEpisodeSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all show ratings from trakt
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromRatings(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        // <summary>
        /// Removes all season ratings from trakt
        /// </summary>
        /// <param name="syncData">list of shows with seasons</param>
        public static TraktSyncResponse RemoveSeasonsFromRatings(TraktSeasonSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all movie ratings from trakt
        /// </summary>
        /// <param name="syncData">list of movies</param>
        public static TraktSyncResponse RemoveMoviesFromRatings(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncRatingsRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Collection

        /// <summary>
        /// Sends movie sync data to Trakt Collection
        /// </summary>
        /// <param name="syncData">The sync data to send</param>
        /// <returns>The response from trakt</returns>
        public static TraktSyncResponse AddMoviesToCollection(TraktMovieSync syncData)
        {
            // check that we have everything we need
            if (syncData == null || syncData.Movies == null || syncData.Movies.Count == 0)
                return null;

            // serialize data to JSON and send to server
            string response = TraktWeb.PostToTrakt(TraktURIs.SyncCollection, syncData.ToJSON());

            // return success or failure
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes all episodes for each show in users collection
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveShowsFromCollection(TraktShowSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncCollectionRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        /// <summary>
        /// Removes movies from users collection
        /// </summary>
        /// <param name="syncData">list of shows</param>
        public static TraktSyncResponse RemoveMoviesFromCollection(TraktMovieSync syncData)
        {
            if (syncData == null)
                return null;

            var response = TraktWeb.PostToTrakt(TraktURIs.SyncCollectionRemove, syncData.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Pausesd State

        public static bool RemovePausedState(uint id)
        {
            return TraktWeb.DeleteFromTrakt(string.Format(TraktURIs.SyncPausedRemove, id));
        }

        #endregion

        #endregion

        #region Get Current User Data

        #region Ratings

        /// <summary>
        /// Returns the current users Rated Movies
        /// </summary>
        public static IEnumerable<TraktUserMovieRating> GetRatedMovies()
        {
            string ratedMovies = TraktWeb.GetFromTrakt(TraktURIs.RatedMovies);
            var result = ratedMovies.FromJSONArray<TraktUserMovieRating>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Movie.Title != null && r.Movie.Ids != null);
        }

        /// <summary>
        /// Returns the current users Rated Shows
        /// </summary>
        public static IEnumerable<TraktUserShowRating> GetRatedShows()
        {
            string ratedShows = TraktWeb.GetFromTrakt(TraktURIs.RatedShows);
            var result = ratedShows.FromJSONArray<TraktUserShowRating>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        /// <summary>
        /// Returns the current users Rated Episodes
        /// </summary>
        public static IEnumerable<TraktUserEpisodeRating> GetRatedEpisodes()
        {
            string ratedEpisodes = TraktWeb.GetFromTrakt(TraktURIs.RatedEpisodes);
            var result = ratedEpisodes.FromJSONArray<TraktUserEpisodeRating>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        /// <summary>
        /// Returns the current users Rated Seasons
        /// </summary>
        public static IEnumerable<TraktUserSeasonRating> GetRatedSeasons()
        {
            string ratedSeasons = TraktWeb.GetFromTrakt(TraktURIs.RatedSeasons);
            var result = ratedSeasons.FromJSONArray<TraktUserSeasonRating>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        #endregion

        #region Watched

        /// <summary>
        /// Returns the current users watched movies and play counts
        /// </summary>
        public static IEnumerable<TraktMoviePlays> GetWatchedMovies()
        {
            string watchedMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchedMovies);
            var result = watchedMovies.FromJSONArray<TraktMoviePlays>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Movie.Title != null && r.Movie.Ids != null);
        }

        /// <summary>
        /// Returns the current users watched episodes and play counts
        /// </summary>
        public static IEnumerable<TraktShowPlays> GetWatchedShows()
        {
            string watchedShows = TraktWeb.GetFromTrakt(TraktURIs.WatchedShows);
            var result = watchedShows.FromJSONArray<TraktShowPlays>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        #endregion

        #region Watchlist

        /// <summary>
        /// Returns the current users watchlist movies
        /// </summary>
        public static IEnumerable<TraktMovieWatchlist> GetWatchlistMovies()
        {
            string watchlistMovies = TraktWeb.GetFromTrakt(TraktURIs.WatchlistMovies);
            var result = watchlistMovies.FromJSONArray<TraktMovieWatchlist>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Movie.Title != null && r.Movie.Ids != null);
        }

        /// <summary>
        /// Returns the current users watchlist shows
        /// </summary>
        public static IEnumerable<TraktShowWatchlist> GetWatchlistShows()
        {
            string watchlistShows = TraktWeb.GetFromTrakt(TraktURIs.WatchlistShows);
            var result = watchlistShows.FromJSONArray<TraktShowWatchlist>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        /// <summary>
        /// Returns the current users watchlist episodes
        /// </summary>
        public static IEnumerable<TraktEpisodeWatchlist> GetWatchlistEpisodes()
        {
            string watchlistEpisodes = TraktWeb.GetFromTrakt(TraktURIs.WatchlistEpisodes);
            var result = watchlistEpisodes.FromJSONArray<TraktEpisodeWatchlist>();

            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        /// <summary>
        /// Returns the current users watchlist seasons
        /// </summary>
        public static IEnumerable<TraktSeasonWatchlist> GetWatchlistSeasons()
        {
            string watchlistSeasons = TraktWeb.GetFromTrakt(TraktURIs.WatchlistSeasons);
            var result = watchlistSeasons.FromJSONArray<TraktSeasonWatchlist>();

            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        #endregion

        #region Collection

        /// <summary>
        /// Returns the current users collected movies
        /// </summary>
        public static IEnumerable<TraktMovieCollected> GetCollectedMovies()
        {
            string collectedMovies = TraktWeb.GetFromTrakt(TraktURIs.CollectedMovies);
            var result = collectedMovies.FromJSONArray<TraktMovieCollected>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Movie.Title != null && r.Movie.Ids != null);
        }

        /// <summary>
        /// Returns the current users collected episodes
        /// </summary>
        public static IEnumerable<TraktShowCollected> GetCollectedShows()
        {
            string collectedShows = TraktWeb.GetFromTrakt(TraktURIs.CollectedShows);
            var result = collectedShows.FromJSONArray<TraktShowCollected>();
            if (result == null) return null;

            // filter out anything invalid
            return result.Where(r => r.Show.Title != null && r.Show.Ids != null);
        }

        #endregion

        #region Custom Lists

        /// <summary>
        /// Returns all custom lists for a user
        /// </summary>
        /// <param name="username">Username of person's list</param>
        public static IEnumerable<TraktListDetail> GetCustomLists(string username = "me")
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserLists, username));
            return response.FromJSONArray<TraktListDetail>();
        }

        public static bool DeleteCustomList(string listId, string username ="me")
        {
            return TraktWeb.DeleteFromTrakt(string.Format(TraktURIs.UserListDelete, username, listId));
        }

        public static TraktListDetail CreateCustomList(TraktList list, string username = "me")
        {
            var response = TraktWeb.PostToTrakt(string.Format(TraktURIs.UserListAdd, username), list.ToJSON());
            return response.FromJSON<TraktListDetail>();
        }

        public static IEnumerable<TraktListItem> GetCustomListItems(string listId, string username = "me", string extendedInfoParams = "min")
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserListItems, username, listId, extendedInfoParams));
            return response.FromJSONArray<TraktListItem>();
        }

        public static TraktSyncResponse AddItemsToList(string id, TraktSyncAll items, string username = "me")
        {
            var response = TraktWeb.PostToTrakt(string.Format(TraktURIs.UserListItemsAdd, username, id), items.ToJSON());
            return response.FromJSON<TraktSyncResponse>();
        }

        #endregion

        #region Paused State

        public static IEnumerable<TraktSyncPausedMovie> GetPausedMovies()
        {
            var response = TraktWeb.GetFromTrakt(TraktURIs.SyncPausedMovies);
            return response.FromJSONArray<TraktSyncPausedMovie>();
        }

        public static IEnumerable<TraktSyncPausedEpisode> GetPausedEpisodes()
        {
            var response = TraktWeb.GetFromTrakt(TraktURIs.SyncPausedEpisodes);
            return response.FromJSONArray<TraktSyncPausedEpisode>();
        }

        #endregion

        #region Comments
        /// <summary>
        /// Get comments for user sorted by most recent
        /// </summary>
        /// <param name="username">Username of person that made comment</param>
        /// <param name="commentType">all, reviews, shouts</param>
        /// <param name="type"> all, movies, shows, seasons, episodes, lists</param>
        /// <param name="extendedInfoParams">Extended Info: min, full, images (comma separated)</param>
        public static TraktComments GetUsersComments(string username = "me", string commentType = "all", string type = "all", string extendedInfoParams = "min", int page = 1, int maxItems = 50)
        {
            var headers = new WebHeaderCollection();

            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserComments, username, commentType, type, extendedInfoParams, page, maxItems), out headers);
            if (response == null)
                return null;

            try
            {
                return new TraktComments
                {
                    CurrentPage = page,
                    TotalItemsPerPage = maxItems,
                    TotalPages = int.Parse(headers["X-Pagination-Page-Count"]),
                    TotalItems = int.Parse(headers["X-Pagination-Item-Count"]),
                    Comments = response.FromJSONArray<TraktCommentItem>()
                };
            }
            catch
            {
                // most likely bad header response
                return null;
            }
        }
        #endregion

        #region Likes
        /// <summary>
        /// Gets the current users liked items (comments and/or lists)
        /// </summary>
        /// <param name="type">The type of liked item: all (default), lists or comments</param>
        /// <param name="extendedInfoParams">Extended Info: min, full, images (comma separated)</param>
        /// <param name="page">Page Number</param>
        /// <param name="maxItems">Maximum number of items to request per page (this should be consistent per page request)</param>
        public static TraktLikes GetLikedItems(string type = "all", string extendedInfoParams = "min", int page = 1, int maxItems = 10)
        {
            var headers = new WebHeaderCollection();

            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.UserLikedItems, type, extendedInfoParams, page, maxItems), out headers);
            if (response == null)
                return null;

            try
            {
                return new TraktLikes
                {
                    CurrentPage = page,
                    TotalItemsPerPage = maxItems,
                    TotalPages = int.Parse(headers["X-Pagination-Page-Count"]),
                    TotalItems = int.Parse(headers["X-Pagination-Item-Count"]),
                    Likes = response.FromJSONArray<TraktLike>()
                };
            }
            catch
            {
                // most likely bad header response
                return null;
            }
        }
        #endregion

        #endregion

        #region Search Methods

        /// <summary>
        /// Gets the seasons for a show
        /// </summary>
        /// <param name="id">the id of the tv show. Trakt ID, Trakt slug, or IMDb ID Example: game-of-thrones.</param>
        public static IEnumerable<TraktSeasonSummary> GetShowSeasons(string id, string extendedParameter = "full")
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.SeasonSummary, id));
            return response.FromJSONArray<TraktSeasonSummary>();
        }

        /// <summary>
        /// Get the episode details using IMDb ID
        /// </summary>
        /// <param name="id">The IMDb for the episode.</param>"
        /// ToDo: Implement search using other ID's.
        public static IEnumerable<TraktEpisodeDetails> GetEpisodeDetails(string id)
        {
            var response = TraktWeb.GetFromTrakt(string.Format(TraktURIs.IMDBSearch, id));
            return response.FromJSONArray<TraktEpisodeDetails>();
        }

        #endregion
    }
}