namespace Plugin.Common
{
    using App.Common.Helpers;
    using System;
    using System.Collections.Generic;

    public class TestContext : ITestContext
    {
        public IDictionary<string, string> Settings { get; protected set; }
        public Authorization Auth { get; private set; }
        public EventArgs EventArgs { get; private set; }
        public Guid Id { get; private set; }
        //public static ITestContext Context { get; set; }

        public TestContext(EventArgs ev, Authorization auth)
        {
            this.Id = GuidHelper.CreateNew();
            this.EventArgs = ev;
            this.Auth = auth;
            this.Settings = new Dictionary<string, string>();
        }

        public void SetEventArgs(EventArgs ev)
        {
            this.EventArgs = ev;
        }

        public bool Exist(string key)
        {
            return this.Settings.ContainsKey(key);
        }

        public string Get(string key)
        {
            if (!this.Exist(key)) { return string.Empty; }
            return this.Settings[key];
        }

        public void Add(string key, object value)
        {
            this.Settings[key] = value != null ? value.ToString() : string.Empty;
        }

        public void SetAuthorization(Authorization authorization)
        {
            this.Auth = authorization;
        }
    }
}
