using System;
using System.Collections.Generic;
using Steam.Models.SteamStore;
using System.Threading.Tasks;
using Steam.Models.SteamPartner;

namespace SteamWebAPI2.Interfaces
{
    internal interface ISteamPartner
    {
        Task<PartnerUserStatsForGameModel> SetPartnerUserStatsForGame(UInt64 steamId, Dictionary<string, UInt32> achievements);
        
        // Task<StoreAppDetailsDataModel> GetStoreAppDetailsAsync(uint appId, string cc = "");
        // Task<StoreFeaturedCategoriesModel> GetStoreFeaturedCategoriesAsync();
        // Task<StoreFeaturedProductsModel> GetStoreFeaturedProductsAsync();
    }
}