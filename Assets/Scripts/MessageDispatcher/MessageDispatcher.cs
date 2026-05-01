namespace MessageDispatcher
{
    using System;
    using System.Collections.Generic;

    public static class MessageDispatcher
    {
        private static Dictionary<Type, Action<IMessage>> _subscribers 
            = new Dictionary<Type, Action<IMessage>>();

        public static void Subscribe<T>(Action<T> handler) where T : IMessage
        {
            Type type = typeof(T);

            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = delegate { };
            }

            _subscribers[type] += (msg) => handler((T)msg);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IMessage
        {
            Type type = typeof(T);

            if (_subscribers.ContainsKey(type))
            {
                _subscribers[type] -= (msg) => handler((T)msg);
            }
        }

        public static void Publish<T>(T message) where T : IMessage
        {
            Type type = typeof(T);

            if (_subscribers.ContainsKey(type))
            {
                _subscribers[type]?.Invoke(message);
            }
        }
    }
}