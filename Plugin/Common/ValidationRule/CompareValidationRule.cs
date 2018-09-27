namespace Plugin.Common.ValidationRule
{
    using Helpers;
    using Microsoft.VisualStudio.TestTools.WebTesting;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel;

    public class CompareValidationRule : Microsoft.VisualStudio.TestTools.WebTesting.ValidationRule
    {
        [Description("Check if response was empty")]
        public bool CheckEmptyResponse { get; set; }
        [Description("Value to compare")]
        public string ExpectedValue { get; set; }
        [Description("Property to get value")]
        public string Path { get; set; }

        public override void Validate(object sender, ValidationEventArgs e)
        {
            string response = e.Response.BodyString;
            if (this.CheckEmptyResponse == true && string.IsNullOrWhiteSpace(response))
            {
                e.IsValid = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(response))
            {
                e.IsValid = false;
                e.Message = "Response content was empty";
                return;
            }
            JObject json = JObject.Parse(response);
            string value = JObjectHelper.GetValueByPath(json, this.Path);
            e.IsValid = value == this.ExpectedValue;
        }
    }
}
