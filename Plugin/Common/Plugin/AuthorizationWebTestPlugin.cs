namespace Plugin.Common.Plugin
{
    using App.Common.Helpers;
    using App.Common.Http;
    using Connector;
    using LoadTest;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using System.ComponentModel;

    public class AuthorizationExtractionWebTestPlugin: WebTestPlugin
    {
        [Description("Login url")]
        public string Url { get; set; }
        [Description("User name or email")]
        public string UserName { get; set; }
        [Description("Password")]
        public string Password { get; set; }

        public AuthorizationExtractionWebTestPlugin():base()
        {
        }
        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            ITestContext context = new TestContext(e, AuthenticationInfo.Authorization);
            context.Add(TestContextConst.BaseUri, e.WebTest.Context[TestContextConst.BaseUri]);
            context.SetEventArgs(e);
            e.WebTest.Context[TestContextConst.ContextKey] = context;
            //TestContext.Context = context;
            //TestContext.Context.SetEventArgs(e);

            base.PreWebTest(sender, e);
            string loginUri = TemplateHelper.CompileTemplate(this.Url, context);
            var loginRequest = new LoginRequest(this.UserName, this.Password);
            IConnector connector = new RESTConnector();
            IResponseData<LoginResponse> response = connector.Post<LoginRequest, LoginResponse>(loginUri, loginRequest);
            if (response.HasError())
            {
                e.WebTest.Stop();
                return;
            }
            e.WebTest.Context.Add(LoadTestConst.Authorization, response.Data.Authorization);
            AuthenticationInfo.Authorization = new Authorization(response.Data.Authorization, response.Data.AuthToken);

            context= e.WebTest.Context[TestContextConst.ContextKey] as ITestContext;
            context.SetAuthorization(AuthenticationInfo.Authorization);
        }
        public override void PreRequestDataBinding(object sender, PreRequestDataBindingEventArgs e)
        {
            base.PreRequestDataBinding(sender, e);
            ITestContext context = e.WebTest.Context[TestContextConst.ContextKey] as ITestContext;
            context.SetEventArgs(e);
            //TestContext.Context.SetEventArgs(e);
            e.Request.Headers.Add(HtmlHeaderConst.Authorization, AuthenticationInfo.Authorization.Auth);
            e.Request.Url = TemplateHelper.CompileTemplate(e.Request.Url, context);

            //only handle this for http post with body
            if (!(e.Request.Body is StringHttpBody)) { return; }
            StringHttpBody request = e.Request.Body as StringHttpBody;
            string json = request.BodyString;
            json = TemplateHelper.CompileTemplate(json, context);
            request.BodyString = json;
        }
        public override void PostRequest(object sender, PostRequestEventArgs e)
        {
            base.PostRequest(sender, e);
            ITestContext context = e.WebTest.Context[TestContextConst.ContextKey] as ITestContext;
            context.SetEventArgs(e);
            //TestContext.Context.SetEventArgs(e);
        }
        public override void PostWebTest(object sender, PostWebTestEventArgs e)
        {
            base.PostWebTest(sender, e);
        }
    }
}
