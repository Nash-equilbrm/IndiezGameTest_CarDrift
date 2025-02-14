using Game.Car;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.RaceTrack
{
    public class CarSpawner : MonoBehaviour
    {
        public GameObject playerPrefab;
        public GameObject opponentPrefab;
        public Transform spawnPlayerPosition;
        public Transform spawnOpponentPosition;


        internal void Spawn()
        {
            GameObject playerCar = Instantiate(playerPrefab, parent: GameManager.Instance.World);
            playerCar.transform.SetPositionAndRotation(spawnPlayerPosition.position, Quaternion.identity);


            //GameObject opponentCar = Instantiate(opponentPrefab, parent: GameManager.Instance.World);
            //opponentCar.transform.SetPositionAndRotation(spawnOpponentPosition.position, Quaternion.identity);
            object param = Tuple.Create(
                (object)playerCar.GetComponent<CarController>(),
                new object());
                //(object)opponentCar.GetComponent<CarController>());
            this.Broadcast(EventID.OnSpawnedCars, param);
        }
    }
}