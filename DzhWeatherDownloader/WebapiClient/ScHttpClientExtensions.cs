using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DzhWeatherDownloader.WebapiClient
{
    public static class ScHttpClientExtensions
    {
        /// <summary>
        /// Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        /// <remarks>This method uses a default instance of System.Net.Http.Formatting.JsonMediaTypeFormatter.</remarks>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return client.PostAsJsonAsync(requestUri, value, CancellationToken.None);
        }
        /// <summary>
        /// Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        /// <remarks>This method uses a default instance of System.Net.Http.Formatting.JsonMediaTypeFormatter.</remarks>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value, CancellationToken cancellationToken)
        {
            return client.PostAsJsonAsync(new Uri(client.BaseAddress, requestUri), value, cancellationToken);
        }

        /// <summary>
        /// Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        /// <remarks>This method uses a default instance of System.Net.Http.Formatting.JsonMediaTypeFormatter.</remarks>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            return client.PostAsJsonAsync(requestUri, value, CancellationToken.None);
        }

        /// <summary>
        /// Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="client">The client used to make the request.</param>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="value">The value that will be placed in the request's entity body.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task object representing the asynchronous operation.</returns>
        /// <remarks>This method uses a default instance of System.Net.Http.Formatting.JsonMediaTypeFormatter.</remarks>
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, Uri requestUri, T value, CancellationToken cancellationToken)
        {
            var s = JsonConvert.SerializeObject(value);
            StringContent content = new StringContent(s);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return client.PostAsync(requestUri, content, cancellationToken);
        }
    }
}
