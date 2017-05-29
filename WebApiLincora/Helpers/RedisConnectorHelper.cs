using StackExchange.Redis;
using System;

namespace WebApiLincora.Helpers
{
    public class RedisConnectorHelper : IConnectorHelper
    {
        static int USERS_DATABASE = 0;
        static int TOPICS_DATABASE = 1;
        /// <summary>
        /// Class providing methods to connect Redis instance
        /// </summary>
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
        /// <summary>
        /// Creates connection to the Redis database
        /// </summary>
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

        public bool AddSubscriptionTopics(string topic)
        {
            return Connection.GetDatabase(TOPICS_DATABASE).SetAdd("SubscriptionTopics", topic);
        }

        public bool AddPublicationTopics(string topic)
        {
            return Connection.GetDatabase(TOPICS_DATABASE).SetAdd("PublicationTopics", topic);
        }

        public bool AddUser(string username, string password)
        {
            return Connection.GetDatabase(USERS_DATABASE).StringSet(username, password);
        }

        public bool RemoveUser(string username)
        {
            return Connection.GetDatabase(USERS_DATABASE).KeyDelete(username);
        }

        public bool ChangePassword(string username, string password, string newpassword)
        {
            var cache = Connection.GetDatabase(USERS_DATABASE);
            if (cache.StringGet(username) == password)
            {
                return cache.StringSet(username, newpassword);
            }
            return false;
        }
    }
}
