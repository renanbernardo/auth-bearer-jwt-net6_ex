using ShopApi.Models;

namespace ShopApi.Repositories;

public static class UserRepository
{
    public static User Get(string username, string password)
    {
        var users = new List<User>();
        users.Add(new() { Id = 1, Username = "Margareth", Password = "cat123", Role = "manager" });
        users.Add(new() { Id = 1, Username = "Chiquito", Password = "cat321", Role = "employee" });
        return users.FirstOrDefault(x => 
        string.Equals(x.Username, username, StringComparison.CurrentCultureIgnoreCase)
        && x.Password == password);
    }
}