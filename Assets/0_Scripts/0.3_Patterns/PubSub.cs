using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Patterns
{
    public class PubSub : Singleton<PubSub>
    {
        private Dictionary<EventID, Action<object>> listeners = new Dictionary<EventID, Action<object>>();

        #region Register, Unregister, Broadcast
        public void Register(EventID id, Action<object> action)
        {
            if (action == null) { return; }
            if (listeners.ContainsKey(id))
            {
                if (listeners[id] != null)
                    if (!listeners[id].GetInvocationList().Contains(action))
                        listeners[id] += action;
            }
            else
            {
                listeners.Add(id, action);
            }
        }
        public void Unregister(EventID id, Action<object> action)
        {

            if (listeners.ContainsKey(id) && action != null)
            {
                if (listeners[id].GetInvocationList().Contains(action))
                    listeners[id] -= action;
            }
        }
        public void UnregisterAll(EventID id)
        {
            if (listeners.ContainsKey(id))
            {
                listeners.Remove(id);
            }
        }
        public void Broadcast(EventID id, object? data)
        {
            if (listeners.ContainsKey(id))
            {
                listeners[id].Invoke(data);
            }
        }
        #endregion
    }
    public static class PubSubExtension
    {
        public static void Register(this MonoBehaviour listener, EventID id, Action<object> action)
        {
            if (PubSub.HasInstance)
            {
                PubSub.Instance.Register(id, action);
            }
            else LogUtility.Error("Register", "No Instance");
        }
        public static void Unregister(this MonoBehaviour listener, EventID id, Action<object> action)
        {
            if (PubSub.HasInstance)
            {
                PubSub.Instance.Unregister(id, action);
            }
        }
        public static void UnregisterAll(this MonoBehaviour listener, EventID id)
        {
            if (PubSub.HasInstance)
            {
                PubSub.Instance.UnregisterAll(id);
            }
        }
        public static void Broadcast(this MonoBehaviour listener, EventID id)
        {
            if (PubSub.HasInstance)
            {
                PubSub.Instance.Broadcast(id, null);
            }
        }
        public static void Broadcast(this MonoBehaviour listener, EventID id, object data)
        {
            if (PubSub.HasInstance)
            {
                PubSub.Instance.Broadcast(id, data);
            }
        }
    }
}