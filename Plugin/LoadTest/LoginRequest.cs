namespace Plugin.LoadTest
{
    public class LoginRequest
    {
        public string Password { get; private set; }
        public string UserName { get; private set; }
        public LoginRequest(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}
