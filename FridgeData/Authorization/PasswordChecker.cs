namespace FridgeData.Authorization
{
    public class PasswordChecker : IPasswordChecker
    {
        public bool Check(string login, string password)
        {
            return login.ToLower() == "dave" && password == "G0Whyte!";
        }
    }
}
