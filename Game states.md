# Game states

## Game Manager

```csharp
namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        #region Hierachy
        [Header("Hierachy")]
        public Transform World = null;
        public Transform Settings = null;
        public Transform UI = null;
        public Transform Managers = null;
        #endregion
        public CarSpawner carSpawner = null;
        public AudioController audioController = null;

        public string StateName;
        private StateMachine<GameManager> _stateMachine = new StateMachine<GameManager>();

        #region States
        private InitGameState _initGameState = null;
        private StartGameState _startGameState = null;
        private GameplayState _gameplayState = null;
        private FinishGameState _finishGameState = null;

        public CarController CarController { get; set; } = null;
        public CarController OpponentCarController { get; set; } = null;
        public bool PlayerWin { get; set; } = true;
        #endregion

        private void Update()
        {
            StateName = _stateMachine.CurrentState.name;
        }

        private void Start()
        {
            LogUtility.Info("GameManager", "Start");
            _initGameState = new InitGameState(this, "Init State");
            _startGameState = new StartGameState(this, "Start Game State");
            _gameplayState = new GameplayState(this, "Play Game State");
            _finishGameState = new FinishGameState(this, "End Game State");

            _stateMachine.Initialize(_initGameState);
        }

        #region Change States
        public void ChangeToStartGameState()
        {
            LogUtility.Info("GameManager", "ChangeToStartGameState");
            _stateMachine.ChangeState(_startGameState);
        }

        public void ChangeToGameplayState()
        {
            LogUtility.Info("GameManager", "ChangeToGameplayState");
            _stateMachine.ChangeState(_gameplayState);
        }

        public void ChangeToFinishGameState()
        {
            LogUtility.Info("GameManager", "ChangeToFinishGameState");
            _stateMachine.ChangeState(_finishGameState);
        }

        public void ChangeToInitGameState()
        {
            LogUtility.Info("GameManager", "ChangeToInitGameState");
            _stateMachine.ChangeState(_initGameState);
        }
        #endregion
    }

}

```

In this update, I've set up the **GameManager**, which acts as the core controller for managing different game states and key components. Since I'm using a **State Machine**, this makes it easier to transition between various game phases, ensuring a structured and maintainable flow.

---

### **🛠 Hierarchy Management**

The GameManager maintains references to major sections of the game, including:

- **World** – Contains all environment-related objects.
- **Settings** – Stores game settings and configurations.
- **UI** – Manages the game's user interface.
- **Managers** – A container for essential game managers, like audio and car spawning systems.

---

### **🚗 Core Game Components**

- **CarSpawner** – Responsible for spawning cars in the game.
- **AudioController** – Handles all game sounds and effects.
- **CarController & OpponentCarController** – Track the player's and opponent's car behavior.

There's also a **PlayerWin** flag to determine the winner at the end of a race.

---

### **📜 State Machine Implementation**

To structure game flow properly, I've implemented a **State Machine** that transitions between different states:

- **InitGameState** – Handles initial setup when the game starts.
- **StartGameState** – Prepares the race, such as spawning cars and initializing UI.
- **GameplayState** – The main state where the race happens, handling player controls and physics.
- **FinishGameState** – Triggers end-of-race events, such as showing results and determining the winner.

The `StateName` variable keeps track of the current active state for debugging purposes.

---

### **🔄 State Transitions**

The GameManager includes methods for switching between states:

- `ChangeToStartGameState()`
- `ChangeToGameplayState()`
- `ChangeToFinishGameState()`
- `ChangeToInitGameState()`

Each method logs the transition, making debugging easier when tracking state changes.

## Init Game State

The `InitGameState` is responsible for setting up the game before it starts. It initializes core systems like **DOTween**, sets the **target frame rate**, and begins spawning game objects such as cars. Once the game objects are ready, it assigns the **player and opponent controllers** and transitions to the **StartGameState**. This ensures a smooth and structured start before gameplay begins.

