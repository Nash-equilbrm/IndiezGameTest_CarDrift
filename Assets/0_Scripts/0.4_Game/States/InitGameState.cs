using DG.Tweening;
using Game.Car;
using Patterns;
using System;
using UnityEngine;


namespace Game.States
{
    public class InitGameState : State<GameManager>
    {
        public InitGameState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            InitGame();
            _context.Register(EventID.OnSpawnedCars, OnSpawnedCars);
            _context.Broadcast(EventID.StartSpawningCars);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.OnSpawnedCars, OnSpawnedCars);
        }

        private void OnSpawnedCars(object obj)
        {
            var tuple = (Tuple<object, object>)obj;
            _context.CarController = tuple.Item1 as CarController;

            _context.ChangeToStartGameState();
        }

        private void InitGame()
        {
            DOTween.Init(recycleAllByDefault: false, // Prevent issues in short-lived playable ads
                useSafeMode: true, // Prevents crashes
                LogBehaviour.ErrorsOnly)
                .SetCapacity(50, 10);


            bool isLowEndDevice = SystemInfo.systemMemorySize < 3000 ||  // Less than 3GB RAM
                         SystemInfo.processorCount <= 4 ||     // 4 or fewer CPU cores
                         SystemInfo.graphicsMemorySize < 500;  // Less than 500MB VRAM

            Application.targetFrameRate = isLowEndDevice ? 30 : 60;
        }

    }

}
