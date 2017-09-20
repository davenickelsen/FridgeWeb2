namespace FridgeData.Authorization
{
    public interface IPasswordChecker
    {
        bool Check(string login, string password);
    }
}