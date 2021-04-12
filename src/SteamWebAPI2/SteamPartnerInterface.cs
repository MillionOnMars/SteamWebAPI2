﻿using AutoMapper;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamWebAPI2
{
    /// <summary>
    /// Represents an interface into the Steam Partner Web API
    /// </summary>
    public abstract class SteamPartnerInterface
    {
        private const string steamPartnerApiBaseUrl = "http://partner.steam-api.com/";
        private readonly SteamPartnerRequest steamPartnerRequest;

        protected readonly IMapper mapper;

        /// <summary>
        /// Constructs and maps the default objects for Steam Partner Web API use
        /// </summary>
        public SteamPartnerInterface(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            steamPartnerRequest = new SteamPartnerRequest(steamPartnerApiBaseUrl);
        }

        /// <summary>
        /// Constructs and maps based on a custom http client
        /// </summary>
        /// <param name="httpClient">Client to make requests with</param>
        public SteamPartnerInterface(IMapper mapper, HttpClient httpClient)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            steamPartnerRequest = new SteamPartnerRequest(steamPartnerApiBaseUrl, httpClient);
        }

        /// <summary>
        /// Constructs and maps based on a custom Steam Partner Web API URL
        /// </summary>
        /// <param name="steamPartnerApiBaseUrl">Steam Partner Web API URL</param>
        public SteamPartnerInterface(string steamPartnerApiBaseUrl)
        {
            steamPartnerRequest = new SteamPartnerRequest(steamPartnerApiBaseUrl);
        }

        /// <summary>
        /// Constructs and maps based on a custom http client and custom Steam Partner Web API URL
        /// </summary>
        /// <param name="steamPartnerApiBaseUrl">Steam Partner Web API URL</param>
        /// <param name="httpClient">Client to make requests with</param>
        public SteamPartnerInterface(string steamPartnerApiBaseUrl, HttpClient httpClient)
        {
            steamPartnerRequest = new SteamPartnerRequest(steamPartnerApiBaseUrl, httpClient);
        }

        /// <summary>
        /// Calls a endpoint on the constructed Web API with parameters
        /// </summary>
        /// <typeparam name="T">Type of object which will be deserialized from the response</typeparam>
        /// <param name="endpointName">Endpoint to call on the interface</param>
        /// <param name="parameters">Parameters to pass to the endpoint</param>
        /// <returns>Deserialized response object</returns>
        internal async Task<T> CallMethodAsync<T>(string endpointName, IList<SteamWebRequestParameter> parameters = null)
        {
            Debug.Assert(!string.IsNullOrEmpty(endpointName));

            return await steamPartnerRequest.SendPartnerRequestAsync<T>(endpointName, parameters);
        }
    }
}