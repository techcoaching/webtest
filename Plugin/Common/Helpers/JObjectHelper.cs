namespace Plugin.Common.Helpers
{
    using System;
    using Newtonsoft.Json.Linq;
    public class JObjectHelper
    {
        internal static string GetValueByPath(JObject json, string path)
        {
            JToken value = json.SelectToken(path);
            return value.ToString();
        }
    }
}
