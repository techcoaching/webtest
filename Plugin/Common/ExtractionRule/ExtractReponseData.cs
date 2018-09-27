namespace Plugin.Common.ExtractionRule
{
    using System;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel;
    using Helpers;

    public class ExtractReponseData : Microsoft.VisualStudio.TestTools.WebTesting.ExtractionRule
    {
        [Description("Property path")]
        public string Path { get; set; }
        public override void Extract(object sender, ExtractionEventArgs e)
        {
            try
            {
                string jsonResponse = e.Response.BodyString;
                if (string.IsNullOrWhiteSpace(jsonResponse)) { return; }
                JObject json = JObject.Parse(jsonResponse);
                string value = JObjectHelper.GetValueByPath(json, this.Path);
                e.WebTest.Context.Add(this.ContextParameterName, value);
                ITestContext context = e.WebTest.Context[TestContextConst.ContextKey] as ITestContext;
                context.Add(this.ContextParameterName, value);
                //context.SetEventArgs(e);
                //TestContext.Context.Add(this.ContextParameterName, value);
                e.Success = true;
            }
            catch (Exception ex)
            {
                e.Success = false;
                e.Message = ex.Message;
            }
        }
    }
}
