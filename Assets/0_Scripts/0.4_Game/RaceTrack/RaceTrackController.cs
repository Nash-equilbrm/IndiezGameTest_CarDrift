using Game.Car;
using Game;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Commons;


namespace Game.RaceTrack
{
    public class RaceTrackController : MonoBehaviour
    {
        public RaceTrackAnimation animationController = null;
        public CarSpawner carSpawner = null;
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
            if (carSpawner == null) return;
            LogUtility.Info("RaceTrackController", "OnStartInitGame");
            carSpawner?.Spawn();
        }
        
        private void OnGameStartCounting(object obj)
        {
            animationController.PlayStartLineCountingAnim();
        }

        public void OnFinishCounting()
        {
            this.Broadcast(EventID.FinishCounting);
        }

    }
}