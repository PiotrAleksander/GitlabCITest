namespace WebApiLincora.Helpers
{
    /// <summary>
    /// TopicsMatchingHelper contains functions for performing check if client might publish/subscribe to a topic provided
    /// </summary>
    public class TopicsMatchingHelper
    {
        private static string[] topicsToSubscribe;

        private static string[] topicsToPublish;

        public static string[] TopicsToSubscribe { get => topicsToSubscribe; set => topicsToSubscribe = value; }
        public static string[] TopicsToPublish { get => topicsToPublish; set => topicsToPublish = value; }

        public TopicsMatchingHelper(string[] SubscriptionTopics, string[] PublishingTopics)
        {
            TopicsToSubscribe = SubscriptionTopics;
            TopicsToPublish = PublishingTopics;
        }

        /// <summary>
        /// Validates provided topic syntax
        /// </summary>
        /// <param name="topicToMatch">Topic to validate</param>
        /// <returns>True if topic syntax is allowed on Broker</returns>
        public bool IsTopicAllowed(string Topic, int AccessType)
        {
            string[] topics;

            if (AccessType == 1)
            {
                topics = TopicsToSubscribe;
            } else
            {
                topics = TopicsToPublish;
            }

            foreach (var topic in topics)
            {
                if (TopicMatch(topic, Topic))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Validates topic against provided topic template
        /// </summary>
        /// <param name="TemplateTopic">Template topic used as a pattern</param>
        /// <param name="Topic">Topic to validate</param>
        /// <returns>True if Topic matches TemplateTopic wildcardwise</returns>
        public bool TopicMatch(string TemplateTopic, string Topic)
        {
            var TopicTokens = TemplateTopic.Split('/');
            var ToMatchTokens = Topic.Split('/');
            var topicTokensLen = TopicTokens.Length;
            var toMatchTokensLen = ToMatchTokens.Length;

            for (var i = 0; i < topicTokensLen; i++)
            {
                if (ToMatchTokens[i] == "#" && i == (toMatchTokensLen - 1) && toMatchTokensLen > 1)
                {
                    return true;
                }

                if (TopicTokens[i] != ToMatchTokens[i] && TopicTokens[i] != "{variable}")
                {
                    return false;
                }
            }

            return true;
        }
    }
}
