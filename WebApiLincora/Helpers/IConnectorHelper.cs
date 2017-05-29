namespace WebApiLincora.Helpers
{
    /// <summary>
    /// Contract for database usage
    /// </summary>
    public interface IConnectorHelper
    {
        bool AddUser(string username, string password);
        bool RemoveUser(string username);
        bool ChangePassword(string username, string password, string newpassword);
        bool AddSubscriptionTopics(string topic);
        bool AddPublicationTopics(string topic);
    }
}