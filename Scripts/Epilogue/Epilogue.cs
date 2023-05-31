using InGame;
using MochaLib.Audio;
using MochaLib.Scene;
using UniRx;
using Unity1week202303.Score;
using Unity1week202303.System;
using UnityEngine;
using UnityEngine.UI;
namespace Unity1week202303.Epilogue
{
    public class Epilogue : MonoBehaviour
    {
        [SerializeField] private StageController stageController;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private NovelManager novelManager;
        [SerializeField] private AudioClip titleAudioClip;
        [SerializeField] private AudioClip gameAudioClip;
        [SerializeField] private CanvasGroup _titleCanvas;
        [SerializeField] private CanvasGroup _mainCanvas;
        [SerializeField] private CanvasGroup _fadeOutCanvas;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _titleButton;
        [SerializeField] private AudioClip _seAudioClip;
        private void Awake()
        {
            var sceneTransition = new SceneTransition();
            _titleButton.onClick.AddListener(() =>
            {
                AudioPlayer.Se.Play(_seAudioClip, false);
                stageController.Initialize();
                sceneTransition.CrossFade(_fadeOutCanvas, _titleCanvas, 0.5f).Subscribe(_ => AudioPlayer.Bgm.Play(titleAudioClip, true));
            });
            _continueButton.onClick.AddListener(() =>
            {
                AudioPlayer.Se.Play(_seAudioClip, false);
                stageController.Initialize();
                sceneTransition.CrossFade(_fadeOutCanvas, _mainCanvas, 0.5f).Subscribe(_ =>
                {
                    timeManager.StartCount();
                    AudioPlayer.Bgm.Play(gameAudioClip, true);
                });
            });

            TimeManager.OnClear.Subscribe(_ =>
            {
                switch (TimeManager.ClearTime)
                {
                    case <= 55.0f:
                        novelManager.SetTextFilePath("Perfect");
                        break;
                    case <= 65.0f:
                        novelManager.SetTextFilePath("Great");
                        break;
                    case <= 75.0f:
                        novelManager.SetTextFilePath("Normal");
                        break;
                    default:
                        novelManager.SetTextFilePath("Bad");
                        break;
                }
                novelManager.Init();
            });
        }
    }
}
