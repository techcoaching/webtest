namespace Plugin.Common.Connector
{
    using System;
    using App.Common.Http;
    using App.Common;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using App.Common.Connector;
    using App.Common.MVC.Attributes;
    using App.Common.Helpers;

    public class RESTConnector : IConnector
    {
        public IResponseData<Response> Post<Request, Response>(string uri, Request request)
        {
            Uri url = new Uri(uri);
            string baseUrl = this.GetBaseUri(uri);
            using (HttpClient client = this.CreateHttpClient(baseUrl))
            {
                HttpContent content = this.GetContent(request);
                HttpResponseMessage responseMessage = client.PostAsync(uri, content).Result;
                IResponseData<Response> result = this.GetResponseAs<ResponseData<Response>>(responseMessage.Content);
                return result;
            }
        }

        private TResponse GetResponseAs<TResponse>(HttpContent content)
        {
            if (content.Headers.ContentType.MediaType == HttpMediaType.TextHtml)
            {
                throw new UnsupportedMediaTypeException(content.ReadAsStringAsync().Result, content.Headers.ContentType);
            }
            string result = content.ReadAsStringAsync().Result;
            return JsonHelper.ToObject<TResponse>(result);
        }

        private HttpContent GetContent<TData>(TData data)
        {
            return new JsonContent<TData>(data);
        }

        private HttpClient CreateHttpClient(string baseUrl, object extArgs = null)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ConnectorContentType.Json));
            AuthenticationHeaderValue authorizationHeader = this.GetAuthorizationHeader(extArgs);
            if (authorizationHeader != null)
            {
                client.DefaultRequestHeaders.Authorization = authorizationHeader;
            }
            return client;
        }

        private AuthenticationHeaderValue GetAuthorizationHeader(object extArgs)
        {
            HttpAuthorizationAttribute author = ObjectHelper.GetClassAttribute<HttpAuthorizationAttribute>(extArgs);
            if (author != null)
            {
                return AuthenticationHeaderValue.Parse(author.ToString());
            }
            return null;
        }

        private string GetBaseUri(string uri)
        {
            Uri url = new Uri(uri);
            if (url.Port == HttpConst.DEFAULT_HTTP_PORT)
            {
                return string.Format("{0}://{1}", url.Scheme, url.Host);
            }
            return string.Format("{0}://{1}:{2}", url.Scheme, url.Host, url.Port);
        }
    }
}
