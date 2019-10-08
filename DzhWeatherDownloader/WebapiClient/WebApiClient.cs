using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DzhWeatherDownloader.WebapiClient
{
    public class WebApiClient
    {
        protected HttpClient _client;
        public string ControllerUrl { get; private set; }

        public WebApiClient(int timeout=15)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(timeout);
        }

        public WebApiClient (string server,string prefix ,string controller,int timeout=15):this(timeout)
        {
            ControllerUrl = $"{prefix}{controller}";
            _client.BaseAddress = new Uri(server);
        }

        public WebApiClient(string server, string controller, int timeout = 15) : this(server,"api/",controller,timeout)
        {
        }

        public TResult Query<TResult>(string url, object para)
        {
            return Task.Run(() => QueryAsync<TResult>(url, para)).WaitForResult();
        }

        private Task<TResult> QueryAsync<TResult>(string url, object para)
        {
            url = $"{ControllerUrl}{url}{formatPara(para)}";
            return processAsync<TResult>(_client.GetAsync(url));
        }

        protected string formatPara(object para)
        {
            if (para == null) return string.Empty;
            RouteValueDictionary dic = new RouteValueDictionary(para);
            return $"?{string.Join("$", dic.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value?.ToString())}"))}";
        }

        public sealed class VoidResult { }


        protected void process(Task<HttpResponseMessage> operation)
        {
            Task.Run(() => processAsync(operation)).Wait();
        }

        protected TResult process<TResult>(Task<HttpResponseMessage> operation)
        {
            return Task.Run(() => processAsync<TResult>(operation)).WaitForResult();
        }

        private async Task processAsync(Task<HttpResponseMessage> operation)
        {
            await processAsync<VoidResult>(operation);
        }

        private async Task<TResult> processAsync<TResult>(Task<HttpResponseMessage> operation)
        {
            using (var rsp = await processAsyncNeedDispose(operation))
            {
                if (typeof(TResult) == typeof(VoidResult)) return default;
                return await rsp.Content.ReadAsAsync<TResult>();
            }
        }

        private async Task<HttpResponseMessage> processAsyncNeedDispose(Task<HttpResponseMessage> operation)
        {
            try
            {
                var rsp = await operation;
                HttpError.CheckResponse(rsp);
                return rsp;
            }
            catch (AggregateException e)
            {
                throw e.InnerException;
            }
            catch(TaskCanceledException cancelException)
            {
                throw cancelException;
            }
        }
    }

    public class WebApiClient<TData, TKey> : WebApiClient
    {
        private Func<TData, TKey> _idSelector = null;
        private Action<TData, TKey> _idWriter = null;
        public WebApiClient(string server, string prefix, string controller, Func<TData, TKey> idSelector, Action<TData, TKey> idWriter, int timeout = 15) : base(server, prefix, controller, timeout)
        {
            _idSelector = idSelector;
            _idWriter = idWriter;
        }
        public WebApiClient(string server, string controller, Func<TData, TKey> idSelector, Action<TData, TKey> idWriter, int timeout = 15) : this(server, "api/", controller, idSelector, idWriter, timeout)
        {

        }
        public TData QueryData(string url, object para)
        {
            return Query<TData>(url, para);
        }
        public TData[] QueryDataList(string url, object para)
        {
            return Query<TData[]>(url, para);
        }
        public virtual void Create(TData data)
        {
            TData result = process<TData>(_client.PostAsJsonAsync(ControllerUrl, data));
            _idWriter(data, _idSelector(result));
        }
        public virtual TData[] LoadAll()
        {
            return QueryDataList(string.Empty, null);
        }
        public virtual TData Find(TKey id)
        {
            return QueryData($"/{formatId(id)}", null);
        }
        public virtual TData[] FindAll(TKey key)
        {
            return QueryDataList(string.Empty, new { key = key });
        }
        public virtual void Remove(TKey id)
        {
            string url = $"{ControllerUrl}/{formatId(id)}";
            process(_client.DeleteAsync(url));
        }
        public virtual void Update(TData data)
        {
            process(_client.PutAsJsonAsync(ControllerUrl, data));
        }

        private string formatId(TKey id)
        {
            string url = id.ToString();
            if (url.Contains(":"))
                return HttpUtility.UrlEncode(url);
            else
                return url;
        }
    }
}
