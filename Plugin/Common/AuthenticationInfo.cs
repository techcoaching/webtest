namespace Plugin.Common
{
    public class AuthenticationInfo
    {
        public static Authorization Authorization { get; set; }
    }

    public class Authorization
    {
        public string Auth { get; private set; }
        public string AuthToken { get; private set; }
        public Authorization(string auth, string token)
        {
            this.Auth = auth;
            this.AuthToken = token;
        }
    }
}
