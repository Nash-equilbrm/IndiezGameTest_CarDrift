using Commons;
using Game.AI;
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
        public CheckpointManager checkpointManager = null;

        public void Spawn()
        {
            GameObject playerCar = SpawnPlayer();
            GameObject opponentCar = SpawnOpponent();
            if (checkpointManager != null) { 
                opponentCar.GetComponent<OpponentAIController>().checkpoints = checkpointManager.Checkpoints;
            }
            else
            {
                LogUtility.InvalidInfo("CarSpawner", "Missing checkpointManager ref");
            }
            object param = Tuple.Create((object)playerCar.GetComponent<CarController>(), (object)opponentCar.GetComponent<CarController>());
            //new object());
            LogUtility.InvalidInfo("CarSpawner", "Broadcast(EventID.OnSpawnedGameobjects)");
            this.PubSubBroadcast(EventID.OnSpawnedGameobjects, param);
        }

        private GameObject SpawnPlayer()
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
            return playerCar;
        }

        private GameObject SpawnOpponent()
        {
            GameObject opponentCar;
            if (GameManager.HasInstance && GameManager.Instance.OpponentCarController != null)
            {
                opponentCar = GameManager.Instance.OpponentCarController.gameObject;
            }
            else
            {
                opponentCar = Instantiate(opponentPrefab, parent: GameManager.Instance.World);
            }
            opponentCar.gameObject.SetActive(false);
            opponentCar.transform.SetPositionAndRotation(spawnOpponentPosition.position, Quaternion.identity);
            opponentCar.gameObject.SetActive(true);
            return opponentCar;
        }
    }
}