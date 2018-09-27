namespace Plugin.Common.Connector
{
    using App.Common.Http;
    public interface IConnector
    {
        IResponseData<Response> Post<Request, Response>(string loginUri, Request loginRequest);
    }
}
