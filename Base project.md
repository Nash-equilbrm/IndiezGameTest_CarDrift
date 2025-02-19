# Base project

# Render Pipeline: URP

# Design pattern:

- **Singleton**: For game management classes
    
    ```csharp
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        LogUtility.NotificationInfo($"No {typeof(T).Name} Singleton Instance");
                    }
    
                }
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
    ```
    

- **State Machine**: For game states, AI behaviors
    
    ```csharp
     public class StateMachine<T>
     {
         private State<T> _currentState;
         public State<T> CurrentState { get => _currentState; set => _currentState = value; }
    
         private State<T> _previousState;
         public State<T> PreviousState { get => _previousState; set => _previousState = value; }
    
         public void Initialize(State<T> startingState)
         {
             CurrentState = startingState;
             startingState.Enter();
         }
    
         public void ChangeState(State<T> newState)
         {
             CurrentState.Exit();
             PreviousState = CurrentState;
             CurrentState = newState;
             newState.Enter();
         }
     }
    
     public abstract class State<T>
     {
         protected T _context;
         public State(T context)
         {
             _context = context;
         }
    
         public virtual void Enter()
         {
    
         }
    
         public virtual void HandleInput()
         {
    
         }
    
         public virtual void LogicUpdate()
         {
    
         }
    
         public virtual void PhysicsUpdate()
         {
    
         }
    
         public virtual void Exit()
         {
    
         }
     }
    ```
    

- **Publisher - Subcriber:** Handling game events
    
    ```csharp
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
                listeners.Add(id, (obj) => { });
                listeners[id] += action;
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
    ```
    

- **Object Pooling:** For optimizing/recycling game objects quantity instantiated in game
    
    ```csharp
        public class ObjectPooling : Singleton<ObjectPooling>
        {
            [Serializable]
            public class ObjectPool
            {
                [Tooltip("Pool's name"), SerializeField]
                private string name;
                public string Name { get => name; }
                [Tooltip("The object to instantiate"), SerializeField]
                private GameObject prefab;
                [Tooltip("The pool of instantated objects"), SerializeField]
                private List<GameObject> pool = new List<GameObject>();
    
                /// <summary>
                /// Returns a game object from the pool. Instantiate a new one if none is available
                /// </summary>
                /// <param name="position">Position of the object</param>
                /// <param name="rotation">Rotation of the object</param>
                public GameObject Get(Vector3 position = default(Vector3), Vector3 rotation = default(Vector3), Transform parent = null)
                {
                    for (int i = 0; i < pool.Count; i++)
                    {
                        if (!pool[i].activeInHierarchy)
                        {
                            pool[i].transform.SetParent(parent);
                            pool[i].transform.position = position;
                            pool[i].transform.rotation = Quaternion.Euler(rotation);
                            pool[i].SetActive(true);
                            return pool[i];
                        }
                    }
                    var newObject = Instantiate(prefab, position, Quaternion.Euler(rotation));
    #if UNITY_EDITOR
                    newObject.name = $"{name}_{pool.Count}";
    #endif
                    pool.Add(newObject);
                    pool[pool.Count - 1].transform.SetParent(parent);
                    return pool[pool.Count - 1];
                }
    
                /// <summary>
                /// Destroyes currently disabled objects
                /// </summary>
                public void DestroyUnused()
                {
                    for (int i = 0; i < pool.Count; i++)
                    {
                        if (!pool[i].activeInHierarchy)
                        {
                            Destroy(pool[i]);
                            pool.Remove(pool[i]);
                        }
                    }
                }
    
                /// <summary>
                /// Destroyes all objects
                /// </summary>
                public void DestroyAll()
                {
                    for (int i = 0; i < pool.Count; i++)
                        Destroy(pool[i]);
    
                    pool.Clear();
                }
    
                /// <summary>
                /// Recycle all objects in pool
                /// </summary>
                public void RecycleAll()
                {
                    for (int i = 0; i < pool.Count; i++)
                    {
                        pool[i].transform.SetParent(InactiveObjects);
                        pool[i].SetActive(false);
                    }
                }
    
                /// <summary>
                /// Instantiates new objects to the pool
                /// </summary>
                /// <param name="count">Number of objects to prepare</param>
                public void Prepare(int count = 0)
                {
                    if (pool.Count >= count)
                    {
                        RecycleAll();
                        return;
                    }
                    for (int i = 0; i < count; i++)
                    {
                        var newObject = Instantiate(prefab, InactiveObjects);
    #if UNITY_EDITOR
                        newObject.name = $"{name}_{pool.Count}";
    #endif
                        pool.Add(newObject);
                        pool[pool.Count - 1].SetActive(false);
                    }
                }
    
                /// <summary>
                /// Find the first object that satisfied a condition
                /// </summary>
                public GameObject Find(Func<GameObject,bool> condition)
                {
                    return pool.FirstOrDefault(condition);
                }
    
            }
    
            //A transform to store inactive objects
            private static Transform inactiveObjects;
            private static Transform InactiveObjects
            {
                get
                {
                    if (!inactiveObjects)
                    {
                        inactiveObjects = new GameObject("Inactive Objects").transform;
                        inactiveObjects.transform.SetParent(Instance.transform);
                    }
                    return inactiveObjects;
                }
            }
    
            public List<ObjectPool> pools = new List<ObjectPool>();
    
            /// <summary>
            /// Get a pool by its name
            /// </summary>
            /// <param name="name"></param>
            public ObjectPool GetPool(string name)
            {
                foreach (ObjectPool pool in pools)
                {
                    if (pool.Name == name)
                    {
                        return pool;
                    }
                }
                return null;
            }
    
            /// <summary>
            /// Disables the object and moves it under the Pool object
            /// </summary>
            /// <param name="obj">Object to disable</param>
            public static void Remove(GameObject obj)
            {
                obj.SetActive(false);
                obj.transform.SetParent(InactiveObjects);
                obj.transform.localPosition = Vector3.zero;
            }
        }
    ```
    

# UIManager:

- UI prefabs are class classify into 3 types with descending priority: **Screens → Popups → Notifications**
- Use **CanvasGroup** as Require Component to Hide and Show a UI prefab (by changing alpha field) → UI can be reuse without instantiating a new prefab every times we need