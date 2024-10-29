namespace TraktRater.Sites.API.IMDb
{
    using CsvHelper.Configuration;
    using global::TraktRater.TraktAPI.DataStructures;
    using System;

    sealed class IMDbListCsvMap : ClassMap<IMDbListItem>
    {
        public IMDbListCsvMap()
        {
            Map(m => m.Position).Name("Position");
            Map(m => m.Id).Name("Const");
            Map(m => m.CreatedDate).Name("Created");
            Map(m => m.ModifiedDate).Name("Modified");
            Map(m => m.Description).Name("Description");
            Map(m => m.Title).Name("Title");
            Map(m => m.OriginalTitle).Name("Original Title");
            Map(m => m.Url).Name("URL");
            Map(m => m.Type).Name("Title Type");
            Map(m => m.IMDbRating).Name("IMDb Rating");
            Map(m => m.Runtime).Name("Runtime (mins)");
            Map(m => m.Year).Name("Year");
            Map(m => m.Genres).Name("Genres");
            Map(m => m.Votes).Name("Num Votes");
            Map(m => m.ReleaseDate).Name("Release Date");
            Map(m => m.Directors).Name("Directors");
            Map(m => m.MyRating).Name("Your Rating");
            Map(m => m.DateRated).Name("Date Rated");
        }
    }

    class IMDbListItem
    {
        /// <summary>
        /// Example Header as of 29th Oct 2024 (same for watchlist and custom lists)
        /// Position,Const,Created,Modified,Description,Title,Original Title,URL,Title Type,IMDb Rating,Runtime (mins),Year,Genres,Num Votes,Release Date,Directors,Your Rating,Date Rated
        /// </summary>

        public int? Position { get; set; }

        public string Id { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string OriginalTitle { get; set; }

        public string Url { get; set; }

        public string Type { get; set; }

        public string IMDbRating { get; set; }

        public int? Runtime { get; set; }

        public int? Year { get; set; }

        public string Genres { get; set; }

        public int? Votes { get; set; }

        public string ReleaseDate { get; set; }

        public string Directors { get; set; }

        public int? MyRating { get; set; }

        public string DateRated { get; set; }
        
        public TraktMovie ToTraktMovie()
        {
            return new TraktMovie()
            {
                Ids = new TraktMovieId() { ImdbId = Id },
                Title = Title,
                Year = Year
            };
        }

        public TraktShow ToTraktShow()
        {
            return new TraktShow()
            {
                Ids = new TraktShowId() { ImdbId = Id },
                Title = Title,
                Year = Year
            };
        }
    }
}
