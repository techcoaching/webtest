namespace Plugin.LoadTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.LoadTesting;
    using App.Common.Http;
    using App.Common.Helpers;
    using System.ComponentModel;
    using Common.Connector;
    using Common;

    public class Login : ILoadTestPlugin
    {
        [Description("Login url")]
        public string Url { get; set; }
        [Description("User name or email")]
        public string UserName { get; set; }
        [Description("Password")]
        public string Password { get; set; }
        public void Initialize(LoadTest loadTest)
        {
            string loginUri = this.Url;
            var loginRequest = new LoginRequest(this.UserName, this.Password);
            IConnector connector = new RESTConnector();
            IResponseData<LoginResponse> response = connector.Post<LoginRequest, LoginResponse>(loginUri, loginRequest);
            if (response.HasError()) {
                loadTest.Abort(new InvalidOperationException(JsonHelper.ToJson(response.Errors)));
                return;
            }
            loadTest.Context.Add(LoadTestConst.Authorization, response.Data.Authorization);
            AuthenticationInfo.Authorization = new Authorization(response.Data.Authorization, response.Data.AuthToken);
        }
    }
}
