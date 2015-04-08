﻿namespace TraktRater.Sites.API.TVDb
{
    using System;
    using System.IO;

    public static class TVDbCache
    {
        static readonly string cAppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static readonly string cEpisodeRatingsFileCache = cAppDir + @"\TraktRater\Ratings\{0}.xml";
        public static readonly string cShowRatingsFileCache = cAppDir + @"\TraktRater\Ratings\series.xml";
        public static readonly string cShowInfoFileCache = cAppDir + @"\TraktRater\Series\{0}.xml";
        public static readonly string cShowSearchFileCache = cAppDir + @"\TraktRater\SearchResults\tvdb_{0}.xml";

        public static string GetFromCache(string filename, int expiresInDays = 1)
        {
            try
            {
                if (!File.Exists(filename)) return null;

                // if cache is older than X days disregard
                if (File.GetLastWriteTime(filename) <= DateTime.Now.Subtract(TimeSpan.FromDays(expiresInDays)))
                    return null;

                return File.ReadAllText(filename);
            }
            catch { return null; }
        }

        public static void CacheResponse(string response, string filename)
        {
            if (response == null) return;

            try
            {
                string dir = Path.GetDirectoryName(filename);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(filename, response);
            }
            catch (Exception)
            {
            }
        }

        public static void DeleteFromCache(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (Exception)
            {
            }
        }
    }
}
