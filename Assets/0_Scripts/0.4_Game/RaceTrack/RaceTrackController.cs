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
            this.PubSubRegister(EventID.OnGameStartCounting, OnGameStartCounting);
            this.PubSubRegister(EventID.OnStartInitGame, OnStartInitGame);
        }

        private void OnDisable()
        {
            this.PubSubUnregister(EventID.OnGameStartCounting, OnGameStartCounting);
            this.PubSubUnregister(EventID.OnStartInitGame, OnStartInitGame);
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
            this.PubSubBroadcast(EventID.OnFinishCounting);
        }

        public void OnCounting()
        {
            this.PubSubBroadcast(EventID.OnCounting);
        }

    }
}