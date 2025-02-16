using Commons;
using Game.Commons;
using Patterns;
using UnityEngine;


namespace Game.Car
{
    public class CarController : MonoBehaviour
    {
        public CarMovement movementController = null;

        public CarMovement MovementController { get => movementController; set => movementController = value; }

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
            movementController.ResetMovement();
        }

        private void OnFinishGame(object obj)
        {
            // stop engine
            LogUtility.Info("OnFinishGame", "ResetMovement + KeepGettingInput = false");
            movementController.ResetMovement();
            movementController.keepGettingInput = false;
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

