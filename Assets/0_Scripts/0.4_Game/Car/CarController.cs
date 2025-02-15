using Commons;
using Game.Commons;
using Patterns;
using UnityEngine;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        [SerializeReference] private CarMovement _movementController;

        public CarMovement MovementController { get => _movementController; set => _movementController = value; }

        private void Start()
        {
            this.Register(EventID.OnStartGameplay, OnStartGameplay);
            this.Register(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnDestroy()
        {
            this.Unregister(EventID.OnStartGameplay, OnStartGameplay);
            this.Unregister(EventID.OnFinishGame, OnFinishGame);
        }

        private void OnStartGameplay(object obj)
        {
            LogUtility.Info("OnStartGameplay", "ResetMovement");
            _movementController.ResetMovement();
        }

        private void OnFinishGame(object obj)
        {
            // stop engine
            LogUtility.Info("OnFinishGame", "ResetMovement + KeepGettingInput = false");
            _movementController.ResetMovement();
            _movementController.KeepGettingInput = false;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == Constants.STR_FINISH_LINE_TAG)
            {
                LogUtility.Info("CarController", "Hit Finish Line");
                this.Broadcast(EventID.OnHitFinishLine);
            }
        }
    }
}

