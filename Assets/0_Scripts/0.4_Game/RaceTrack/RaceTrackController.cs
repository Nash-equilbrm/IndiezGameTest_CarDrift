using Game.Car;
using Game;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.RaceTrack
{
    public class RaceTrackController : MonoBehaviour
    {
        [SerializeField] private RaceTrackAnimation _animationController;
        [SerializeField] private CarSpawner _carSpawner;
        private void OnEnable()
        {
            this.Register(EventID.OnGameStartCounting, OnGameStartCounting);
            this.Register(EventID.OnStartInitGame, OnStartInitGame);
        }

        private void OnDisable()
        {
            this.Unregister(EventID.OnGameStartCounting, OnGameStartCounting);
            this.Unregister(EventID.OnStartInitGame, OnStartInitGame);
        }

        private void OnStartInitGame(object obj)
        {
            if (_carSpawner == null) return;
            _carSpawner?.Spawn();
        }
        
        private void OnGameStartCounting(object obj)
        {
            _animationController.PlayStartLineCountingAnim();
        }

        public void OnFinishCounting()
        {
            this.Broadcast(EventID.FinishCounting);
        }

    }
}