using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ToolBox;

namespace Azure.ServiceBus
{
    public class ServiceBusManager
    {
        private const int DefaultMessagePriority = 500;
        private const long MaximumMbForTopic = 5120;

        private readonly string _connectionString;
        private bool _initialized = false;

        public NamespaceManager Namespace => 
            NamespaceManager.CreateFromConnectionString(_connectionString);

        public ServiceBusManager(string connectionString)
        {
            _connectionString = connectionString;
            ServiceBusEnvironment.SystemConnectivity.Mode = ConnectivityMode.Http;
        }

        public static BrokeredMessage GetMessage<T>(T t, int category) =>
            GetMessage(t, category, DefaultMessagePriority);
        
        public static BrokeredMessage GetMessage<T>(T t, int category, int priority) =>
            new BrokeredMessage(JsonHelper.ToJson(t))
            {
                Label = typeof(T).ToString(),
                Properties =
                {
                    {"LogLevel", 1},
                    {"Priority", priority},
                    {"Category", category},
                }
            };

        public SubscriptionClient GetSubscription(string topicName, string subscriptionName) =>
            !IsInitialzied(topicName) || !Namespace.SubscriptionExists(topicName, subscriptionName)
                ? null
                : SubscriptionClient.CreateFromConnectionString(_connectionString, topicName, subscriptionName);

        public SubscriptionClient CreateSubscription(string topicName, string subscriptionName) =>
            CreateSubscription(_connectionString, topicName, subscriptionName, null, 0);
        
        public SubscriptionClient CreateSubscription(string connectionString, string topicName, string subscriptionName, List<int> categoryList, int minutesToLive = 0)
        {
            if (!IsInitialzied(topicName)) return null;
            if (Namespace.SubscriptionExists(topicName, subscriptionName)) throw new MessagingEntityAlreadyExistsException(subscriptionName);

            var subscriptionDescription = new SubscriptionDescription(topicName, subscriptionName);

            if (minutesToLive > 0)
            { subscriptionDescription.AutoDeleteOnIdle = new TimeSpan(0, minutesToLive, 0); }

            if (!(categoryList?.Any() ?? false))
            { Namespace.CreateSubscription(subscriptionDescription); }
            else
            {
                var textFilter = $"user.Category IN ({string.Join(";", categoryList.Select(category => category.GetHashCode()))})";
                Namespace.CreateSubscription(subscriptionDescription, new SqlFilter(textFilter));
            }

            return SubscriptionClient.CreateFromConnectionString(_connectionString, topicName, subscriptionName);
        }
        
        internal void DeleteSubscription(string topicName, string subscriptionName) =>
            Namespace.DeleteSubscription(topicName, subscriptionName);
        
        internal SubscriptionDescription GetSubscriptionDescription(string topicName, string subscriptionName) => 
            Namespace.GetSubscription(topicName, subscriptionName);
 
        internal List<RuleDescription> GetSubscriptionRules(string topicName, string subscriptionName) =>
            Namespace.GetRules(topicName, subscriptionName).ToList();

        private bool IsInitialzied(string topicName) =>
            _initialized
                ? _initialized
                : CreateTopicIfNotExist(topicName, out _initialized);

        private bool CreateTopicIfNotExist(string topicName, out bool initialized)
        {
            try
            {
                if (!Namespace.TopicExists(topicName))
                {
                    var topicDescription = new TopicDescription(topicName)
                    {
                        EnablePartitioning = false,
                        MaxSizeInMegabytes = MaximumMbForTopic
                    };

                    Namespace.CreateTopic(topicDescription);
                }

                initialized = true;
            }
            catch (Exception)
            { initialized = false; }

            return initialized;
        }
    }
}
