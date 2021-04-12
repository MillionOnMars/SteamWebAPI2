using System;
using AutoMapper;
using Steam.Models.SteamPartner;
using SteamWebAPI2.Models.SteamPartner;
using SteamWebAPI2.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Steam.Models.SteamStore;
using SteamWebAPI2.Models.SteamStore;

namespace SteamWebAPI2.Interfaces
{
    public class SteamPartner : ISteamPartner
    {
        private readonly ISteamWebInterface steamWebInterface;
        private readonly IMapper mapper;
        private readonly AppId appId;
        
        public SteamPartner(IMapper mapper, ISteamWebRequest steamWebRequest, AppId appId, ISteamWebInterface steamWebInterface = null)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.appId = appId;
            
            this.steamWebInterface = steamWebInterface == null
                ? new SteamWebInterface("ISteamUserStats", steamWebRequest)
                : steamWebInterface;
        }

        public async Task<PartnerUserStatsForGameModel> SetPartnerUserStatsForGame(ulong steamId, Dictionary<string, uint> achievements)
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();

            parameters.AddIfHasValue(appId, "appid");
            parameters.AddIfHasValue(steamId, "steamid");
            parameters.AddIfHasValue(achievements.Count, "count");
            parameters.AddIfHasValue(achievements.Keys.ToArray()[0], "name[0]");
            parameters.AddIfHasValue(achievements.Values.ToArray()[0], "value[0]");

            var stats = await steamWebInterface.PostAsync<PartnerUserStatsForGameResultContainer>("SetUserStatsForGame", 1, parameters);
            var statsModel = mapper.Map<PartnerUserStatsForGameResultContainer, PartnerUserStatsForGameModel>(stats.Data);
            return statsModel;
        }

#if UNUSED_CODE
        /// <summary>
        /// Maps to the steam store api endpoint: GET http://store.steampowered.com/api/appdetails/
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public async Task<StoreAppDetailsDataModel> GetStoreAppDetailsAsync(uint appId, string cc = "")
        {
            List<SteamWebRequestParameter> parameters = new List<SteamWebRequestParameter>();

            parameters.AddIfHasValue(appId, "appids");
            parameters.AddIfHasValue(cc, "cc");

            var appDetails = await CallMethodAsync<AppDetailsContainer>("appdetails", parameters);

            var appDetailsModel = mapper.Map<Data, StoreAppDetailsDataModel>(appDetails.Data);

            return appDetailsModel;
        }

        /// <summary>
        /// Maps to the steam store api endpoint: GET http://store.steampowered.com/api/featuredcategories/
        /// </summary>
        /// <returns></returns>
        public async Task<StoreFeaturedCategoriesModel> GetStoreFeaturedCategoriesAsync()
        {
            var featuredCategories = await CallMethodAsync<FeaturedCategoriesContainer>("featuredcategories");

            var featuredCategoriesModel = mapper.Map<FeaturedCategoriesContainer, StoreFeaturedCategoriesModel>(featuredCategories);

            return featuredCategoriesModel;
        }

        /// <summary>
        /// Maps to the steam store api endpoint: GET http://store.steampowered.com/api/featured/
        /// </summary>
        /// <returns></returns>
        public async Task<StoreFeaturedProductsModel> GetStoreFeaturedProductsAsync()
        {
            var featuredProducts = await CallMethodAsync<FeaturedProductsContainer>("featured");

            var featuredProductsModel = mapper.Map<FeaturedProductsContainer, StoreFeaturedProductsModel>(featuredProducts);

            return featuredProductsModel;
        }
#endif
    }
}