using DG.Tweening;
using Game.Car;
using Patterns;
using System;
using System.Collections;
using Commons;
using UnityEngine;


namespace Game.States
{
    public class InitGameState : State<GameManager>
    {
        private float _initInterval = 1f;
        public InitGameState(GameManager context) : base(context)
        {
            LogUtility.Info("InitGameState", "ctor");
        }

        public override void Enter()
        {
            LogUtility.Info("InitGameState", "Enter");
            InitGame();
            StartSpawnGameObjects();
        }

        public override void Exit()
        {
            LogUtility.Info("InitGameState", "Exit");
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
            LogUtility.Info("InitGameState", "OnSpawnedGameobjects");
            var tuple = (Tuple<object, object>)obj;
            _context.CarController = tuple.Item1 as CarController;
            _context.OpponentCarController = tuple.Item2 as CarController;
            _context.ChangeToStartGameState();
        }

        private void StartSpawnGameObjects()
        {
            LogUtility.Info("InitGameState", "StartSpawnGameObjects");
            Sequence seq = DOTween.Sequence().AppendInterval(_initInterval).AppendCallback(OnStartSpawnGameObjects)
                .SetAutoKill(true);
            seq.Play();
            // _context.StartCoroutine(IEStartSpawnGameObjects());
        }

        // private IEnumerator IEStartSpawnGameObjects()
        // {
        //     LogUtility.Info("InitGameState", "IEStartSpawnGameObjects");
        //     if (!PubSub.HasInstance)
        //     {
        //         yield return new WaitForSeconds(_initInterval);
        //     }
        //     LogUtility.Info("InitGameState", $"IEStartSpawnGameObjects PubSub.HasInstance: {PubSub.HasInstance}");
        //     _context.Register(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
        //     _context.carSpawner.gameObject.SetActive(true);
        //     _context.Broadcast(EventID.OnStartInitGame);
        // }
        private void OnStartSpawnGameObjects()
        {
            LogUtility.Info("InitGameState", $"OnStartSpawnGameObjects PubSub.HasInstance: {PubSub.HasInstance}");
            _context.PubSubRegister(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
            _context.carSpawner.gameObject.SetActive(true);
            _context.PubSubBroadcast(EventID.OnStartInitGame);
        }

    }

}
