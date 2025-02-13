using Game.Car;
using Game.RaceTrack;
using Game.States;
using Patterns;
using UnityEngine;


namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        #region Hierachy
        [Header("Hierachy")]
        public Transform World;
        public Transform Settings;
        public Transform UI;
        public Transform Managers;
        #endregion


        private StateMachine<GameManager> _stateMachine = new();

        #region States
        private InitGameState _initGameState;
        private StartGameState _startGameState;

        public CarController CarController { get; internal set; }
        [field: SerializeField] public CarSpawner CarSpawner { get; internal set; }
        #endregion




        private void Start()
        {
            CarSpawner = FindObjectOfType<CarSpawner>();

            _initGameState = new(this);
            _startGameState = new(this);

            _stateMachine.Initialize(_initGameState);
        }


        #region Change States
        internal void ChangeToStartGameState()
        {
            _stateMachine.ChangeState(_startGameState);
        }


        #endregion
    }

}
