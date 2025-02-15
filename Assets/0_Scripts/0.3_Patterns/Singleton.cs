using Commons;
using System;
using System.Collections;
using UnityEngine;


namespace Patterns
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                //    if (instance == null)
                //    {
                //        instance = FindAnyObjectByType<T>();
                //        if (instance == null)
                //        {
                //            LogUtility.NotificationInfo($"No {typeof(T).Name} Singleton Instance");
                //        }

                //    }
                return instance;
            }
        }
        protected virtual void Awake()
        {
            CheckInstance();
        }
        public static bool HasInstance => instance != null;

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (T)((object)this);
                DontDestroyOnLoad(this);
                return true;
            }
            if (instance == this)
            {
                DontDestroyOnLoad(this);
                return true;
            }
            Destroy(gameObject);
            return false;
        }

        public static void WaitForInstance(MonoBehaviour context, Action callback)
        {
            context.StartCoroutine(IEWaitForInstance(callback));
        }

        private static IEnumerator IEWaitForInstance(Action callback)
        {
            yield return new WaitUntil(() => HasInstance);
            callback?.Invoke();
        }
    }
}