```csharp
namespace Game.States
{
    public class InitGameState : State<GameManager>
    {
        private float _initInterval = 1f;
        public InitGameState(GameManager context) : base(context)
        {
        }

        public InitGameState(GameManager context, string name) : base(context, name)
        {
        }

        public override void Enter()
        {
            InitGame();
            StartSpawnGameObjects();
        }

        public override void Exit()
        {
            _context.PubSubUnregister(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
        }

        private void InitGame()
        {
            LogUtility.Info("InitGameState", "InitGame");
            DOTween.Init(recycleAllByDefault: false, useSafeMode: true, LogBehaviour.ErrorsOnly).SetCapacity(50, 10);

            //bool isLowEndDevice = SystemInfo.systemMemorySize < 3000 ||  // Less than 3GB RAM
            //             SystemInfo.processorCount <= 4 ||     // 4 or fewer CPU cores
            //             SystemInfo.graphicsMemorySize < 500;  // Less than 500MB VRAM

            //Application.targetFrameRate = isLowEndDevice ? 30 : 60;
            Application.targetFrameRate = 60;
        }

        private void OnSpawnedGameobjects(object obj)
        {
            var tuple = (Tuple<object, object>)obj;
            _context.CarController = tuple.Item1 as CarController;
            _context.OpponentCarController = tuple.Item2 as CarController;
            _context.ChangeToStartGameState();
        }

        private void StartSpawnGameObjects()
        {
            Sequence seq = DOTween.Sequence().AppendInterval(_initInterval).AppendCallback(OnStartSpawnGameObjects)
                .SetAutoKill(true);
            seq.Play();
        }

        
        private void OnStartSpawnGameObjects()
        {
            _context.PubSubRegister(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
            _context.carSpawner.gameObject.SetActive(true);
            _context.audioController.gameObject.SetActive(true);
            _context.PubSubBroadcast(EventID.OnStartInitGame);
        }

    }

}
```

## Start Game State

`StartGameState` wait for the timer to complete, then initiate the race by transitioning to the `GameplayState`.

```csharp
namespace Game.States
{
    public class StartGameState : State<GameManager>
    {
        public StartGameState(GameManager context) : base(context)
        {
        }

        public StartGameState(GameManager context, string name) : base(context, name)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.PubSubRegister(EventID.OnFinishCounting, OnStartGameplay);
            Tuple<CarController, CarController> data = Tuple.Create(_context.CarController, _context.OpponentCarController);
            UIManager.Instance.ShowOverlap<GameplayOverlap>(data: data, forceShowData: true);
            _context.PubSubBroadcast(EventID.OnGameStartCounting);
        }

        public override void Exit()
        {
            base.Exit();
            _context.PubSubUnregister(EventID.OnFinishCounting, OnStartGameplay);
        }

        private void OnStartGameplay(object obj)
        {
            _context.ChangeToGameplayState();
        }
    }
}

```

## Gameplay State

The `GameplayState` handles the main racing phase. When the state is entered, it registers event listeners and broadcasts the start of gameplay. During the race, it listens for the **OnHitFinishLine** event to determine the winner based on which car reaches the finish line first. Once a winner is identified, the game transitions to the `FinishGameState`.

```csharp
namespace Game.States
{
    public class GameplayState : State<GameManager>
    {
        public GameplayState(GameManager context) : base(context)
        {
        }

        public GameplayState(GameManager context, string name) : base(context, name)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.PubSubRegister(EventID.OnHitFinishLine, OnHitFinishLine);
            _context.PubSubBroadcast(EventID.OnStartGameplay);
        }

        public override void Exit()
        {
            base.Exit();
            _context.PubSubUnregister(EventID.OnHitFinishLine, OnHitFinishLine);
        }

        private void OnHitFinishLine(object obj)
        {
            CarCollision winner = obj as CarCollision;
            if (winner != null)
            {
                _context.PlayerWin = winner.gameObject == _context.CarController.gameObject;
            }
            _context.ChangeToFinishGameState();
        }
    }
}
```

## Finish Game State

The `FinishGameState` handles the race's conclusion. Upon entry, it displays the **ReplayPopup**, showing whether the player won or lost. It also registers the **OnReplayBtnClicked** event, allowing the player to restart the game. When the replay button is clicked, the state transitions back to `InitGameState`, resetting the game for a new race.

```csharp
namespace Game.States
{
    public class FinishGameState : State<GameManager>
    {
        public FinishGameState(GameManager context) : base(context)
        {
        }

        public FinishGameState(GameManager context, string name) : base(context, name)
        {
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.ShowPopup<ReplayPopup>(data: _context.PlayerWin, forceShowData: true);
            _context.PubSubRegister(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
            _context.PubSubBroadcast(EventID.OnFinishGame);
        }

        public override void Exit()
        {
            base.Exit();
            _context.PubSubUnregister(EventID.OnReplayBtnClicked, OnReplayBtnClicked);
        }

        private void OnReplayBtnClicked(object obj)
        {
            _context.ChangeToInitGameState();
        }
    }
}

```