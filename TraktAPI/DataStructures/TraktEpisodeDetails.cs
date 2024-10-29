using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TraktRater.TraktAPI.DataStructures
{
    [DataContract]
    public class TraktEpisodeDetails
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "score")]
        public uint Score { get; set; }

        [DataMember(Name = "episode")]
        public TraktEpisodeSummary Episode { get; set; }
    }
}
