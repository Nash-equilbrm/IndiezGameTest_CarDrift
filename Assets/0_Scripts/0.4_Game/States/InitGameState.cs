using DG.Tweening;
using Game.Car;
using Patterns;
using System;
using System.Collections;
using UnityEngine;


namespace Game.States
{
    public class InitGameState : State<GameManager>
    {
        private float _initTime = 0.5f;
        public InitGameState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            InitGame();
            StartSpawnGameObjects();
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
        }

        private void InitGame()
        {
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
            _context.StartCoroutine(IEStartSpawnGameObjects());
        }

        private IEnumerator IEStartSpawnGameObjects()
        {
            while (!PubSub.HasInstance)
            {
                yield return null;
            }
            _context.Register(EventID.OnSpawnedGameobjects, OnSpawnedGameobjects);
            _context.carSpawner.gameObject.SetActive(true);
            _context.Broadcast(EventID.OnStartInitGame);
        }

    }

}
