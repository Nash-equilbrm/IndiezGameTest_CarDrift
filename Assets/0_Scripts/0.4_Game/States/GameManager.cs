using Commons;
using Game.Audio;
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
