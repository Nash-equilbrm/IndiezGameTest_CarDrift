using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Commons
{
    public static class Common
    {
        public static float ScreenWidthInUnit => Camera.main.orthographicSize * 2 * Screen.width / Screen.height;
        public static float ScreenHeightInUnit => Camera.main.orthographicSize * 2;

        public static T ReadJson<T>(string filePath)
        {
            if (filePath == null || filePath.Length == 0 || !File.Exists(filePath)) return default;

            string json = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(json);

            return data;
        }

        public static T ReadJson<T>(TextAsset file)
        {
            if (file == null) return default;

            T data = JsonUtility.FromJson<T>(file.text);

            return data;
        }

        public static void LoadSceneAsync(MonoBehaviour context, int sceneIndex, LoadSceneMode loadSceneMode, System.Action callback)
        {
            context.StartCoroutine(IELoadSceneAsync(sceneIndex, loadSceneMode, callback));
        }


        private static IEnumerator IELoadSceneAsync(int sceneIndex, LoadSceneMode loadSceneMode, System.Action callback)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
            asyncLoad.allowSceneActivation = true;

            while (!asyncLoad.isDone)
            {
                yield return null; 
            }

            callback?.Invoke();
        }


        public static void UnloadSceneAsync(MonoBehaviour context, int sceneIndex, System.Action callback = null, bool gcCollect = false)
        {
            context.StartCoroutine(IEUnloadSceneAsync(sceneIndex, callback, gcCollect));
        }

        private static IEnumerator IEUnloadSceneAsync(int sceneIndex, System.Action callback, bool gcCollect)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneIndex);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }

            callback?.Invoke();
            if (gcCollect)
            {
                Resources.UnloadUnusedAssets();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        public static T GetRandomItem<T>(T[] array)
        {
            if (array == null || array.Length == 0) return default;
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
    }


    public static class Constants
    {
        public static int SCENE_JUMP_DASH = 1;
        public static int SCENE_KNIFE_HIT = 2;
        public static string JUMP_DASH_PLAYER_RUN_ANIM = "Ninja Run";
        public static string JUMP_DASH_PLAYER_JUMP_ANIM = "Ninja Jump";
        public static string JUMP_DASH_PLAYER_DEAD_ANIM = "Ninja Dead";

        public static string STR_BLOCK_TAG = "Block";
        public static string STR_KNIFE_TAG = "Knife";
        public static string STR_TARGET_TAG = "Target";
        public static string STR_BONUS_TAG = "Bonus";
        public static string STR_BEST_SCORE = "Best Score";
        public static string STR_SCORE = "Score";
        public static string STR_ENEMY_TAG = "Enemy";

        public static float CAMERA_SHAKE_HEAVY = 1f;
        public static float CAMERA_SHAKE_MEDIUM = 0.5f;
        public static float CAMERA_SHAKE_LIGHT = 0.1f;

        public static float KNIFE_HIT_TARGET_SPIN_LERP_SPEED = 0.2f;
    }
}


