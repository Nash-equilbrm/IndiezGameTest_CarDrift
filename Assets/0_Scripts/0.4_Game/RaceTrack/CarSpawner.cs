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
        public GameObject playerPrefab = null;
        public GameObject opponentPrefab = null;
        public Transform spawnPlayerPosition = null;
        public Transform spawnOpponentPosition = null;


        public void Spawn()
        {
            GameObject playerCar;
            if (GameManager.HasInstance && GameManager.Instance.CarController != null)
            {
                playerCar = GameManager.Instance.CarController.gameObject;
            }
            else
            {
                playerCar = Instantiate(playerPrefab, parent: GameManager.Instance.World);
            }
            playerCar.gameObject.SetActive(false);
            playerCar.transform.SetPositionAndRotation(spawnPlayerPosition.position, Quaternion.identity);
            playerCar.gameObject.SetActive(true);


            //GameObject opponentCar = Instantiate(opponentPrefab, parent: GameManager.Instance.World);
            //opponentCar.transform.SetPositionAndRotation(spawnOpponentPosition.position, Quaternion.identity);
            object param = Tuple.Create((object)playerCar.GetComponent<CarController>(), new object());
            //(object)opponentCar.GetComponent<CarController>());
            this.Broadcast(EventID.OnSpawnedGameobjects, param);
        }
    }
}