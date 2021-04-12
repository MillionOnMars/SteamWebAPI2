using Newtonsoft.Json;

namespace SteamWebAPI2.Models.SteamPartner
{
    internal class PartnerUserStatsForGameResultContainer
    {
        [JsonProperty("result")]
        public uint Result { get; set; }
    }
}
