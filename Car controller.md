# Car controller

# Car Controller

A car controller is a component that manages the car's behavior, including **movement**, **collisions**, **visual effects** (VFX), and **sound effects** (SFX), in response to game events.

```csharp
namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        public CarMovement movementController = null;
        public Transform coordinateUI = null;
        public bool keepGettingInput = true;
        private Vector2 _input = Vector2.zero;
        private Vector2 _lastInput = Vector2.zero;

        public Func<Vector2> onGetInputValue = null;

        private void Start()
        {
            this.PubSubRegister(EventID.OnStartGameplay, OnStartGameplay);
            this.PubSubRegister(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnDestroy()
        {
            this.PubSubUnregister(EventID.OnStartGameplay, OnStartGameplay);
            this.PubSubUnregister(EventID.OnFinishGame, OnFinishGame);
        }

        private void Update()
        {
            ControlCoordinate();
        }

        private void OnStartGameplay(object obj)
        {
            ResetController();
        }

        private void OnFinishGame(object obj)
        {
            // stop engine
            ResetController();
            keepGettingInput = false;
        }

      

        protected virtual void ControlCoordinate()
        {
            if (coordinateUI is null || onGetInputValue is null) return;
            _input = keepGettingInput ? onGetInputValue.Invoke() : Vector2.zero;

            Vector3 lookDirection;
            Vector2 input;
            if (_input == Vector2.zero)
            {
                input = _lastInput;
            }
            else
            {
                input = _input;
                _lastInput = _input;
            }
            lookDirection = Camera.main.transform.TransformDirection(input);
            lookDirection.y = 0;
            coordinateUI.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            movementController.DirectionVector = coordinateUI.forward;
        }

        protected virtual void ResetController()
        {
            movementController.ResetMovement();
            _lastInput = Vector2.zero;
            keepGettingInput = true;
        }
    }
}

```

- **Event Handling**
    - Registers and unregisters event listeners for `OnStartGameplay` and `OnFinishGame`.
    - Resets movement at the start of the game and disables input when the game ends.
- **Input Processing**
    - Retrieves input using `onGetInputValue`.
    - If no new input is detected, it maintains the last known direction to prevent abrupt stops.
- **Coordinate System & Rotation**
    - Updates `coordinateUI` to align with the player's movement direction.
    - Converts input into a world-space direction based on the camera's perspective.
    - Adjusts `movementController.DirectionVector` to match the computed direction.

## Car movement

 

```csharp
namespace Game.Car
{
    public class CarMovement : MonoBehaviour
    {
        [LunaPlaygroundField("acceleration", 1, "Car Settings")] public float acceleration = 10;
        [LunaPlaygroundField("maxSpeed", 2, "Car Settings")] public float maxSpeed = 15;
        [LunaPlaygroundField("steerSpeed", 3, "Car Settings")] public float steerSpeed = 100;
        [LunaPlaygroundField("traction", 4, "Car Settings")] public float traction = 1;
        [LunaPlaygroundField("drag", 5, "Car Settings")] public float drag = .98f;
        [LunaPlaygroundField("driftAngleThreshold", 6, "Car Settings")] public float driftAngleThreshold = 10f;
        [LunaPlaygroundField("steeringThreshold", 7, "Car Settings")] public float steeringThreshold = 0.01f;

        public CarController carController = null;

        private Vector3 _directionVector = Vector3.zero;
        public Vector3 DirectionVector
        {
            get { return _directionVector; }
            set { _directionVector = value; }
        }

        public bool KeepGettingInput
        {
            get { return carController.keepGettingInput; }
        }

        private Vector3 _velocity = Vector3.zero;
        public Vector3 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }
        public float CurrentSpeedSqr
        {
            get { return _velocity.sqrMagnitude; }
        }

        public bool IsDrifting
        {
            get { return KeepGettingInput ? Vector3.Angle(_velocity, transform.forward) > driftAngleThreshold : false; }
        }

        private void OnEnable()
        {
            ResetMovement();
        }

        private void Update()
        {
            Move();
            Steer();
            ApplyTraction();
            if (!KeepGettingInput)
            {
                Drag();
            }
        }

        private void Move()
        {
            if (KeepGettingInput)
            {
                float turnAngle = Vector3.Angle(transform.forward, _directionVector);
                float turnFactor = Mathf.Clamp01(turnAngle / 90f);
                float adjustedAcceleration = acceleration * (1 - 0.5f * turnFactor);

                Vector3 accelerationVector = transform.forward * adjustedAcceleration;
                _velocity += accelerationVector * Time.deltaTime;

                if (_velocity.magnitude > maxSpeed)
                {
                    _velocity = _velocity.normalized * maxSpeed;
                }
                
                transform.position += _velocity * Time.deltaTime;
            }
        }

        private void Steer()
        {
            float angle = Vector3.Angle(transform.forward, _directionVector);
            Quaternion targetRotation = Quaternion.LookRotation(_directionVector);

            if (angle > 1f)
            {
                LogUtility.Info("Steering");
                Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);
                transform.rotation = newRotation;
                Drag();
            }
            else
            {
                LogUtility.Info("Stop Steering");
                transform.rotation = targetRotation;
            }
        }

        private void Drag()
        {
            _velocity *= drag;
            if (_velocity.sqrMagnitude < 0.01f)
            {
                _velocity = Vector3.zero;
            }
        }
        
        private void ApplyTraction()
        {
            _velocity = Vector3.Lerp(_velocity.normalized, transform.forward, traction * Time.deltaTime) * _velocity.magnitude;
        }

        internal void ResetMovement()
        {
            _velocity = Vector3.zero;
        }
    }
}
```

