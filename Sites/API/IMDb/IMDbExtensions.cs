namespace TraktRater.Sites.API.IMDb
{
    using System.Collections.Generic;

    public static class IMDbExtensions
    {
        public static IMDbType ItemType(this Dictionary<string, string> item)
        {
            return item[IMDbFieldMapping.cType].ItemType();
        }

        public static IMDbType ItemType(this string itemType)
        {
            IMDbType retValue = IMDbType.Unknown;

            switch (itemType.Replace(" ", "").ToLower())
            {
                case "video":
                    retValue = IMDbType.Movie;
                    break;
                case "documentary":
                case "tv movie":
                    retValue = IMDbType.Movie;
                    break;
                case "tvshort":
                case "featurefilm":
                case "unknownwork":
                case "movie":
                    retValue = IMDbType.Movie;
                    break;
                case "short":
                    retValue = IMDbType.Movie;
                    break;

                case "tvspecial":
                case "tv mini series":
                    retValue = IMDbType.Show;
                    break;
                case "tvseries":
                    retValue = IMDbType.Show;
                    break;

                case "tv episode":
                    retValue = IMDbType.Episode;
                    break;
                
                default:
                    // most likely a movie
                    retValue = IMDbType.Movie;
                    break;
            }
            return retValue;
        }

        public static bool IsCSVExport(this string provider)
        {
            return provider != "web";
        }
    }
}
