using Newtonsoft.Json;
using SteamWebAPI2.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SteamWebAPI2
{
    /// <summary>
    /// Represents a request to send to a Steam Partner Web API
    /// </summary>
    internal class SteamPartnerRequest
    {
        private string steamPartnerApiBaseUrl;
        private readonly HttpClient httpClient;

        /// <summary>
        /// Constructs a Steam Partner Web API request
        /// </summary>
        /// <param name="steamPartnerApiBaseUrl">Steam Partner Web API URL</param>
        public SteamPartnerRequest(string steamPartnerApiBaseUrl)
        {
            if (string.IsNullOrEmpty(steamPartnerApiBaseUrl))
            {
                throw new ArgumentNullException("steamPartnerApiBaseUrl");
            }

            this.steamPartnerApiBaseUrl = steamPartnerApiBaseUrl;
        }

        public SteamPartnerRequest(string steamPartnerApiBaseUrl, HttpClient httpClient)
        {
            if (string.IsNullOrEmpty(steamPartnerApiBaseUrl))
            {
                throw new ArgumentNullException("steamPartnerApiBaseUrl");
            }

            this.steamPartnerApiBaseUrl = steamPartnerApiBaseUrl;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Sends a request to a Steam Partner Web API endpoint
        /// </summary>
        /// <typeparam name="T">Type of object which will be deserialized from the response</typeparam>
        /// <param name="endpointName">Endpoint to call on the interface</param>
        /// <returns></returns>
        public async Task<T> SendPartnerRequestAsync<T>(string endpointName)
        {
            Debug.Assert(!string.IsNullOrEmpty(endpointName));

            return await SendPartnerRequestAsync<T>(endpointName, null);
        }

        /// <summary>
        /// Sends a request to a Steam Partner Web API endpoint with parameters
        /// </summary>
        /// <typeparam name="T">Type of object which will be deserialized from the response</typeparam>
        /// <param name="endpointName">Endpoint to call on the interface</param>
        /// <param name="parameters">Parameters to pass to the endpoint</param>
        /// <returns>Deserialized response object</returns>
        public async Task<T> SendPartnerRequestAsync<T>(string endpointName, IList<SteamWebRequestParameter> parameters)
        {
            Debug.Assert(!string.IsNullOrEmpty(endpointName));

            if (parameters == null)
            {
                parameters = new List<SteamWebRequestParameter>();
            }

            string command = BuildRequestCommand(endpointName, parameters);

            string response = await GetHttpStringResponseAsync(command).ConfigureAwait(false);

            var deserializedResult = JsonConvert.DeserializeObject<T>(response);
            return deserializedResult;
        }

        /// <summary>
        /// Returns a string from an HTTP request and removes tabs and newlines
        /// </summary>
        /// <param name="command">Command (method endpoint) to send to an interface</param>
        /// <returns>HTTP response as a string without tabs and newlines</returns>
        private async Task<string> GetHttpStringResponseAsync(string command)
        {
            HttpClient client = httpClient ?? new HttpClient();
            string response = await client.GetStringAsync(command);
            response = response.Replace("\n", "");
            response = response.Replace("\t", "");
            return response;
        }

        /// <summary>
        /// Builds a command to send with a request so that parameters and formats are correct
        /// </summary>
        /// <param name="endpointName">Endpoint to call on the interface</param>
        /// <param name="parameters">Parameters to send to the endpoint</param>
        /// <returns>Deserialized response object</returns>
        public string BuildRequestCommand(string endpointName, IList<SteamWebRequestParameter> parameters)
        {
            Debug.Assert(!string.IsNullOrEmpty(endpointName));

            if (steamPartnerApiBaseUrl.EndsWith("/"))
            {
                steamPartnerApiBaseUrl = steamPartnerApiBaseUrl.Remove(steamPartnerApiBaseUrl.Length - 1, 1);
            }

            string commandUrl = string.Format("{0}/{1}/", steamPartnerApiBaseUrl, endpointName);

            // if we have parameters, join them together with & delimiter and append them to the command URL
            if (parameters != null && parameters.Count > 0)
            {
                string parameterString = string.Join("&", parameters);
                commandUrl += string.Format("?{0}", parameterString);
            }

            return commandUrl;
        }
    }
}