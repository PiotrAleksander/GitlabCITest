using StackExchange.Redis;
using System;

namespace WebApiLincora.Helpers
{
    public class RedisConnectorHelper : IConnectorHelper
    {
        static int USERS_DATABASE = 0;
        static int TOPICS_DATABASE = 1;

        static RedisConnectorHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("172.16.40.121:6379");
            });

            SubscriptionTopics = Connection.GetDatabase(TOPICS_DATABASE).SetMembers("SubscriptionTopics").ToStringArray();
            PublicationTopics = Connection.GetDatabase(TOPICS_DATABASE).SetMembers("PublicationTopics").ToStringArray();
        }

        public static string[] SubscriptionTopics { get; internal set; }
        public static string[] PublicationTopics { get; internal set; }

        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        public static IDatabase Cache
        {
            get
            {
                return Connection.GetDatabase(USERS_DATABASE);
            }
        }

        public void AddSubscriptionTopics(string topic)
        {
            Connection.GetDatabase(TOPICS_DATABASE).SetAdd("SubscriptionTopics", topic);
        }

        public void AddPublicationTopics(string topic)
        {
            Connection.GetDatabase(TOPICS_DATABASE).SetAdd("PublicationTopics", topic);
        }

        public void AddUser(string username, string password)
        {
            Connection.GetDatabase(USERS_DATABASE).StringSet(username, password);
        }

        public void RemoveUser(string username)
        {
            Connection.GetDatabase(USERS_DATABASE).KeyDelete(username);
        }

        public void ChangePassword(string username, string password, string newpassword)
        {
            var cache = Connection.GetDatabase(USERS_DATABASE);
            if (cache.StringGet(username) == password)
            {
                cache.StringSet(username, newpassword);
            }
        }
    }
}
