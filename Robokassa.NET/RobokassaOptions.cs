namespace Robokassa.NET
{
    public class RobokassaOptions
    {
        public string ShopName { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
        public string TestPassword1 { get; set; }
        public string TestPassword2 { get; set; }

        public RobokassaOptions(string shopName, string password1, string password2, string testPassword1, string testPassword2)
        {
            ShopName = shopName;
            Password1 = password1;
            Password2 = password2;
            TestPassword1 = testPassword1;
            TestPassword2 = testPassword2;
        }
        
        public RobokassaOptions(string testPassword1, string testPassword2)
        {
            TestPassword1 = testPassword1;
            TestPassword2 = testPassword2;
        }
    }
}