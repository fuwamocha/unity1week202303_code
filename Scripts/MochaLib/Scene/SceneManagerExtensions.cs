using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MochaLib.Scene
{
    public static class SceneManagerExtensions
    {
        private static AsyncOperation _asyncScene;

        /// <summary>
        ///     シーンをロードする
        /// </summary>
        /// <param name="sceneName">シーン名</param>
        public static void Load(Enum sceneName)
        {
            var scene = sceneName.ToString();

            SceneManager.LoadScene(scene);
        }

        /// <summary>
        ///     シーンを非同期でロードする
        /// </summary>
        /// <param name="isActivate">すぐに遷移するかどうか</param>
        public static void LoadAsync(Enum sceneName, bool isActivate)
        {
            var scene = sceneName.ToString();

            _asyncScene = SceneManager.LoadSceneAsync(scene);
            _asyncScene.allowSceneActivation = isActivate;
        }

        /// <summary>
        ///     非同期でロードしたシーンに遷移する
        /// </summary>
        public static void ActivateScene()
        {
            if (_asyncScene == null)
            {
                throw new NullReferenceException("Scene is not loaded.");
            }
            _asyncScene.allowSceneActivation = true;
        }
    }
}
