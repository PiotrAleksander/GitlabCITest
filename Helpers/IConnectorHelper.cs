namespace WebApiLincora.Helpers
{
    /// <summary>
    /// Contract for database usage
    /// </summary>
    public interface IConnectorHelper
    {
        void AddUser(string username, string password);
        void RemoveUser(string username);
        void ChangePassword(string username, string password, string newpassword);
        void AddSubscriptionTopics(string topic);
        void AddPublicationTopics(string topic);
    }
}