The `CarMovement` script handles the physics and movement behavior of the car, controlling acceleration, steering, traction, and drag. It works alongside `CarController` to process input and apply movement mechanics dynamically.

- **Movement Control**
    - Accelerates the car forward based on the `acceleration` value.
    - Limits speed using `maxSpeed`.
    - Adjusts acceleration based on steering intensity to create realistic handling.
- **Steering Mechanism**
    - Rotates the car towards the input direction using `steerSpeed`.
    - Reduces velocity while turning to simulate real-world traction loss.
    - Prevents sharp, unnatural rotations by gradually adjusting the car’s facing direction.
- **Traction & Drag**
    - Applies `traction` to stabilize movement and align velocity with the forward direction.
    - Uses `drag` to gradually slow down the car when there’s no input, preventing infinite momentum.
- **Drifting Detection**
    - Checks if the car's velocity is misaligned with its forward direction.
    - If the angle exceeds `driftAngleThreshold`, the car is considered to be drifting.
- Lamda expression:
    - To ensure compatibility with **Luna Playable**, I removed all lambda expressions from the code. Using explicit method references instead of inline lambdas helps prevent potential compilation issues in the Luna Playable build process.

## Car Collision

```csharp
namespace Game.Car
{
    public class CarCollision : MonoBehaviour
    {
        [LunaPlaygroundField("collisionForce", 8, "Car Settings")] public float collisionForce = 10f; // Adjust this value
        public Rigidbody rb = null;
        public CarMovement movementController = null;
        public Action<Vector3> onCollision = null;
        [SerializeField] private bool _enableDetectingCollision = true;

        private void OnEnable()
        {
            this.PubSubRegister(EventID.OnStartGameplay, OnStartGameplay);
        }

        private void OnDisable()
        {
            this.PubSubUnregister(EventID.OnStartGameplay, OnStartGameplay);
        }

        private void OnStartGameplay(object obj)
        {
            _enableDetectingCollision = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_enableDetectingCollision) return;
            if (other.gameObject.tag == Constants.STR_FINISH_LINE_TAG)
            {
                this.PubSubBroadcast(EventID.OnHitFinishLine, this);
                _enableDetectingCollision = false;
            }
        }

        
        void OnCollisionEnter(Collision collision)
        {
            CarCollision otherCarCollision = collision.gameObject.GetComponent<CarCollision>();

            if (otherCarCollision != null)
            {
                Vector3 pushDirection = collision.transform.position - transform.position;
                pushDirection.Normalize();

                CarMovement otherCarMovement = collision.gameObject.GetComponent<CarMovement>();

                if (movementController != null && otherCarMovement != null)
                {
                    movementController.Velocity -= pushDirection * collisionForce;
                    otherCarMovement.Velocity += pushDirection * collisionForce;

                    if (onCollision != null)
                    {
                        onCollision.Invoke(collision.GetContact(0).point);
                    }
                }
            }
        }
    }
}
```

The `CarCollision` script handles collisions between vehicles and interactions with important game objects, such as the finish line. It ensures proper physics responses when cars collide and triggers necessary events for gameplay progression.

- **Collision Detection**
    - Uses `OnTriggerEnter` to detect when the car crosses the finish line.
    - Uses `OnCollisionEnter` to manage physical interactions with other cars.
- **Finish Line Logic**
    - When the car touches an object tagged as `FINISH_LINE_TAG`, it broadcasts `OnHitFinishLine` and disables further collision detection to prevent multiple triggers.
- **Car-to-Car knock-back effect on Collisions**
    - Detects if the colliding object has a `CarCollision` component.
    - Calculates a **push direction** between the two cars to simulate impact force.
    - Adjusts each car’s velocity by applying `collisionForce` in the appropriate direction.
    - Invokes `onCollision` to allow additional effects (such as visual feedback or sounds).

### CarSFX and CarVFX

Car VFX and SFX are used to enhance the visual and audio feedback based on the car's behavior. When the car's forward vector is misaligned with its velocity (indicating a drift), when its speed exceeds a certain threshold, or when it collides with another car, corresponding sound and visual effects are triggered to create a more immersive experience.