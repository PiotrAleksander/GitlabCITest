namespace WebApiLincora.Helpers
{
    /// <summary>
    /// TopicsMatchingHelper contains functions for performing check if client might publish/subscribe to a topic provided
    /// </summary>
    public class TopicsMatchingHelper
    {
        public static string[] _topicsToSubscribe = new string[] {
            "dev/{variable}/commands/+",
            "dev/+/events/+",
            "sys/+/commands/+",
            "dev/+/replies/+/+",
            "drv/{model]/+/replies/+/+",
            "drv/{variable}/+/events/+",
            "drv/{variable}/+/replies/+/+",
            "sys/+/events/+",
            "sys/+commands/+",
            "sys/+/replies/+/+"
        };

        public static string[] _topicsToPublish = new string[]
        {
            "dev/{variable}/replies/{variable}/{variable}",
            "dev/{variable}/events/{variable}",
            "drv/{variable}/{variable}/events/{variable}",
            "drv/{variable}/{variable}/commands/{variable}",
            "dev/{variable}/commands/{variable}",
            "sys/{variable}/replies/{variable}/{variable}",
            "sys/{variable}/events/{variable}",
            "sys/{variable}/commands/{variable}"
        };


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
                topics = _topicsToSubscribe;
            } else
            {
                topics = _topicsToPublish;
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
