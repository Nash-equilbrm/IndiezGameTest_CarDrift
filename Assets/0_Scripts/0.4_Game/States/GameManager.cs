using Game.Car;
using Game.RaceTrack;
using Game.States;
using Patterns;
using System;
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
        private GameplayState _gameplayState;
        private FinishGameState _finishGameState;

        public CarController CarController { get; internal set; }
        [field: SerializeField] public CarSpawner CarSpawner { get; internal set; }
        #endregion




        private void Start()
        {
            CarSpawner = FindObjectOfType<CarSpawner>();

            _initGameState = new(this);
            _startGameState = new(this);
            _gameplayState = new(this);
            _finishGameState = new(this);

            _stateMachine.Initialize(_initGameState);
        }


        #region Change States
        internal void ChangeToStartGameState()
        {
            _stateMachine.ChangeState(_startGameState);
        }

        internal void ChangeToGameplayState()
        {
            _stateMachine.ChangeState(_gameplayState);
        }

        internal void ChangeToFinishGameState()
        {
            _stateMachine.ChangeState(_finishGameState);
        }

        internal void ChangeToInitGameState()
        {
            _stateMachine.ChangeState(_initGameState);
        }


        #endregion
    }

}
