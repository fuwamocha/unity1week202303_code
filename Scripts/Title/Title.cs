using System.Collections.Generic;
using DG.Tweening;
using InGame;
using MochaLib.Audio;
using MochaLib.Scene;
using UniRx;
using Unity1week202303.InGame;
using Unity1week202303.Score;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Unity1week202303.Title
{
    public class Title : MonoBehaviour
    {
        [SerializeField] private StageController stageController;
        [SerializeField] private TimeManager timeManager;
        [SerializeField] private AudioClip titleAudioClip;
        [SerializeField] private AudioClip gameAudioClip;
        [SerializeField] private BgmPlayer bgmPlayer;
        [SerializeField] private SePlayer sePlayer;
        [SerializeField] private CanvasGroup _fadeInCanvas;
        [SerializeField] private CanvasGroup _fadeOutCanvas;
        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Button startButton;
        [SerializeField] private AudioClip _seAudioClip;
        private EventTrigger _eventTrigger;
        private Image _startButtonImage;

        private void Awake()
        {
            _fadeInCanvas.alpha = 0f;
            AudioPlayer.Initialize(bgmPlayer, sePlayer);
            AudioPlayer.Bgm.Play(titleAudioClip, true);
            _startButtonImage = startButton.GetComponent<Image>();
            _eventTrigger = startButton.GetComponent<EventTrigger>();
            _eventTrigger.triggers = new List<EventTrigger.Entry>();
        }

        private void OnEnable()
        {
            startButton.onClick.AddListener(ChangeButtonScale);
            SubscribeHoverEvent();
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            _eventTrigger.triggers.Clear();
        }

        private void SubscribeHoverEvent()
        {
            _eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            });
            _eventTrigger.triggers[0].callback.AddListener(data => ChangeImage(true));
            _eventTrigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
                callback = new EventTrigger.TriggerEvent()
            });
            _eventTrigger.triggers[1].callback.AddListener(data => ChangeImage(false));
        }

        private void ChangeButtonScale()
        {
            AudioPlayer.Se.Play(_seAudioClip, false);
            stageController.Initialize();
            var sceneTransition = new SceneTransition();
            sceneTransition.CrossFade(_fadeOutCanvas, _fadeInCanvas, 0.5f).Subscribe(_ =>
            {
                if (TutorialPop.IsDidTutorial)
                {
                    timeManager.StartCount();
                }
                AudioPlayer.Bgm.Play(gameAudioClip, true);
            });
        }

        private void ChangeImage(bool isHover)
        {
            startButton.transform.DOScale(isHover ? 0.6f * 1.1f : 0.6f, 0.3f).SetEase(Ease.OutElastic);
            _startButtonImage.sprite = isHover ? hoverSprite : normalSprite;
        }
    }
}
